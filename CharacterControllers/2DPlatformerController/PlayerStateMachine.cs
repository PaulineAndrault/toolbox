using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    GROUNDED,
    JUMPING,
    FALLING,
}

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private PlayerState _currentState;
    [SerializeField] private PlayerController _controller;
    [SerializeField] private Animator _animator;

    private void Start()
    {
        // Initialisation du premier state
        OnStateEnter(PlayerState.GROUNDED);
    }

    #region Structure classique de la State Machine

    private void Update()
    {
        OnStateUpdate(_currentState);
        _animator.SetFloat("Velocity", Mathf.Abs(_controller.GetHorizontalVelocity()));
    }

    private void OnStateEnter(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.GROUNDED:
                OnEnterGrounded();
                break;

            case PlayerState.JUMPING:
                OnEnterJump();
                break;

            case PlayerState.FALLING:
                OnEnterFall();
                break;

            default:
                Debug.LogError($"OnStateEnter: Invalid state {state}");
                break;
        }
    }

    private void OnStateUpdate (PlayerState state)
    {
        switch (state)
        {
            case PlayerState.GROUNDED:
                OnUpdateGrounded();
                break;

            case PlayerState.JUMPING:
                OnUpdateJump();
                break;

            case PlayerState.FALLING:
                OnUpdateFall();
                break;

            default:
                Debug.LogError($"OnStateEnter: Invalid state {state}");
                break;
        }
    }

    private void OnStateExit(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.GROUNDED:
                OnExitGrounded();
                break;

            case PlayerState.JUMPING:
                OnExitJump();
                break;

            case PlayerState.FALLING:
                OnExitFall();
                break;

            default:
                Debug.LogError($"OnStateEnter: Invalid state {state}");
                break;
        }
    }

    private void TransitionToState(PlayerState toState)
    {
        OnStateExit(_currentState);
        _currentState = toState;
        OnStateEnter(toState);
    }

    #endregion  

    private void OnEnterGrounded()
    {
        _animator.SetBool("Idle", true);
        _controller.ResetJumpCount();
    }
    private void OnUpdateGrounded()
    {
        // Transition vers Jumping
        // if (Input.GetButtonDown("Jump") && _controller._jumpCount < _controller._jumpCountMax)
        if (Input.GetButtonDown("Jump") && _controller.CanJump())
        {
            TransitionToState(PlayerState.JUMPING);
            return;
        }

        // Transition vers Falling
        if (!_controller.IsGrounded())
        {
            TransitionToState(PlayerState.FALLING);
            return;
        }

    }    
    private void OnExitGrounded()
    {
        _animator.SetBool("Idle", false);
    }

    private void OnEnterJump()
    {
        _controller.Jump();
        _animator.SetFloat("GoingUp", 0f);
        _animator.SetBool("Jump", true);
    }

    private void OnUpdateJump()
    {
        _animator.SetFloat("GoingUp", _controller.GetVerticalVelocity());

        // Transition Fall
        if(_controller.GetVerticalVelocity() < 0)
        {
            TransitionToState(PlayerState.FALLING);
        }

        // Transition Jump
        if (Input.GetButtonDown("Jump") && _controller.CanJump())
        {
            _controller.Jump();
            return;
        }
    }
    private void OnExitJump()
    {
        _animator.SetBool("Jump", false);
    }

    private void OnEnterFall()
    {
        _animator.SetBool("Jump", true);
    }

    private void OnUpdateFall()
    {
        _animator.SetFloat("GoingUp", _controller.GetVerticalVelocity());
        
        // Transition Jump
        if (Input.GetButtonDown("Jump") && _controller.CanJump())
        {
            TransitionToState(PlayerState.JUMPING);
            return;
        }

        // Transition Grounded
        if (_controller.IsGrounded())
        {
            TransitionToState(PlayerState.GROUNDED);
            return;
        }
    }

    private void OnExitFall()
    {
        _animator.SetBool("Jump", false);
    }
}
