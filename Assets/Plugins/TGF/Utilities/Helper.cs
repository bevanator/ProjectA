using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using TMPro;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace TGF.Utilities
{
    public static class Helper
    {
     
        public const long NUMBER_1_TRILLION       = 1000000000000; //1,000,000,000,000;
        public const long NUMBER_1_BILLION        = 1000000000;    //1,000,000,000;
        public const long NUMBER_1_MILLION        = 1000000;       //1,000,000;
        public const long NUMBER_1_THOUSAND       = 1000;          //1,000;
        
        private static Tweener _TimeTweener;

        private static NavMeshPath _navMeshPath = new NavMeshPath(); 
        
        public static bool IsIOS()
        {
#if UNITY_IOS
            return true;
#endif

            return false;
        }
	
        public static bool IsAndroid()
        {
#if UNITY_ANDROID
				return true;
#endif

#pragma warning disable CS0162
            return false;
#pragma warning restore CS0162
        }
	
        public static bool IsStandAlone()
        {
#if UNITY_STANDALONE
				return true;
#endif

            return false;
        }
	
        public static bool IsStandAloneOSX()
        {
#if UNITY_STANDALONE_OSX
				return true;
#endif

            return false;
        }
	
        public static bool IsStandAloneWIN()
        {
#if UNITY_STANDALONE_WIN
				return true;
#endif
		
            return false;
        }
	
        public static bool IsStandAloneLinux()
        {
#if UNITY_STANDALONE_LINUX
				return true;
#endif
		
            return false;
        }
        
        
        public static bool IsTouchDevice()
        {
            return IsIOS() || IsAndroid();
        }

        public static bool IsMobilePlatform()
        {
            return IsIOS() || IsAndroid();
        }
        
        
        public static bool IsIphoneX()
        {
            if((Application.isEditor || Application.platform == RuntimePlatform.IPhonePlayer)
               && Screen.width == 1125)
            {
                return true;
            }

            return false;
        }

        public static bool IsNarrowDisplay()
        {
            // the display is too narrow. example Samsung S8 or iPhoneX
            return IsIphoneX() || ((Screen.width / (float)Screen.height) < 0.56f);
        }

        public static bool IsShortDisplay()
        {
            float minSize = 4.3f;

            if((float)Screen.height / Screen.dpi < minSize)
            {
                // this is a short phone
                //Debug.Log("returned true");
                return true;
            }

            return false;
        }

        public static bool IsBigTablet()
        {
            if (Application.isEditor) return false;

#if UNITY_IOS || UNITY_ANDROID
            //Debug.Log("ScreenDPI: " + Screen.dpi);
            //Debug.Log("ScreenWidth: " + Screen.width);
            //Debug.Log("(float)Screen.width / Screen.dpi: " + ((float)Screen.width / Screen.dpi));

            float minSize = 5f;

            if((float)Screen.width / Screen.dpi > minSize)
            {
                // this is a big tablet
                //Debug.Log("returned true");
                return true;
            }
#endif
            return false;
        }
        
        public static GameObject CreateDebugCube(Vector3 pos, float size, Color color, Transform parent = null)
        {
            return CreateDebugCube(pos, Vector3.one * size, color, parent);
        }
        
        public static GameObject CreateDebugCube(Vector3 pos, Vector3 size, Color color, Transform parent = null)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            
            if(parent != null)
                cube.transform.parent = parent;
            
            cube.transform.position = pos;
            cube.transform.localScale = size;
            cube.GetComponent<Renderer>().material.color = color;

            return cube;
        }
        
        public static void DrawDebugCircle(Vector3 center, float radius, Color color)
        {
            Vector3 prevPos = center + new Vector3(radius, 0, 0);
            for (int i = 0; i < 30; i++)
            {
                float angle = (float)(i + 1) / 30.0f * Mathf.PI * 2.0f;
                Vector3 newPos = center + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                Debug.DrawLine(prevPos, newPos, color);
                prevPos = newPos;
            }
        }
        
        
        
        
        public static long GetRoundUpNumbers(long number)
        {
            if (number < NUMBER_1_THOUSAND)
                return number;
            
            //Debug.Log("number: " + number);
            long num = NUMBER_1_THOUSAND;
            if (number >= NUMBER_1_BILLION)
            {
                num = NUMBER_1_BILLION;
            }
            else if (number >= NUMBER_1_MILLION)
            {
                num = NUMBER_1_MILLION;
            }

            int d = Mathf.FloorToInt(number / num);
            
            //Debug.Log("d: " + d);
            long r = number % num;
            
            int t = Mathf.FloorToInt( Mathf.FloorToInt(r / (num * 0.1f)) * (num * 0.1f) );
            //Debug.Log("t: " + t);

            number = (long) (d * num + t);
            //Debug.Log("new: " + number);
            return number;
        }

        public static Tweener AnimateNumberForTextRoundUp(this TextMeshProUGUI txt, int from, int to, float duration, string prefix = "$", string suffix = "", TweenCallback onComplete = null)
        {
            return DOVirtual.Int(from, to, duration, value =>
            {
                txt.text = prefix + GetRoundUpNumbersAsString(value) + suffix;
            }).OnComplete(onComplete).SetLink(txt.gameObject);
        }

        public static string GetRoundUpNumbersAsString(long number)
        {
            number = GetRoundUpNumbers(number);
            
            if (number < NUMBER_1_THOUSAND)
                return number.ToString();
            
            if (number >= NUMBER_1_BILLION)
            {
                return (number / (float)NUMBER_1_BILLION) + "B";
            }
            
            if (number >= NUMBER_1_MILLION)
            {
                return (number / (float)NUMBER_1_MILLION) + "M";
            }
            
            return (number / (float)NUMBER_1_THOUSAND) + "K";
        }

        public static int GetRoundUpBasedOnFirstFewDigits(int number, int firstDigitsLength)
        {
            string numberS = number.ToString();
            if (numberS.Length < firstDigitsLength) return number;

            string result = numberS.Substring(0, firstDigitsLength);
            StringBuilder sb = new StringBuilder(result);
            
            int rest = numberS.Length - firstDigitsLength;

            for (int i = 0; i < rest; i++)
            {
                sb.Append("0");
            }

            result = sb.ToString();
            
            return int.Parse(result);
        }
        
        public static int GetDigitsCount(int n)
        {
            return n == 0 ? 1 : (n > 0 ? 1 : 2) + (int)Math.Log10(Math.Abs((double)n));
        }
        


        public static void PauseTime(float duration, System.Action func = null)
        {
            _TimeTweener = DOVirtual.Float(
                        Time.timeScale, 
                        0, 
                        duration, 
                        value => Time.timeScale = value)
                    .SetUpdate(true)
                    .OnComplete(delegate
                    {
                        func?.Invoke();
                    })
                ;
        }

        public static void UnPauseTime()
        {
            if(_TimeTweener != null) _TimeTweener.Kill();
            Time.timeScale = 1;
        }

        public static string GetSentenceFromCamelCase(string camel)
        {
            camel = Regex.Replace(camel, "([a-z])([A-Z])", "$1$2");
            camel = Regex.Replace(camel, "([A-Z])([a-z])", " $1$2");
            return camel;
        }

        public static Rect RectTransformToScreenSpace(RectTransform transform)
        {
            Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
            
            return new Rect(new Vector2(transform.position.x - size.x, transform.position.y), size);
        }
        
         public static bool IsTouchingUIElement(Vector2 pos, GameObject allowedObject = null)
        {
            // if(EventSystem.current.IsPointerOverGameObject())
            // {
            //     return true; // yes mouse is over some UI elevent
            // }

            // the above code doesn't work for touch. so we need to use raycast into ui element
            if(Input.touchCount > 0)
            {
                PointerEventData pointer = new PointerEventData(EventSystem.current);
                pointer.position =  pos;
                List<RaycastResult> listRayCastResult = new List<RaycastResult>();
			
                EventSystem.current.RaycastAll(pointer, listRayCastResult);

                if (allowedObject != null)
                {
                    bool onlyTouchedAllowedObject = false;
                    foreach(RaycastResult rcr in listRayCastResult)
                    {
                        if (rcr.gameObject == allowedObject || rcr.gameObject.transform.IsChildOf(allowedObject.transform))
                        {
                            onlyTouchedAllowedObject = true;
                        }
                        else
                        {
                            //Debug.Log("Touched: " + rcr.gameObject.name);
                            return true;
                        }
                    }

                    if (onlyTouchedAllowedObject) return false;
                }
                else
                {
                    if(listRayCastResult.Count > 0) return true; // yes there was UI element
                }

                
            }

            return false;
        }

        public static bool IsTouchingUIElement(GameObject obj, Vector2 pos)
        {
            if(Input.touchCount > 0)
            {
                PointerEventData pointer = new PointerEventData(EventSystem.current);
                pointer.position =  pos;
                List<RaycastResult> listRayCastResult = new List<RaycastResult>();

                EventSystem.current.RaycastAll(pointer, listRayCastResult);
                
                if(listRayCastResult.Count == 0) return false; 

                foreach(RaycastResult rcr in listRayCastResult)
                {
                    if(rcr.gameObject == obj || rcr.gameObject.transform.IsChildOf(obj.transform))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        
        public static Tweener AnimateNumberForText(this TextMeshProUGUI txt, int from, int to, float duration, string prefix = "$", string suffix = "", TweenCallback onComplete = null)
        {
            return DOVirtual.Int(from, to, duration, value =>
            {
                txt.text = prefix + value + suffix;
            }).OnComplete(onComplete).SetLink(txt.gameObject);
        }

        static IEnumerator AnimateNumberForTextCorutine(TextMeshProUGUI txt, int from, int to, float duration, string prefix = "$", Action onComplete = null)
        {
            float timeStarted = Time.time;

            long val = from;
            
            while (Time.time < timeStarted + duration)
            {
                val = (long) (from + (to - from) * (Time.time - timeStarted) / duration);
                txt.text = prefix + val;
                //Debug.Log(txt.text + ": " + val);
                yield return new WaitForEndOfFrame();
            }
            
            txt.text = prefix + to;
            
            onComplete?.Invoke();
        }

        
        
        public static Vector2 GetScaledUIPosFromScreenPos(Canvas scaledCanvas, Vector2 screenPos)
        {
            return screenPos / scaledCanvas.transform.localScale.x;
        }
        
        

        public static Button TopUIButtonUnderPointer(Vector2 pos)
        {
            if(Input.touchCount > 0)
            {
                PointerEventData pointer = new PointerEventData(EventSystem.current);
                pointer.position =  pos;
                List<RaycastResult> listRayCastResult = new List<RaycastResult>();

                EventSystem.current.RaycastAll(pointer, listRayCastResult);
                
                foreach(RaycastResult rcr in listRayCastResult)
                {
                    //Debug.Log("Touched: " + rcr.gameObject.name);
                    Button btn = rcr.gameObject.GetComponent<Button>();
                    if (btn != null) return btn;
                }
            }

            return null;
        }


        public static bool IsInLayerMask(int layer, LayerMask layerMask)
        {
            return layerMask == (layerMask | (1 << layer));
        }


        public static void DeleteAllChilds(Transform trans)
        {
            int childCount = trans.childCount;
            for (int i = 0; i < childCount; i++)
            {
                GameObject.DestroyImmediate(trans.GetChild(0).gameObject);
            }
        }



        public static Vector3 ChangeX(this Vector3 vector, float newX)
        {
            Vector3 newVector = new Vector3(newX, vector.y, vector.z);
            return newVector;
        }
        
        public static Vector3 ChangeY(this Vector3 vector, float newY)
        {
            Vector3 newVector = new Vector3(vector.x, newY, vector.z);
            return newVector;
        }
        
        public static Vector3 ChangeZ(this Vector3 vector, float newZ)
        {
            Vector3 newVector = new Vector3(vector.x, vector.y, newZ);
            return newVector;
        }


        public static string FullObjectPath(GameObject go)
        {
            return go.transform.parent == null ? go.name : FullObjectPath(go.transform.parent.gameObject) + "/" + go.name;
        }
        
        
        public static float Remap (float value, float fromMin, float fromMax, float toMin,  float toMax)
        {
            var fromAbs  =  value - fromMin;
            var fromMaxAbs = fromMax - fromMin;      
       
            var normal = fromAbs / fromMaxAbs;
 
            var toMaxAbs = toMax - toMin;
            var toAbs = toMaxAbs * normal;
 
            var to = toAbs + toMin;
       
            return to;
        }
        
        
        private static float RemapClamped(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            float z = Mathf.InverseLerp(fromMin, fromMax, value);
            float remap = Mathf.Lerp(toMin, toMax, z);
            return remap;
        }
        
        
        public static float GetNavPathLength(Vector3 fromPos, Vector3 toPos)
        {
            float lng = 0.0f;
            
            _navMeshPath.ClearCorners();

            if (NavMesh.CalculatePath(fromPos, toPos, NavMesh.AllAreas, _navMeshPath))
            {
                lng = GetNavPathLength(_navMeshPath);
            }
            
            return lng;
        }


        public static float GetNavPathLength(NavMeshPath navMeshPath)
        {
            float distance = 0;
            if (( navMeshPath.status == NavMeshPathStatus.PathComplete ) && ( navMeshPath.corners.Length > 1 ))
            {
                for ( int i = 1; i < navMeshPath.corners.Length; ++i )
                {
                    distance += Vector3.Distance( navMeshPath.corners[i-1], navMeshPath.corners[i] );
                }
            }

            return distance;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = RandomNumber.GetNextRandomNumber(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        
        
        public static float NextFloat(this System.Random rand, float min, float max)
        {
            double d = rand.NextDouble();
            d = (max - min) * d + min;
            return (float)d;
        }
        
#if UNITY_ANDROID
    public static int GetAndroidSDKLevel() {
        using (var version = new AndroidJavaClass("android.os.Build$VERSION")) {
            return version.GetStatic<int>("SDK_INT");
        }
    }
    #else
    public static int GetAndroidSDKLevel() {
        return 0;
    }
#endif
        
        
    }
}
