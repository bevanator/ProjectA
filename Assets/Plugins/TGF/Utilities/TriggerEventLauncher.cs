using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TGF.Utilities
{
    [RequireComponent(typeof(Collider))]
    public class TriggerEventLauncher : MonoBehaviour
    {
        public event Action<GameObject, Collider> OnTriggerEnterEvent, OnTriggerExitEvent, OnTriggerStayEvent;

        // Unity does not fire exit event for collider that gets disabled
        // this bool will fix that issue
        [SerializeField] private bool m_UseAdvanceCheckForExit;
        
        private List<Collider> _inTouch = new List<Collider>();
        private List<Collider> _inTouchPrevious = new List<Collider>();
    
        private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();

        private void Awake()
        {
            
        }

        private void OnEnable()
        {
            if (m_UseAdvanceCheckForExit) StartCoroutine(checkIntouch());
        }

        private void FixedUpdate()
        {
            //Debug.Log("Fixed Update");
            if(m_UseAdvanceCheckForExit) _inTouch.Clear();
        }
        
        private void OnDestroy()
        {
            if(m_UseAdvanceCheckForExit) StopCoroutine(checkIntouch());
        }
        
        IEnumerator checkIntouch()
        {
            while (true)
            {
                yield return _waitForFixedUpdate;
                //yield return Timing.WaitUntilDone(new CoroutineHandle(_waitForFixedUpdate));
                //Debug.Log("Checking");

                if (_inTouch.Count < _inTouchPrevious.Count)
                {
                    // lost entry

                    foreach (Collider _collider in _inTouchPrevious)
                    {
                        if (!_inTouch.Contains(_collider))
                        {
                            //Debug.Log("Advance Exit: " + _collider.gameObject.name);
                            OnTriggerExitEvent?.Invoke(gameObject, _collider);
                        }
                    }
                }

                //_inTouchPrevious = _inTouch.ToList();
                if(_inTouch.Count > 0)
                    _inTouchPrevious = new List<Collider>(_inTouch);
                else
                    _inTouchPrevious.Clear();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterEvent?.Invoke(gameObject, other);
        }

        private void OnTriggerExit(Collider other)
        {
            if(!m_UseAdvanceCheckForExit)
                OnTriggerExitEvent?.Invoke(gameObject, other);
        }

        private void OnTriggerStay(Collider other)
        {
            OnTriggerStayEvent?.Invoke(gameObject, other);

            if (m_UseAdvanceCheckForExit)
            {
                if(!_inTouch.Contains(other))
                    _inTouch.Add(other);    
            }
        }
    }
}