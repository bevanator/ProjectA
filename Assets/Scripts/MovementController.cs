using System;
using ProjectA.Hash;
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
        [SerializeField] private SwordController m_SwordController;
        [SerializeField] private PlayerAnimController m_PlayerAnimController;
        


        protected void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _playerInputController = GetComponent<PlayerInputController>();
            _characterController = GetComponent<CharacterController>();
            m_PlayerAnimController = GetComponent<PlayerAnimController>();
        }

        private void Update()
        {
            Move();
            CheckGravityAndGround();
            HandleFloatAnimations();
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
        
        private void Move()
        {
            if(_restrictMovement) return;
            if (_playerInputController.Direction.magnitude > 0.1f)
            {
                if (!_isMoving)
                {
                    _isMoving = true;
                    if (_isGrounded) m_PlayerAnimController.StartRunAnim();
                }
            }
            if (!(_playerInputController.Direction.magnitude > 0.1f))
            {
                if (_isMoving)
                {
                    _isMoving = false;
                    m_PlayerAnimController.StopRunAnim();
                }
                return;
            }
            if (_isMoving)
            {
                _speed = m_MaxMovementSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Euler(0f, _playerInputController.RelativeAngle, 0f);
                _characterController.Move(_speed * _playerInputController.InputDirection.normalized);
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
        private void HandleFloatAnimations()
        {
            if (!_characterController.isGrounded && !_isFloating)
            {
                _isFloating = true;
                m_PlayerAnimController.StopRunAnim();
                m_PlayerAnimController.FloatAnim();
            }
            if (_characterController.isGrounded && _isFloating)
            {
                _isFloating = false;
                _animator.SetTrigger(AnimHash.Land);
                if (_isMoving)
                {
                    m_PlayerAnimController.LandAnim();
                    m_PlayerAnimController.StartRunAnim();
                }
            }
        }
        
        private void Jump()
        {
            UnRestrictMovement();
            m_PlayerAnimController.StopRunAnim();
            m_PlayerAnimController.JumpAnim();
            _isFloating = true;
            _velocity.y = Mathf.Sqrt(m_JumpHeight * -2f * m_Gravity);
        }
        private void Slash()
        {
            _isMoving = false;
            m_PlayerAnimController.StopRunAnim();
            m_PlayerAnimController.Slash1Anim();
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