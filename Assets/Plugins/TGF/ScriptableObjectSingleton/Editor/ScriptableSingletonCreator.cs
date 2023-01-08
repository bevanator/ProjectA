using System;
using System.IO;
using System.Text;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace TGF.ScriptableObjectSingleton.Editor
{
	public class ScriptableSingletonCreator : OdinEditorWindow
	{
		[MenuItem("Thunder Games/New Scriptable Singleton")]
		public static void OpenWindow()
		{
			var window = GetWindow<ScriptableSingletonCreator>();
			window.Show();
		}

		public string Namespace;
		public string SingletonClass;
		[FolderPath] public string ClassLocation;

		[Button]
		public void CreateSingleton()
		{
			EditorPrefs.SetBool("SingletonCreationInProgress", true);
			EditorPrefs.SetString("SingleCreationData", Namespace + "." + SingletonClass);
			string datapath = Application.dataPath;
			string template = File.ReadAllText(datapath +
			                                   "/Plugins/Thunder Games/Editor/ScriptableObjectSingleton/ScriptableSingletonTemplate.txt");
			template = template.Replace("#Namespace#", Namespace);
			template = template.Replace("#SingletonClass#", SingletonClass);

			datapath = datapath.Replace("Assets", string.Empty);
			File.WriteAllText(datapath + "/" + ClassLocation + "/" + SingletonClass + ".cs", template, Encoding.UTF8);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		[DidReloadScripts]
		private static void CreateSingletonAsset()
		{
			if (EditorPrefs.GetBool("SingletonCreationInProgress"))
			{
				string singletonCreationData = EditorPrefs.GetString("SingleCreationData");
				EditorPrefs.DeleteKey("SingleCreationData");
				EditorPrefs.DeleteKey("SingletonCreationInProgress");
				var names = singletonCreationData.Split('.');
				Type type = Type.GetType(names[1]);
				ScriptableObject so = ScriptableObject.CreateInstance(singletonCreationData);
				if (!AssetDatabase.IsValidFolder("Assets/Resources"))
				{
					AssetDatabase.CreateFolder("Assets", "Resources");
				}

				if (!AssetDatabase.IsValidFolder("Assets/Resources/Singletons"))
				{
					AssetDatabase.CreateFolder("Assets/Resources", "Singletons");
				}

				AssetDatabase.CreateAsset(so, "Assets/Resources/Singletons/" + names[1] + "Singleton.asset");
				AssetDatabase.SaveAssets();
			}
		}
	}
}