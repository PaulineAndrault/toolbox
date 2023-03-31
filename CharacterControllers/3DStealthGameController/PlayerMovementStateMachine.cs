using UnityEngine;

namespace AdvancedController
{
    public class PlayerMovementStateMachine : MonoBehaviour
    {
        [SerializeField] private MovementState _currentState;
        //[SerializeField] private Animator _animator;
        private PlayerInput _playerInput;
        private PlayerController _playerController;
        private PlayerAirStateMachine _playerAirStateMachine;

        public MovementState CurrentState { get => _currentState; }

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _playerController = GetComponent<PlayerController>();
            _playerAirStateMachine = GetComponent<PlayerAirStateMachine>();
        }

        private void Start()
        {
            OnStateEnter(MovementState.IDLE);
        }
        private void Update()
        {
            OnStateUpdate(_currentState);
        }

        #region STATE MACHINE PATTERN
        private void OnStateEnter(MovementState state)
        {
            switch (state)
            {
                case MovementState.IDLE:
                    OnEnterIdle();
                    break;
                case MovementState.WALK:
                    OnEnterWalk();
                    break;
                case MovementState.RUN:
                    OnEnterRun();
                    break;
                case MovementState.CROUCH:
                    OnEnterCrouch();
                    break;

                default:
                    Debug.LogError("OnStateEnter: Invalid state " + state.ToString());
                    break;
            }
        }
        private void OnStateUpdate(MovementState state)
        {
            switch (state)
            {
                case MovementState.IDLE:
                    OnUpdateIdle();
                    break;
                case MovementState.WALK:
                    OnUpdateWalk();
                    break;
                case MovementState.RUN:
                    OnUpdateRun();
                    break;
                case MovementState.CROUCH:
                    OnUpdateCrouch();
                    break;

                default:
                    Debug.LogError("OnStateUpdate: Invalid state " + state.ToString());
                    break;
            }
        }
        private void OnStateExit(MovementState state)
        {
            switch (state)
            {
                case MovementState.IDLE:
                    OnExitIdle();
                    break;
                case MovementState.WALK:
                    OnExitWalk();
                    break;
                case MovementState.RUN:
                    OnExitRun();
                    break;
                case MovementState.CROUCH:
                    OnExitCrouch();
                    break;

                default:
                    Debug.LogError("OnStateExit: Invalid state " + state.ToString());
                    break;
            }
        }
        public void TransitionToState(MovementState toState)
        {
            OnStateExit(_currentState);
            _currentState = toState;
            OnStateEnter(toState);
        }

        #endregion

        #region IDLE
        private void OnEnterIdle()
        {
            // On arrête complètement le mouvement
            _playerController.ResetHorizontalVelocity();

            // _animator.SetBool("IsIdle", true);
        }
        private void OnUpdateIdle()
        {
            // Si on a la touche Crouch d'active (mouvement ou non) et si on est au sol
            if (_playerInput.CrouchButton.IsActive && _playerAirStateMachine.CurrentState == AirState.GROUNDED)
            {
                TransitionToState(MovementState.CROUCH);
            }
            // Si on se déplace
            else if (_playerInput.Movement.HasMovement)
            {
                // Si on a la touche Run d'active
                if (_playerInput.RunButton.IsActive)
                {
                    TransitionToState(MovementState.RUN);
                }
                // Sinon, on marche simplement
                else
                {
                    TransitionToState(MovementState.WALK);
                }
            }
        }
        private void OnExitIdle()
        {
            //_animator.SetBool("IsIdle", false);
        }
        #endregion

        #region WALK
        private void OnEnterWalk()
        {
            //_animator.SetBool("IsWalking", true);
        }
        private void OnUpdateWalk()
        {
            if (!_playerInput.Movement.HasMovement)
            {
                // On passe en état IDLE
                TransitionToState(MovementState.IDLE);
            }
            // Sinon si on appuie la touche Sprint
            else if (_playerInput.RunButton.IsActive)
            {
                // On passe en état RUN
                TransitionToState(MovementState.RUN);
            }
            else if (_playerInput.CrouchButton.IsActive && _playerAirStateMachine.CurrentState == AirState.GROUNDED)
            {
                // On passe en état CROUCH
                TransitionToState(MovementState.CROUCH);
            }
        }
        private void OnExitWalk()
        {
            //_animator.SetBool("IsWalking", false);
        }
        #endregion

        #region RUN
        private void OnEnterRun()
        {
            //_animator.SetBool("IsSprinting", true);
        }
        private void OnUpdateRun()
        {
            // Si on ne se déplace pas
            if (!_playerInput.Movement.HasMovement)
            {
                // On passe en état IDLE
                TransitionToState(MovementState.IDLE);
            }
            // Si on appuie sur le bouton Crouch pendant le Run, Crouch prend le dessus
            else if (_playerInput.CrouchButton.IsDown && _playerAirStateMachine.CurrentState == AirState.GROUNDED)
            {
                TransitionToState(MovementState.CROUCH);
            }
            // Sinon si on n'appuie pas la touche Run
            else if (!_playerInput.RunButton.IsActive)
            {
                // On passe en état WALKING
                TransitionToState(MovementState.WALK);
            }
        }
        private void OnExitRun()
        {
            //_animator.SetBool("IsSprinting", false);
        }
        #endregion

        #region CROUCH
        private void OnEnterCrouch()
        {
            // animator
        }
        private void OnUpdateCrouch()
        {
            // Si on n'est plus au sol
            if (_playerAirStateMachine.CurrentState != AirState.GROUNDED)
            {
                TransitionToState(MovementState.IDLE);
            }
            // Si on ne se déplace pas et qu'on n'appuie pas sur Crouch
            else if (!_playerInput.Movement.HasMovement)
            {
                // On pourrait rester en Crouch sur place, sauf si on relache le bouton Crouch
                if (!_playerInput.CrouchButton.IsActive)
                {
                    TransitionToState(MovementState.IDLE);
                }
            }
            // Si on presse le bouton RUN, il prend le dessus sur CROUCH
            else if (_playerInput.RunButton.IsDown)
            {
                TransitionToState(MovementState.RUN);
            }
            // Si on relache simplement Crouch en se déplaçant, on WALK
            else if (!_playerInput.CrouchButton.IsActive)
            {
                TransitionToState(MovementState.WALK);
            }
        }
        private void OnExitCrouch()
        {
        }
        #endregion

    }
}