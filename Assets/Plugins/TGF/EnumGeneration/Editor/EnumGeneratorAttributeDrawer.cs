using System;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace TGF.EnumGeneration.Editor
{
	[DrawerPriority(DrawerPriorityLevel.WrapperPriority)]
	public class EnumGeneratorAttributeDrawer : OdinAttributeDrawer<EnumGeneratorAttribute>
	{
		private string newEnumMember;
		protected override void DrawPropertyLayout(GUIContent label)
		{
			Enum @enum = (Enum)Property.ValueEntry.WeakSmartValue;
			Property.ValueEntry.WeakSmartValue =
				SirenixEditorFields.EnumDropdown(label, @enum);

			int value = (int)Property.ValueEntry.WeakSmartValue;
			if (value == 0)
			{
				SirenixEditorGUI.BeginIndentedVertical(SirenixGUIStyles.BoxContainer, GUILayout.Height(EditorGUIUtility.singleLineHeight * 2 + 6));
				newEnumMember = SirenixEditorFields.TextField("New Entry", newEnumMember);
				bool isValidNAme = IsNameValid(Attribute.EnumType, newEnumMember);
				EditorGUI.BeginDisabledGroup(!isValidNAme);
				if (GUILayout.Button("Create New"))
				{
					EnumGeneration.ModifyEnum(Attribute.EnumType, Attribute.EnumFile, newEnumMember, i =>
					{
						Property.ValueEntry.WeakSmartValue = i;
					});
				}
				EditorGUI.EndDisabledGroup();
				SirenixEditorGUI.EndIndentedVertical();
			}
		}
		

		private static bool IsNameValid(Type enumType, string name)
		{
			string[] names = Enum.GetNames(enumType);
			if (name == null) return false;
			return !names.ToList().Contains(name)
			       && !name.Contains(" ") // name can't contain space
			       && !string.IsNullOrEmpty(name) && !string.IsNullOrWhiteSpace(name);
		}

		public override bool CanDrawTypeFilter(Type type)
		{
			return type.IsEnum;
		}
	}
}