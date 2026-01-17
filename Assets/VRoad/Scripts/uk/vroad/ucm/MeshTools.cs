using System;
using uk.vroad.apk;
using UnityEditor;
using UnityEngine;

namespace uk.vroad.ucm
{
    /// <summary> A class containing static Utility methods to assist with drawing of Meshes </summary>
    public class MeshTools: ScriptableObject
    {
#if UNITY_EDITOR
        private static string vroadRootRel = SA.EXPECTED_PKG_LOC;
        private static bool checkedRoot = false;
        
        
        /// <summary> Return root directory of our package relative to project root (parent of Assets/ ) <br/>
        /// This is normally Assets/VRoad, but our package might not have been installed there.
        /// Editor tools such as EditorSceneManager use  paths relative to the Project folder </summary> 
        public static string VRoadRoot()
        {
            if (checkedRoot) return vroadRootRel;
            checkedRoot = true;

            bool asExpected = false;
            try
            {
                MeshTools env = CreateInstance<MeshTools>();
                KFile kenvScriptFile = new KFile(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(env)));
                //                                 ucm   / vroad  / uk     / Scripts/ VRoad
                KDir vroadDir = kenvScriptFile.Directory().Parent().Parent().Parent().Parent();
                string vroadPath = vroadDir.FullPath();
                string vroadRootRelActual = vroadPath.Substring(vroadPath.IndexOf(SA.ASSETS_DIR));

                if (vroadRootRel.Equals(vroadRootRelActual)) asExpected = true;
                else vroadRootRel = vroadRootRelActual;
            }
            catch (Exception) { asExpected = false; }

            if (!asExpected) Debug.LogWarning(SA.UNEXPECTED_PKG_LOC + SA.EXPECTED_PKG_LOC);
			
            return vroadRootRel;
        }
#endif
        

    }
}
