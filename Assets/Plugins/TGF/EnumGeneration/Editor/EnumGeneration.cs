using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEditor;

namespace TGF.EnumGeneration.Editor
{
    internal static class EnumGeneration
    {
        internal static void ModifyEnum([NotNull] Type enumType, [NotNull] string relativeFileLocation, [NotNull] string newValue, Action<int> onSuccess = null, string firstValue = "CreateNew")
        {
            if (string.IsNullOrEmpty(newValue) || string.IsNullOrWhiteSpace(newValue) || newValue.Contains(" ")) 
                throw new ArgumentNullException(nameof(newValue) + " is invalid");

            List<string> allEnumValues = Enum.GetNames(enumType).ToList();
            allEnumValues.Add(newValue);
            
            CreateEnumFromList(enumType, relativeFileLocation, allEnumValues, onSuccess);
        }

        internal static void CreateEnumFromList([NotNull] Type enumType, [NotNull] string relativeFileLocation,
            List<string> allEnumValues, Action<int> onSuccess = null)
        {
            if (enumType == null) throw new ArgumentNullException(nameof(enumType));
            if (!enumType.IsEnum) throw new ArgumentException(nameof(enumType) + " is not an enum");
            
            string fullFilePath = Path.GetFullPath(relativeFileLocation);
            if (!File.Exists(fullFilePath)) throw new FileNotFoundException(fullFilePath);
            
            string newEnumText = GenerateEnumString(enumType.Name, enumType.Namespace, allEnumValues, "");
            
            File.WriteAllText(relativeFileLocation, newEnumText);
            onSuccess?.Invoke(allEnumValues.Count-1);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static string GenerateEnumString(string enumName, string namespaceName, List<string> list, string firstValue)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("namespace ").Append(namespaceName).Append("{\n\t");
            sb.Append("public enum ").Append(enumName).Append("{\n");

            if (!string.IsNullOrEmpty(firstValue) && !string.IsNullOrWhiteSpace(firstValue))
            {
                if (list.Contains(firstValue)) list.Remove(firstValue);
                list.Insert(0, firstValue);
            }
            for (int i = 0; i < list.Count; i++)
            {
                
                string enumVal = list[i];
                sb.Append("\t\t").Append(enumVal).Append(",\n");
            }

            sb.Append("\t}\n}");
            return sb.ToString();
        }
    }
}