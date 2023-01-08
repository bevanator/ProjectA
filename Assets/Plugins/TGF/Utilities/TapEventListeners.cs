using UnityEngine;
using UnityEngine.Events;

namespace TGF.Utilities
{
    public class TapEventListeners : MonoBehaviour
    {
        [SerializeField] private float m_Timeout;
        [SerializeField] private float m_DistanceLimit;

        [SerializeField] private RectTransform m_TouchRegionRect;


        public TapEvent TapEvent = new TapEvent();

        private Vector2 _touchStartedAtPos;
        private float _touchStartedAtTime;

        private Rect _touchRectScreenSpace;

        private Touch _touch;
        private Camera cam;


        private void Start()
        {
            UpdateTouchPos();
        }

        public void UpdateTouchPos()
        {
            if (m_TouchRegionRect != null)
            {
                cam = GetComponentInParent<Canvas>().worldCamera;

                Vector3 touchRegionPos = m_TouchRegionRect.transform.position;
                Rect touchRegionRect = m_TouchRegionRect.rect;
                Vector2 corner_br = cam.WorldToScreenPoint(new Vector3(touchRegionPos.x, touchRegionPos.y));
                Vector2 corner_tl = corner_br + new Vector2(-touchRegionRect.width, touchRegionRect.height);

                _touchRectScreenSpace.x = corner_tl.x;
                _touchRectScreenSpace.y = corner_br.y;
                _touchRectScreenSpace.width = corner_br.x - corner_tl.x;
                _touchRectScreenSpace.height = corner_tl.y;
            }
        }

        void Update()
        {
            if (Input.touchCount > 0)
            {
                _touch = Input.touches[Input.touchCount - 1];

                if (_touch.phase == TouchPhase.Began)
                {
                    _touchStartedAtPos = _touch.position;
                    _touchStartedAtTime = Time.unscaledTime;
                }
                else if (_touch.phase == TouchPhase.Ended)
                {
                    if (Time.unscaledTime < _touchStartedAtTime + m_Timeout
                        && Vector2.Distance(_touchStartedAtPos, _touch.position) / Screen.dpi < m_DistanceLimit
                       )
                    {
                        // tap successful
                        if (m_TouchRegionRect == null
                            || _touchRectScreenSpace.Contains(_touchStartedAtPos))
                        {
                            TapEvent.Invoke(_touch.position);
                        }
                    }
                }
            }
        }
    }


    public class TapEvent : UnityEvent<Vector2>
    {
    }
}