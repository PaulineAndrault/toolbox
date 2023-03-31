using UnityEngine;

namespace AdvancedController
{
    public class PlayerAirStateMachine : MonoBehaviour
    {
        [SerializeField] private AirState _currentState;
        // [SerializeField] private Animator _animator;
        private PlayerInput _playerInput;
        private PlayerController _playerController;
        private GroundChecker _groundChecker;

        private Rigidbody _rb;

        private Vector3 _initialGravity;

        public AirState CurrentState { get => _currentState; }

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _playerController = GetComponent<PlayerController>();
            _rb = GetComponent<Rigidbody>();
            _groundChecker = GetComponent<GroundChecker>();
        }

        private void Start()
        {
            OnStateEnter(AirState.GROUNDED);
        }
        private void Update()
        {
            OnStateUpdate(_currentState);
        }

        #region STATE MACHINE PATTERN
        private void OnStateEnter(AirState state)
        {
            switch (state)
            {
                case AirState.GROUNDED:
                    OnEnterGrounded();
                    break;

                case AirState.JUMP:
                    OnEnterJump();
                    break;

                case AirState.FALL:
                    OnEnterFall();
                    break;

                default:
                    Debug.LogError("OnStateEnter: Invalid state " + state.ToString());
                    break;
            }
        }
        private void OnStateUpdate(AirState state)
        {
            switch (state)
            {
                case AirState.GROUNDED:
                    OnUpdateGrounded();
                    break;
                case AirState.JUMP:
                    OnUpdateJump();
                    break;
                case AirState.FALL:
                    OnUpdateFall();
                    break;
                default:
                    Debug.LogError("OnStateUpdate: Invalid state " + state.ToString());
                    break;
            }
        }
        private void OnStateExit(AirState state)
        {
            switch (state)
            {
                case AirState.GROUNDED:
                    OnExitGrounded();
                    break;
                case AirState.JUMP:
                    OnExitJump();
                    break;
                case AirState.FALL:
                    OnExitFall();
                    break;
                default:
                    Debug.LogError("OnStateExit: Invalid state " + state.ToString());
                    break;
            }
        }
        public void TransitionToState(AirState toState)
        {
            OnStateExit(_currentState);
            _currentState = toState;
            OnStateEnter(toState);
        }

        #endregion

        #region GROUNDED
        private void OnEnterGrounded()
        {
            //_animator.SetBool("IsGrounded", true);

            // On bloque la gravité pour gérer manuellement la hauteur
            _rb.useGravity = false;

            // On reset la velocity verticale
            _playerController.ResetVerticalVelocity();

            // On remet le joueur les pieds sur le sol
            _playerController.ResetHeight();

        }
        private void OnUpdateGrounded()
        {
            // Si on appuie la touche de Saut
            if (_playerInput.JumpButton.IsDown)
            {
                // On passe en état Jump
                TransitionToState(AirState.JUMP);
            }
            // Si on ne touche plus le sol, on tombe
            else if (!_playerController.IsGrounded)
            {
                TransitionToState(AirState.FALL);
            }
        }
        private void OnExitGrounded()
        {
            //_animator.SetBool("IsGrounded", false);
            _rb.useGravity = true;
        }
        #endregion

        #region JUMP
        private void OnEnterJump()
        {
            _playerController.Jump();
            //_animator.SetBool("IsJumping", true);
        }
        private void OnUpdateJump()
        {
            if (_rb.velocity.y <= 0.01f)
            {
                TransitionToState(AirState.FALL);
            }

            //// Cas du saut parfait
            //else if(_playerController.IsGrounded)
            //{
            //    TransitionToState(AirState.GROUNDED);
            //}

        }
        private void OnExitJump()
        {
            //_animator.SetBool("IsJumping", false);
        }
        #endregion

        #region FALL
        private void OnEnterFall()
        {
            Physics.gravity = Physics.gravity * 2;
            //animator
        }
        private void OnUpdateFall()
        {
            if (_playerController.IsGrounded)
            {
                TransitionToState(AirState.GROUNDED);
            }
        }
        private void OnExitFall()
        {
            Physics.gravity = Physics.gravity / 2;
        }
        #endregion

    }
}