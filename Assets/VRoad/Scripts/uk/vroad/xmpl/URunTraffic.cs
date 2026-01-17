using System;
using uk.vroad.api;
using uk.vroad.api.etc;
using uk.vroad.api.str;
using uk.vroad.api.xmpl;
using uk.vroad.apk;
using uk.vroad.pac;
using UnityEngine;
using UnityEngine.UI;

namespace uk.vroad.xmpl
{
    public class URunTraffic: MonoBehaviour, Reporter.IExternalReporter
    {
        private const string STREAMING_BASE = KC.ENV_ASSETS_DIR + KC.ENV_STREAMING_ASSETS_DIR;
        private const string MY_STREAMING_MAPS = KC.ENV_VROAD_STREAMING_DIR + KC.ENV_MAPS_DIR;
        
        public static URunTraffic MostRecentInstance { get; private set;  }

        [Tooltip("A path to the .vroad file, which can be an absolute " +
                 "path, a relative path or a simple filename. For a " +
                 "relative path, several locations are searched " +
                 "for the file, including the project root, the writable " +
                 "map generation folder and streaming maps folder. " +
                 "If you are using a Prefab, the file name must match " +
                 "the file used to create the prefab.")]
        public string vRoadFilePath;
        [Tooltip("A Text object on the Canvas used to show error messages")]
        public Text buildErrorText;
        [Tooltip("A Slider on the canvas used to show the progress of loading the .vroad file")]
        public Slider vRoadSlider;
        [Tooltip("The number of process threads assigned to simulation")]
        public int simulationWorkers = AppTools.N_WORKERS_INIT;
        [Tooltip("For the progress bar, set this to zero if using a Prefab")]
        public int UIProgressSteps = 1;
        [Tooltip("Switch this off to show only the last error on BuildErrorText")]
        public bool showAllErrors =  true;
        [Tooltip("Switch this on to see which locations are searched for the map file")]
        public bool showFileLocations = false;

        protected App app;
        protected bool hasErrorText;
        protected bool hasSlider;
        protected bool loadInProgress;
        protected int progressRaw;
        private string progressActivity;
        private int progressSmoothed;
        
       
        public void SetupTraffic(string path)
        {
            vRoadFilePath = path;
        }

        protected virtual void Awake()
        {
            app = ExampleApp.AwakeInstance();
            
            MostRecentInstance = this;
            hasSlider = vRoadSlider != null;
            hasErrorText = buildErrorText != null;
            if (hasErrorText) Reporter.SetExternalReporter(this);
            
            KEnv.Awake();
            LoadOnAwake();
        }

        protected virtual void LoadOnAwake()
        {
            KFile mapFile = FindMapFile();
            
            if (mapFile != null)
            {
                StartLoad(mapFile, UIProgressSteps); // .vroad
            }
            else
            {
                Reporter.Report("Failed to start. No VRoad file "+vRoadFilePath);
            }
        }

       
        public void Report(string s)
        {
            if (hasErrorText)
            {
                if (!buildErrorText.gameObject.activeSelf) buildErrorText.gameObject.SetActive(true);

                if (showAllErrors) buildErrorText.text = buildErrorText.text  + "\n" + s; 
                else               buildErrorText.text = s; // show last error only
            }

            Debug.Log(s);
        }

        protected virtual void LoadMap(KFile mapFile)
        {
            VRoad.Load(app, mapFile);
        }

        protected void StartLoad(KFile mapFile, int nui)
        {
            loadInProgress = true;
            progressRaw = 0;
            progressSmoothed = 0;
            progressActivity = "Loading Map";
            if (hasSlider) vRoadSlider.gameObject.SetActive(true);
            VRoad.SetWorkerCount(simulationWorkers);

            Reporter.ProgressPartsUI(nui);

            try
            {
                LoadMap(mapFile);
            }
            catch (Exception x)
            {
                Reporter.Report("Failed to Load Map: "+x.ToString());
        
            }
        }

        protected virtual void FixedUpdate()
        {
            if (loadInProgress)
            {
                progressRaw = Reporter.ProgressTotal();

                if (progressRaw < 100)
                {
                    int diff = (progressRaw * 100) - progressSmoothed;
                    if (diff > 0)
                    {
                        int inc = diff > 1000? 100: diff > 500? 20: 5;
                        progressSmoothed += inc;
                        if (hasSlider) vRoadSlider.value = progressSmoothed;
                    }
                }
                else
                {
                    if (hasSlider) vRoadSlider.gameObject.SetActive(false);
                    loadInProgress = false;
                }
            }
        }

        public int Progress() { return progressRaw; }
        public string ProgressActivity() { return progressActivity; }

        protected virtual KFile FindMapFile()
        {
            // KEnv.Awake() is called in Awake to initialize directories in Main Unity Thread
            
            if (vRoadFilePath == null) return null;
            vRoadFilePath = vRoadFilePath.Trim();
            if (vRoadFilePath.Length <= SC.SUFFIX_DOT_VROAD.Length) return null;

            try
            {
                KFile mapFile;

#if UNITY_EDITOR
                // vRoadFilePath can be an absolute path, a relative path, or a simple filename
                //
                // The editor windows for the BuildMap and RunTraffic scenes pre-fill vRoadFilePath
                // with a full absolute path name
                //
                // KFile uses System.IO.FileInfo, which can take either an absolute
                // path or a path relative to Environment.CurrentDirectory.
                // In Unity Editor, CurrentDirectory is the project root - one level above Assets 
                
                // Try as absolute path or as PROJECT_ROOT/ vRoadFilePath
                mapFile = new KFile(vRoadFilePath);
                if (MapFileOK(mapFile)) return mapFile;

                // Try looking for it in the VRoadStreamingAssets/Maps/ folder
                // This is normally Assets/StreamingAssets/VRoadStreamingAssets/Maps/
                // but can also be Assets/VRoad/VRoadStreamingAssets/Maps/
                // In this case vroadFilePath is just a filename
                mapFile = new KFile(KEnv.VroadReadDir(), vRoadFilePath);
                if (MapFileOK(mapFile)) return mapFile;

                // Try looking for it in the location where downloaded, generated maps are stored
                // In this case vroadFilePath is just a filename
                mapFile = new KFile(KEnv.VroadWriteDir(), vRoadFilePath);
                if (MapFileOK(mapFile)) return mapFile;

#else
                // In a built game, the easiest solution is to:
                // (1) Put the .vroad files into Assets/StreamingAssets/, or a subfolder,
                //     from where they will be bundled with the game.
                //     See https://docs.unity3d.com/Manual/StreamingAssets.html
                // (2) Set the variable to blank, for the StreamingAssets root, or to a subfolder,
                //     then the files will be found from that same location when running in the Editor

                mapFile = new KFile(Application.streamingAssetsPath+"/"+streamingAssetsFolder, vRoadFilePath);
                if (MapFileOK(mapFile)) return mapFile;
#endif
            }
            catch (Exception x) { Reporter.Report(x.Message); }

            return null;
        }

        protected bool MapFileOK(KFile mapFile)
        {
            bool ok = mapFile != null && mapFile.Exists() && AppTools.SuitableAppFile(mapFile);

            if (showFileLocations)
            {
                if (ok) Reporter.Report("Map file found at: " + mapFile);  // Report() also goes to Debug.Log()
                else    Reporter.Report("No map file found at: " + mapFile);
                
            }
            return ok;
        }
        

    }
}