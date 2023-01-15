using System;
using UnityEngine;
namespace ProjectA
{
    public class SwordController : MonoBehaviour
    {
        [SerializeField] private Transform m_Spine;
        [SerializeField] private Transform m_Wrist;
        [SerializeField] private Transform m_Katana;

        private void Start()
        {
            
        }
        public void DrawSword()
        {
            m_Katana.parent = m_Wrist;
            m_Katana.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        public void SheathSword()
        {
            m_Katana.parent = m_Spine;
            m_Katana.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }
}