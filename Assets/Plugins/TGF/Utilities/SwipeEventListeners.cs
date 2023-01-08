using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace TGF.Utilities
{
    public class SwipeEventListeners : MonoBehaviour
    {
        [SerializeField] private bool m_FingerUpRequired;
        
        [SerializeField] private bool m_UseTimeout;
        
        [SerializeField, ShowIf("m_UseTimeout", false)] private float m_Timeout;
        [SerializeField] private float m_DistanceMin;

        [SerializeField] private RectTransform m_TouchRegionRect;
        

        public UnityEvent<Vector2, Direction> SwipeEvent;
        public UnityEvent<Vector2> TouchStartedEvent;
        public UnityEvent<Vector2> TouchMovedEvent;
        public UnityEvent<Vector2> TouchEndedEvent;

        private bool _firstTouch;
        
        public enum Direction
        {
            None, Up, Right, Down, Left   
        }

        private Vector2 _touchStartedAtPos;
        private float _touchStartedAtTime;

        private Rect _touchRectScreenSpace;

        private Touch _touch;

        public Vector2 CurrentTouchPos { get; protected set; }

        private void Start()
        {
            if (m_TouchRegionRect != null)
            {
                _touchRectScreenSpace = Helper.RectTransformToScreenSpace(m_TouchRegionRect);
            }
        }

        void Update()
        {
            EmulateWithKeyboard();
            
            if (Input.touchCount > 0)
            {
                _touch = Input.touches[Input.touchCount - 1];
                //Debug.Log("Phase: " + _touch.phase);

                CurrentTouchPos = _touch.position;

                if (_touch.phase == TouchPhase.Began)
                {
                    _touchStartedAtPos = _touch.position;
                    _touchStartedAtTime = Time.unscaledTime;

                    if (!_firstTouch)
                    {
                        _firstTouch = true;
                        TouchStartedEvent?.Invoke(_touchStartedAtPos);
                    }
                }
                else if (_touch.phase == TouchPhase.Moved)
                {
                    //Debug.Log("Moved");
                    if (!m_FingerUpRequired)
                    {
                        if (SendSwipeEvent(_touch.position))
                            _touchStartedAtPos = _touch.position;
                    }

                    TouchMovedEvent?.Invoke(_touch.position);
                }
                else if (_touch.phase == TouchPhase.Ended || _touch.phase == TouchPhase.Canceled)
                {
                    SendSwipeEvent(_touch.position);


                    if (Input.touchCount == 1)
                    {
                        TouchEndedEvent?.Invoke(_touch.position);
                    }
                }
            }
            else
            {
                _firstTouch = false;
            }
        }


        private void EmulateWithKeyboard()
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                SwipeEvent.Invoke(_touchStartedAtPos, Direction.Up);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                SwipeEvent.Invoke(_touchStartedAtPos, Direction.Down);
            } 
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                SwipeEvent.Invoke(_touchStartedAtPos, Direction.Right);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SwipeEvent.Invoke(_touchStartedAtPos, Direction.Left);
            }
        }


        private bool SendSwipeEvent(Vector2 currentTouchPos)
        {
            bool result = false;
            Vector2 temp = currentTouchPos - _touchStartedAtPos;
                    
            if ((!m_UseTimeout || Time.unscaledTime < _touchStartedAtTime + m_Timeout)
                && temp.magnitude / Screen.dpi > m_DistanceMin
               )
            {
                // tap successful
                if (m_TouchRegionRect == null
                    || _touchRectScreenSpace.Contains(_touchStartedAtPos))
                {
                    if (Mathf.Abs(temp.x)  > Mathf.Abs(temp.y))
                    {
                        if (temp.x > 0)
                        {
                            SwipeEvent.Invoke(_touchStartedAtPos, Direction.Right);
                            result = true;
                        }
                        else
                        {
                            SwipeEvent.Invoke(_touchStartedAtPos, Direction.Left);
                            result = true;
                        }
                    }
                    else
                    {
                        if (temp.y > 0)
                        {
                            SwipeEvent.Invoke(_touchStartedAtPos, Direction.Up);
                            result = true;
                        }
                        else
                        {
                            SwipeEvent.Invoke(_touchStartedAtPos, Direction.Down);
                            result = true;
                        }
                    }

                    //Debug.Log("Swipe Action: " + Time.unscaledTime);
                }
            }

            return result;
        }
    }
}