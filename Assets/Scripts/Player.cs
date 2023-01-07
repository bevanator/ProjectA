using System;
using UnityEngine;
using UnityEngine.Serialization;
namespace ProjectA
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float m_Speed;
        private bool _error;

        private void Start()
        {
            What();
        }
        private void What()
        {
            
        }
    }
}