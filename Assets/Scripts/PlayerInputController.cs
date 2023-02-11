using UnityEngine;
namespace ProjectA
{
    public class PlayerInputController : MonoBehaviour
    {
        public float RelativeAngle { get; private set; }
        public Vector3 InputDirection { get; private set; }
        public Vector2 Direction { get; private set; }

        private void Update()
        {
            Vector2 keyboardMovement = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Direction = keyboardMovement;
            ProcessInputDirection();
        }
        private void ProcessInputDirection()
        {
            RelativeAngle = Mathf.Atan2(Direction.x,
                Direction.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            InputDirection = Quaternion.Euler(0f, RelativeAngle, 0f) * Vector3.forward;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, InputDirection.normalized);
        }
    }
}