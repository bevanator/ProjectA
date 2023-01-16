using System;
using Sirenix.OdinInspector;
using UnityEngine;
namespace ProjectA
{
    public class SwordController : MonoBehaviour
    {
        [SerializeField] private Transform m_Spine;
        [SerializeField] private Transform m_Wrist;
        [SerializeField] private Transform m_Katana;
        [ShowInInspector] public bool SwordOut { get; private set; }

        private void Start()
        {
            
        }
        public void DrawSword()
        {
            m_Katana.parent = m_Wrist;
            m_Katana.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            SwordOut = true;
        }

        public void SheathSword()
        {
            m_Katana.parent = m_Spine;
            m_Katana.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            SwordOut = false;
        }
    }
}