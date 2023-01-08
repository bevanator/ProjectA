using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TGF.ScriptableObjectSingleton
{
    public abstract class ScriptableSingletonBase<T> : SerializedScriptableObject where T : SerializedScriptableObject
    {
        private static T _instance = null;

        public static T Instance
        {
            get
            {
                if (!_instance) InitializeSingleton();
                return _instance;
            }
        }
        
        protected static void InitializeSingleton()
        {
            _instance = Resources.LoadAll<T>("Singletons").FirstOrDefault();
            if (_instance != null) Debug.Log("Singleton Initialized : " + _instance.name);
            else Debug.LogError("Could not initialize Singleton of type " + typeof(T));
        }
    }
}