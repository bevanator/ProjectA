using UnityEngine;

namespace TGF.Utilities
{
    public class BillboardFacing : MonoBehaviour
    {
        private Camera _mainCam;

        Quaternion originalRotation;

        //public Vector3 originalEuler;

        void Start()
        {
            originalRotation = transform.rotation;
            _mainCam = Camera.main;
        }

        void LateUpdate()
        {
            if (!_mainCam) _mainCam = Camera.main;
            if(!_mainCam) return;
            transform.rotation = _mainCam.transform.rotation; // * originalRotation;
        }
    }
}