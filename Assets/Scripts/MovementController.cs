using Sirenix.OdinInspector;
using UnityEngine;
namespace ProjectA
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private float m_MaxMovementSpeed = 2.0f;
        private float _speed;

        private Animator _animator;
        private PlayerInputController _playerInputController;
        private CharacterController _characterController;
        private float _verticalVelocity;
        private float _relativeAngle;
        private Vector3 _inputDirection;
        private Camera _camera;

        private Vector3 _velocity;
        
        [ShowInInspector, ReadOnly] private bool _isGrounded;
        [SerializeField] private float m_GroundCheckDistance;
        [SerializeField] private LayerMask m_GroundMasks;
        [SerializeField] private float m_Gravity;
        [SerializeField] private float m_JumpHeight;
        [SerializeField] private bool m_CanDoubleJump;
        [ShowInInspector, ReadOnly]
        private bool _isMoving;
        [ShowInInspector, ReadOnly]
        private bool _isRunningOnGround;
        [ShowInInspector, ReadOnly] private bool _isFloating;
        private static readonly int Running = Animator.StringToHash("Running");
        private static readonly int Jump1 = Animator.StringToHash("Jump");
        private static readonly int Land = Animator.StringToHash("Land");
        private static readonly int Floating = Animator.StringToHash("Float");


        protected void Awake()
        {
            _camera = Camera.main;
            _animator = GetComponentInChildren<Animator>();
            _playerInputController = GetComponent<PlayerInputController>();
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            ProcessInputDirection();
            Move();
            CheckGravityAndGround();
            if (_characterController.isGrounded)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Jump();
                    m_CanDoubleJump = true;
                }
            }
            // else
            // {
            //     if (Input.GetKeyDown(KeyCode.Space) && m_CanDoubleJump)
            //     {
            //         Jump();
            //         m_CanDoubleJump = false;
            //     }
            // }

        }
        private void ProcessInputDirection()
        {
            _relativeAngle = Mathf.Atan2(_playerInputController.Direction.x,
                _playerInputController.Direction.y) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
            _inputDirection = Quaternion.Euler(0f, _relativeAngle, 0f) * Vector3.forward;
        }
        private void Move()
        {
            if (_playerInputController.Direction.magnitude > 0.1f)
            {
                if (!_isMoving)
                {
                    _isMoving = true;
                    print("here");
                    if(_characterController.isGrounded) _animator.SetBool(Running, true);
                }
            }
            if (!(_playerInputController.Direction.magnitude > 0.1f))
            {
                if (_isMoving)
                {
                    _isMoving = false;
                    _animator.SetBool(Running, false);
                }
                return;
            }
            if (_isMoving)
            {
                _speed = m_MaxMovementSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Euler(0f, _relativeAngle, 0f);
                _characterController.Move(_speed * _inputDirection.normalized);
            }
        }
        private void CheckGravityAndGround()
        {
            _isGrounded = Physics.CheckSphere(transform.position, m_GroundCheckDistance, m_GroundMasks);
            if (_characterController.isGrounded && _velocity.y < 0f)
            {
                _velocity.y = -2f;
            }
            _velocity.y += m_Gravity * Time.deltaTime;
            _characterController.Move(Time.deltaTime * _velocity);
            if (!_characterController.isGrounded && !_isFloating)
            {
                _isFloating = true;
                _animator.SetBool(Running, false);
                _animator.SetTrigger(Floating);
            }
            if (_characterController.isGrounded && _isFloating)
            {
                _isFloating = false;
                if(_isMoving) _animator.SetBool(Running, true);
                _animator.SetTrigger(Land);
            }
        }
        private void Jump()
        {
            _animator.SetBool(Running, false);
            _animator.SetTrigger(Jump1);
            _isFloating = true;
            _velocity.y = Mathf.Sqrt(m_JumpHeight * -2f * m_Gravity);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, _inputDirection.normalized);
        }
    }
}