using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TGF.Utilities
{
	public static class GameObjectUtility
	{
		
		public static GameObject InstantiatePrefab(GameObject prefab, Transform parentTransform)
		{
#if UNITY_EDITOR
			return PrefabUtility.InstantiatePrefab(prefab, parentTransform) as GameObject;
#endif
#pragma warning disable CS0162
			return Object.Instantiate(prefab, parentTransform) as GameObject;
#pragma warning restore CS0162
		}

		public static Object InstantiatePrefab(Object prefab, Transform parentTransform)
		{
#if UNITY_EDITOR
			return PrefabUtility.InstantiatePrefab(prefab, parentTransform);
#endif
#pragma warning disable CS0162
			return Object.Instantiate(prefab, parentTransform);
#pragma warning restore CS0162
		}
	}
}