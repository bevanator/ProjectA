using System;
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
        private bool _restrictMovement;
        
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
        private static readonly int Slash1 = Animator.StringToHash("Slash1");
        private static readonly int RunningSword = Animator.StringToHash("RunningSword");

        [SerializeField] private SwordController m_SwordController;


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
            HandleAnimations();
            if (_characterController.isGrounded)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Jump();
                    m_CanDoubleJump = true;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    Slash();
                    RestrictMovement();
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

        private void RestrictMovement()
        {
            _restrictMovement = true;
        }
        private void UnRestrictMovement()
        {
            _restrictMovement = false;
        }

        private void ProcessInputDirection()
        {
            _relativeAngle = Mathf.Atan2(_playerInputController.Direction.x,
                _playerInputController.Direction.y) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
            _inputDirection = Quaternion.Euler(0f, _relativeAngle, 0f) * Vector3.forward;
        }
        private void Move()
        {
            if(_restrictMovement) return;
            if (_playerInputController.Direction.magnitude > 0.1f)
            {
                if (!_isMoving)
                {
                    _isMoving = true;
                    if (_isGrounded) StartRunAnim();
                }
            }
            
            if (!(_playerInputController.Direction.magnitude > 0.1f))
            {
                if (_isMoving)
                {
                    _isMoving = false;
                    StopRunAnim();
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
        }
        private void HandleAnimations()
        {
            if (!_characterController.isGrounded && !_isFloating)
            {
                _isFloating = true;
                StopRunAnim();
                _animator.SetTrigger(Floating);
            }
            if (_characterController.isGrounded && _isFloating)
            {
                _isFloating = false;
                _animator.SetTrigger(Land);
                if (_isMoving)
                {
                    _animator.ResetTrigger(Land);
                    StartRunAnim();
                }
            }
        }
        private void Jump()
        {
            UnRestrictMovement();
            StopRunAnim();
            _animator.SetTrigger(Jump1);
            _isFloating = true;
            _velocity.y = Mathf.Sqrt(m_JumpHeight * -2f * m_Gravity);
        }
        private void Slash()
        {
            _isMoving = false;
            StopRunAnim();
            _animator.SetTrigger(Slash1);
        }
        private void StartRunAnim()
        {
            if (!m_SwordController.SwordOut) _animator.SetBool(Running, true);
            else _animator.SetBool(RunningSword, true);
        }
        private void StopRunAnim()
        {
            if (!m_SwordController.SwordOut) _animator.SetBool(Running, false);
            else _animator.SetBool(RunningSword, false);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, _inputDirection.normalized);
        }
        private void OnGUI()
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 30;
            GUI.Label(new Rect(1200, 100, 300, 300), "Restriction Status: " + _restrictMovement, style);
            GUI.Label(new Rect(1200, 150, 300, 300), "Ground Check: " + _isGrounded, style);
            GUI.Label(new Rect(1200, 200, 300, 300), "Sword Status: " + m_SwordController.SwordOut, style);
            GUI.Label(new Rect(1200, 250, 300, 300), "Movement Status : " + _isMoving, style);
        }
    }
}