using DG.Tweening;
using Sirenix.OdinInspector;
using TGF.Utilities;
using UnityEngine;

namespace TGF.UI
{
    public class UIPanel : MonoBehaviour
    {
        [SerializeField, Required, BoxGroup("$groupTitle")] protected Canvas m_Canvas;
        [SerializeField, Required, BoxGroup("$groupTitle")] protected CanvasGroup m_Panel;

        [SerializeField, BoxGroup("$groupTitle")] protected float m_AnimateInDuration;
        [SerializeField, BoxGroup("$groupTitle")] protected float m_AnimateOutDuration;
        [SerializeField, BoxGroup("$groupTitle")] protected float m_ShowDelay;
        
        [SerializeField, BoxGroup("$groupTitle")] protected bool m_HasModal;
        [SerializeField, BoxGroup("$groupTitle"), ShowIf("m_HasModal")] protected CanvasGroup m_ModalCanvasGroup;
        
        [SerializeField, BoxGroup("$groupTitle")] protected bool m_UsePopAnimation;
        [SerializeField, BoxGroup("$groupTitle"), ShowIf("m_UsePopAnimation")] protected RectTransform m_PopRect;
        
        [SerializeField, BoxGroup("$groupTitle")] protected bool m_PushUpBottomForShortDevice;
        [SerializeField, BoxGroup("$groupTitle"), ShowIf("m_PushUpBottomForShortDevice")] protected RectTransform m_PushUpRect;
        
#pragma warning disable CS0414
        [Space] private string groupTitle = "Inherited Members : UIPanel";
#pragma warning restore CS0414
        protected RectTransform PanelRect;
        
        

        protected void Awake()
        {
            PanelRect = m_Panel.GetComponent<RectTransform>();
            m_Panel.interactable = false;
            m_Canvas.gameObject.SetActive(false);

            // if (m_PushUpBottomForShortDevice && m_PushUpRect != null && Helper.IsShortDisplay())
            // {
            //     m_PushUpRect.offsetMin = new Vector2(0, 80);
            // }
        }

        public virtual void Show()
        {
            // Debug.Log(Helper.FullObjectPath(gameObject));
            // Debug.Log("Show");
            m_Panel.alpha = 0f;

            if (m_ShowDelay > 0)
                DOVirtual.DelayedCall(m_ShowDelay, ShowAfterDelay, true);
            else 
                ShowAfterDelay();
            
            transform.SetAsLastSibling();
        }


        private void ShowAfterDelay()
        {
            m_Canvas.gameObject.SetActive(true);
            m_Canvas.enabled = true;
            m_Panel.blocksRaycasts = true;

            if (m_HasModal)
            {
                if (m_UsePopAnimation)
                {
                    m_ModalCanvasGroup.alpha = 1;
                }
                else
                {
                    m_ModalCanvasGroup.alpha = 0;
                    m_ModalCanvasGroup.DOFade(1, m_AnimateInDuration).SetUpdate(true);    
                }
                
            }

            if (m_UsePopAnimation)
            {
                PopIn(delegate
                {
                    m_Panel.interactable = true;
                    OnShowCompleted();
                });
            }
            else
            {
                AnimateIn(delegate { 
                    m_Panel.interactable = true;
                    OnShowCompleted();
                });    
            }
        }

        protected virtual void OnShowCompleted()
        {
            m_Canvas.enabled = true;
        }

        public virtual void Hide()
        {
            // Debug.Log(Helper.FullObjectPath(gameObject));
            // Debug.Log("Hide");
            m_Panel.interactable = false;
            
            if (m_HasModal)
            {
                m_ModalCanvasGroup.DOFade(0, m_AnimateInDuration).SetUpdate(true);
            }

            if (m_UsePopAnimation)
            {
                PopOut(delegate
                {
                    //m_Canvas.enabled = false;
                    m_Panel.blocksRaycasts = false;
                    m_Canvas.gameObject.SetActive(false);
                    OnHideCompleted();
                });
            }
            else
            {
                AnimateOut(delegate
                {
                    //m_Canvas.enabled = false;
                    m_Panel.blocksRaycasts = false;
                    m_Canvas.gameObject.SetActive(false);
                    OnHideCompleted();
                });
            }
        }

        protected virtual void OnHideCompleted()
        {
            
        }

        public bool IsVisible()
        {
            return m_Canvas.gameObject.activeSelf;
        }

        protected void PopIn(TweenCallback onComplete)
        {
            m_Panel.alpha = 1;
            m_PopRect.localScale = Vector3.zero;
            
            m_PopRect.DOScale(Vector3.one, m_AnimateInDuration)
                .SetEase(Ease.OutBack)
                .SetUpdate(true)
                .OnComplete(onComplete).SetUpdate(true);
        }
        
        protected void PopOut(TweenCallback onComplete)
        {
            m_PopRect.DOScale(Vector3.zero, m_AnimateOutDuration)
                .SetEase(Ease.InBack)
                .SetUpdate(true)
                .OnComplete(onComplete).SetUpdate(true);
        }

        protected void AnimateOut(TweenCallback onComplete)
        {
            m_Panel.DOFade(0, m_AnimateOutDuration).OnComplete(onComplete).SetUpdate(true);
        }

        protected void AnimateIn(TweenCallback onComplete)
        {
            m_Panel.DOFade(1, m_AnimateInDuration).OnComplete(onComplete).SetUpdate(true);
        }
    }
}