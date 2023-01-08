using System;
using Plugins.TGF.SceneSerialization;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using TGF.Utilities;
using UnityEngine;

[DrawerPriority(DrawerPriorityLevel.WrapperPriority)]
public class SerializedSceneAssetSettingsAttributeDrawer : OdinAttributeDrawer<SerializeSceneAssetSettingsAttribute>
{
	private bool _foldout;
	protected override void DrawPropertyLayout(GUIContent label)
	{
		SerializedSceneAsset sceneAsset = (SerializedSceneAsset)Property.ValueEntry.WeakSmartValue;
		InspectorProperty sceneAssetProperty = Property.Children.Get("m_EditorSceneAsset");

		AssetSelectorAttribute attribute = sceneAssetProperty.GetAttribute<AssetSelectorAttribute>();
		
		if(!string.IsNullOrEmpty(Attribute.Paths)) attribute.Paths = Attribute.Paths;
		
		sceneAssetProperty.Draw(label);


		if (Attribute.ShowPath)
		{
			InspectorProperty property = Property.Children.Get("m_ScenePath");
			property.Draw();
		}
		
		if (Attribute.ShowBuildIndex)
		{
			InspectorProperty property = Property.Children.Get("m_BuildIndex");
			property.Draw();
		}

		InspectorProperty addToBuildListButton = Property.Children.Get("AddToBuildList");
		bool addedToBuildList = (bool)ReflectionHelper.GetFieldValue(sceneAsset, "_addedToBuildList");
		if (addToBuildListButton is not null && sceneAssetProperty.ValueEntry.WeakSmartValue is not null && !addedToBuildList)
		{
			SirenixEditorGUI.InfoMessageBox("Scene is not added to the build list!");
			addToBuildListButton.Draw();
		}
		
		if (Attribute.ShowOpenButton)
		{
			InspectorProperty property = Property.Children.Get("OpenScene");
			property.Draw();
		}
		
		if (Attribute.ShowRefreshButton)
		{
			InspectorProperty property = Property.Children.Get("Refresh");
			property.Draw();
		}
	}

	public override bool CanDrawTypeFilter(Type type) => type == typeof(SerializedSceneAsset);
}