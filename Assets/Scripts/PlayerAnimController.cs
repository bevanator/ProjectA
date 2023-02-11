using System;
using ProjectA.Hash;
using UnityEngine;
namespace ProjectA
{
    public class PlayerAnimController : MonoBehaviour
    {
        
        [SerializeField] private SwordController m_SwordController;
        private Animator _animator;
        private void Awake()
        {
            m_SwordController = GetComponent<SwordController>();
            _animator = GetComponentInChildren<Animator>();
        }
        private void Start()
        {
            
        }
        public void StartRunAnim()
        {
            if (!m_SwordController.SwordOut) _animator.SetBool(AnimHash.Running, true);
            else _animator.SetBool(AnimHash.RunningSword, true);
        }
        public void StopRunAnim()
        {
            if (!m_SwordController.SwordOut) _animator.SetBool(AnimHash.Running, false);
            else _animator.SetBool(AnimHash.RunningSword, false);
        }
        public void FloatAnim() => _animator.SetTrigger(AnimHash.Floating);
        public void LandAnim() => _animator.ResetTrigger(AnimHash.Land);
        public void Slash1Anim() => _animator.SetTrigger(AnimHash.Slash1);
        public void JumpAnim() => _animator.SetTrigger(AnimHash.Jump1);
    }
}