using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Plugins.TGF.SceneSerialization.Editor
{
	public class SerializedSceneAssetDrawer : OdinValueDrawer<SerializedSceneAsset>
	{
		protected override void DrawPropertyLayout(GUIContent label)
		{
			InspectorProperty property = Property.Children.Get("m_EditorSceneAsset");
			property?.Draw(label);
		}
	}
}