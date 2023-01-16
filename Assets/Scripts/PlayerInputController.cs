using UnityEngine;
namespace ProjectA
{
    public class PlayerInputController : MonoBehaviour
    {
        public Vector2 Direction { get; private set; }
        
        private void Update()
        {
            Vector2 keyboardMovement = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Direction = keyboardMovement;
            
        }
    }
}