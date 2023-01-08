using System;
using System.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace TGF.SceneLoading
{
    public class SceneTransition : MonoBehaviour
    {
        [SerializeField] private Canvas m_Canvas;
        public Image m_TransitionImage;
        public float m_TransitInDuration, m_TransitOutDuration;
        public Ease m_TransitInEase, m_TransitOutEase;
        public static SceneTransition Instance { get; private set; }

        public bool IsSceneTransiting { get; private set; }

        public event Action OnSceneTransitionInCompleted;
        public event Action OnSceneTransitionOutCompleted;

        public Material m_SceneTransitionMaterial;
        private static readonly int Radius = Shader.PropertyToID("_Radius");
        private static readonly int MaxRadius = Shader.PropertyToID("_RadiusMax");

        

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                return;
            }

            m_SceneTransitionMaterial = m_TransitionImage.material;
            
            
            float maxSide = Mathf.Max(Screen.width, Screen.height);
            m_TransitionImage.rectTransform.sizeDelta = new Vector2(maxSide, maxSide);
            float maxRadius = Vector2.Distance(Vector2.zero, new Vector2(Screen.width, Screen.height));
            m_SceneTransitionMaterial.SetFloat(MaxRadius, maxRadius);
// #if UNITY_EDITOR
//             //m_SceneTransitionMaterial.SetFloat(Radius, maxRadius);
// #else
//             m_SceneTransitionMaterial.SetFloat(Radius, 1);
// #endif
            TransitIn().GetAwaiter();
        }


        /// <summary> 
        /// Used when a scene has been loaded to reveal the content
        /// </summary>
        [Button]
        public async Task TransitIn()
        {
            m_Canvas.gameObject.SetActive(true);
            await DOVirtual.Float(0f, 1f, m_TransitInDuration,
                    (float value) => { m_SceneTransitionMaterial.SetFloat(Radius, value); }).SetEase(m_TransitInEase)
                .OnComplete(delegate
                {
                    m_Canvas.gameObject.SetActive(false);
                    DOVirtual.DelayedCall(0.2f, delegate
                    {
                        IsSceneTransiting = false;
                        OnSceneTransitionInCompleted?.Invoke();
                    });
                })
                .AsyncWaitForCompletion();
        }

        /// <summary>
        /// Used before unloading a scene to hide the content 
        /// </summary>
        [Button]
        public async Task TransitOut()
        {
            IsSceneTransiting = true;
            m_Canvas.gameObject.SetActive(true);
            await DOVirtual.Float(1f, 0f, m_TransitOutDuration,
                    (float value) => { m_SceneTransitionMaterial.SetFloat(Radius, value); })
                .OnComplete(delegate
                {
                    OnSceneTransitionOutCompleted?.Invoke();
                })
                .SetEase(m_TransitOutEase)
                .AsyncWaitForCompletion();
        }


        private void OnApplicationQuit()
        {
            if (m_SceneTransitionMaterial is null) return;
            
            m_SceneTransitionMaterial.SetFloat(Radius, 0);
            m_SceneTransitionMaterial.SetFloat(MaxRadius, 0);
        }
    }
}