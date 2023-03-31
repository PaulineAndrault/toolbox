using UnityEngine;

public enum PlayerState
{
    IDLE,
    RUNNING,
    ROLLING,
    SPRINTING,
}


public class PlayerStateMachine : MonoBehaviour
{
    private PlayerInput _playerInput;
    private PlayerController _playerController;
    private Animator _animator;

    private PlayerState _currentState;

    public PlayerState CurrentState { get => _currentState; set => _currentState = value; }

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerController = GetComponent<PlayerController>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        OnEnterIdle();
    }

    private void Update()
    {
        OnStateUpdate(_currentState);
    }
    private void OnStateEnter(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.IDLE:
                OnEnterIdle();
                break;
            case PlayerState.RUNNING:
                OnEnterRunning();
                break;
            case PlayerState.ROLLING:
                OnEnterRolling();
                break;
            case PlayerState.SPRINTING:
                OnEnterSprinting();
                break;
            default:
                Debug.LogError("OnStateEnter: Invalid state " + state.ToString());
                break;
        }
    }
    private void OnStateUpdate(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.IDLE:
                OnUpdateIdle();
                break;
            case PlayerState.RUNNING:
                OnUpdateRunning();
                break;
            case PlayerState.ROLLING:
                OnUpdateRolling();
                break;
            case PlayerState.SPRINTING:
                OnUpdateSprinting();
                break;
            default:
                Debug.LogError("OnStateUpdate: Invalid state " + state.ToString());
                break;
        }
    }
    private void OnStateExit(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.IDLE:
                OnExitIdle();
                break;
            case PlayerState.RUNNING:
                OnExitRunning();
                break;
            case PlayerState.ROLLING:
                OnExitRolling();
                break;
            case PlayerState.SPRINTING:
                OnExitSprinting();
                break;
            default:
                Debug.LogError("OnStateExit: Invalid state " + state.ToString());
                break;
        }
    }

    private void TransitionToState(PlayerState toState)
    {
        OnStateExit(_currentState);
        _currentState = toState;
        OnStateEnter(toState);
    }


    private void OnEnterIdle()
    {
        // On modifie le paramètre de l'Animator
        _animator.SetTrigger("Idle");
    }

    private void OnUpdateIdle()
    {
        // Si il y a un mouvement
        if (_playerInput.HasMovement)
        {
            // Si la touche Roll est enfoncée
            if (_playerInput.Roll)
            {
                // On passe en SPRINTING
                TransitionToState(PlayerState.SPRINTING);
            }
            // Sinon
            else
            {
                // On passe en RUNNING
                TransitionToState(PlayerState.RUNNING);
            }
        }
        // Sinon si on appuie la touche Roll
        else if (_playerInput.RollDown)
        {
            TransitionToState(PlayerState.ROLLING);
        }
    }

    private void OnExitIdle()
    {

    }


    private void OnEnterRunning()
    {
        // On modifie le paramètre de l'Animator
        _animator.SetTrigger("Run");
    }

    private void OnUpdateRunning()
    {
        // Si il n'y a pas de mouvement
        if (!_playerInput.HasMovement)
        {
            // On passe en IDLE
            TransitionToState(PlayerState.IDLE);
        }
        // Sinon si on appuie la touche Roll
        else if (_playerInput.RollDown)
        {
            // On passe en ROLLING
            TransitionToState(PlayerState.ROLLING);
        }
    }

    private void OnExitRunning()
    {

    }

    private void OnEnterRolling()
    {
        // On démarre la roulade
        _playerController.StartRoll();
        // On modifie le paramètre de l'Animator
        _animator.SetTrigger("Roll");
    }
    private void OnUpdateRolling()
    {
        // Si la roulade est terminée
        if (_playerController.IsRollEnded())
        {
            // Si il y a mouvement et que la touche ROLL est enfoncée
            if (_playerInput.HasMovement && _playerInput.Roll)
            {
                // On passe en SPRINTING
                TransitionToState(PlayerState.SPRINTING);
            }
            // Si il y a mouvement et que la touche ROLL n'est pas enfoncée
            else if (_playerInput.HasMovement && !_playerInput.Roll)
            {
                // On passe en RUNNING
                TransitionToState(PlayerState.RUNNING);
            }
            // Si il n'y a pas de mouvement
            else if (!_playerInput.HasMovement)
            {
                // On passe en IDLE
                TransitionToState(PlayerState.IDLE);
            }
        }
    }
    private void OnExitRolling()
    {

    }

    private void OnEnterSprinting()
    {
        // On démarre le sprint
        _playerController.StartSprinting();
        // On modifie le paramètre de l'Animator
        _animator.SetTrigger("Sprint");
    }
    private void OnUpdateSprinting()
    {
        // Si il n'y a pas de mouvement
        if (!_playerInput.HasMovement)
        {
            // On passe en IDLE
            TransitionToState(PlayerState.IDLE);
        }
        // Sinon (il y a mouvement) si la touche Roll n'est plus enfoncée
        else if (!_playerInput.Roll)
        {
            // On passe en RUNNING
            TransitionToState(PlayerState.RUNNING);
        }
        
    }
    private void OnExitSprinting()
    {
        // On arrête le sprint
        _playerController.StopSprinting();
    }

    private void OnGUI()
    {
        // On affiche l'état en cours         
        GUI.Label(new Rect(50,50,100,100), _currentState.ToString(), new GUIStyle() { fontSize = 50, fontStyle = FontStyle.Bold });
    }
}
