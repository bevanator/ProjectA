using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Plugins.TGF.SceneSerialization
{
	[Serializable]
	public class SerializedSceneAsset
	{
#if UNITY_EDITOR
		[SerializeField, OnValueChanged("SceneProvided"), AssetSelector] private SceneAsset m_EditorSceneAsset;
		[SerializeField, ReadOnly, DisplayAsString] private string m_ScenePath;
#endif
		[SerializeField, ReadOnly, DisplayAsString] private int m_BuildIndex;

#pragma warning disable CS0414
		private bool _addedToBuildList;
#pragma warning restore CS0414
		public int BuildIndex => m_BuildIndex;

#if UNITY_EDITOR
		
		[OnInspectorInit]
		public void InitializeEditor()
		{
			if(!CheckSceneAssetField()) return;
			string scenePath = AssetDatabase.GetAssetPath(m_EditorSceneAsset);
			if (string.IsNullOrEmpty(m_ScenePath) || m_ScenePath != scenePath)
				m_ScenePath = scenePath;
			SetBuildIndex();
		}

		private bool CheckSceneAssetField()
		{
			if (m_EditorSceneAsset is not null) return true;
			m_ScenePath = String.Empty;
			m_BuildIndex = 0;
			return false;
		}

		
		private void SceneProvided()
		{
			if(!CheckSceneAssetField()) return;
			string path = AssetDatabase.GetAssetPath(m_EditorSceneAsset);
			m_ScenePath = path;
			SetBuildIndex();
		}

		[Button, ShowIf("@_addedToBuildList == false && string.IsNullOrEmpty(m_ScenePath) == false")]
		public void AddToBuildList()
		{
			List<EditorBuildSettingsScene> scenes = EditorBuildSettings.scenes.ToList();
			scenes.Add(new EditorBuildSettingsScene(m_ScenePath, true));
			EditorBuildSettings.scenes = scenes.ToArray();
			m_BuildIndex = scenes.Count - 1;
			SetBuildIndex();
		}

		public void SetBuildIndex()
		{
			_addedToBuildList = false;
			EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
			for (int i = 0; i < scenes.Length; i++)
			{
				if (scenes[i].path != m_ScenePath) continue;
				_addedToBuildList = true;
				m_BuildIndex = i;
				break;
			}
		}

		[Button]
		private void OpenScene()
		{
			if (string.IsNullOrEmpty(m_ScenePath)) return;
			EditorSceneManager.OpenScene(m_ScenePath);
		}
		
		
		[Button]
		private void Refresh()
		{
			SetBuildIndex();
		}
#endif
	}
}