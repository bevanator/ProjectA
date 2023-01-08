using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace TGF.Utilities
{
    public class InternetReachabilityService : MonoBehaviour
    {
        private static bool _IsChecking = false;
        public static bool IsInternetReachable { get; private set; }

        public static InternetReachabilityService Instance;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            CheckConnection();
        }

        public static void CheckConnection()
        {
            if(!_IsChecking)
                Instance.StartCoroutine("CheckConnectionInternal");
        }

        private IEnumerator CheckConnectionInternal()
        {
            _IsChecking = true;
            const string url = "https://tg.studio/runtime";

            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                webRequest.timeout = 3;
                yield return webRequest.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log("NetworkError: " + webRequest.error);
                    IsInternetReachable = false;
                }
                else
                {
                    IsInternetReachable = true;
                }
            }
            
            _IsChecking = false;
        }
    }
}