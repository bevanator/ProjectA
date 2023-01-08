using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace TGF.KeystoreSetup.Editor
{
    public class KeystoreSetup : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
#if UNITY_ANDROID
            SetupKeystore(); 
#endif
        }
    
        public static void SetupKeystore()
        {
            PlayerSettings.Android.useCustomKeystore = true;
            string path = Application.dataPath +  "/../Keys/key.keystore";

            string passFilePath = Application.dataPath +  "/../Keys/pass.txt";
            if (File.Exists(passFilePath))
            {
                string pass = File.ReadAllText(passFilePath);

                PlayerSettings.Android.keystoreName = path;
                PlayerSettings.Android.keystorePass = pass;
                PlayerSettings.Android.keyaliasName = "release";
                PlayerSettings.Android.keyaliasPass = pass;    
            }
        }
    }
}