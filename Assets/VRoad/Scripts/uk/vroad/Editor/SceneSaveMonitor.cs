using System.Collections.Generic;

namespace uk.vroad.Editor
{
    public interface LSceneSave
    {
        void OnWillSaveScene();
    }
    
    /// <summary>
    /// This Editor class catches user "Save Scene" actions and calls any listeners that are registered
    /// </summary>
    public class SceneSaveMonitor: UnityEditor.AssetModificationProcessor
    {
        private static readonly HashSet<LSceneSave> listeners = new HashSet<LSceneSave>();

        public static void register(LSceneSave listener)
        {
            listeners.Add(listener);
        }
        
        public static string[] OnWillSaveAssets(string[] paths)
        {
            if (listeners.Count == 0) return paths;
            
            bool sceneSaved = false;
            foreach (string path in paths)
            {
                if (path.EndsWith(".unity"))
                {
                    sceneSaved = true;
                    break;
                }
            }

            if (sceneSaved)
            {
                foreach (LSceneSave listener in listeners)
                {
                    listener.OnWillSaveScene();
                }
            }
            return paths;
        }
    }
}