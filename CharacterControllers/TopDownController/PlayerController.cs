using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // On déclare nos variables serialisées
    [SerializeField] private float _regularSpeed = 5f;
    [SerializeField] private float _sprintSpeed = 10f;
    [SerializeField] private float _rollSpeed = 10f;
    [SerializeField] private float _rollDuration = 1f;
    [SerializeField] private AnimationCurve _rollEasing;

    // On déclare nos composants
    private PlayerInput _input;
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    // On déclare nos variables privées
    private bool _isRolling;            // Roulade en cours ou non
    private bool _isSprinting;          // Sprint en cours ou non
    private Vector2 _lastDirection;     // Dernière direction empruntée par le joueur
    private float _rollEndTime;         // Temps de fin de roulade
    private float _actualSpeed;         // Vitesse réelle

    private void Awake()
    {
        // Mise en cache des composants
        _input = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // On détermine la vitesse réelle sur la vitesse normale
        _actualSpeed = _regularSpeed;
    }

    private void Update()
    {
        // On modifie les paramètres de l'Animator
        SetAnimatorParams();
    }

    private void FixedUpdate()
    {
        // Si une roulade est en cours
        if (_isRolling)
        {
            // On continue le mouvement de roulade
            ContinueRoll();
        }
        // Sinon
        else
        {
            // On stocke la direction empruntée par le joueur
            SaveLastDirection();
            // On applique les mouvements de déplacement
            ApplyMovement();
        }
    }

    public void StartRoll()
    {
        // On définit l'état de la roulade
        _isRolling = true;
        // On définit la vitesse réelle sur la vitesse de roulade
        _actualSpeed = _rollSpeed;
        // On définit le temps de fin de roulade
        _rollEndTime = Time.time + _rollDuration;
    }

    public bool IsRollEnded()
    {
        // On retourne si la roulade est terminée ou non
        return !_isRolling;
    }

    public void StartSprinting()
    {
        // On définit la vitesse réelle sur la vitesse de sprint
        _actualSpeed = _sprintSpeed;
        // On définit l'état du sprint sur true
        _isSprinting = true;
    }
    public void StopSprinting()
    {
        // On définit la vitesse réelle sur la vitesse normale
        _actualSpeed = _regularSpeed;
        // On définit l'état du sprint sur true
        _isSprinting = false;
    }

    private void ContinueRoll()
    {
        // Si la roulade est terminée
        if (Time.time >= _rollEndTime)
        {
            // On définit l'état de la roulade
            _isRolling = false;
            // On définit la vitesse réelle sur la vitesse normale
            _actualSpeed = _regularSpeed;
        }
        else
        {
            // On calcul l'acceleration de la roulade selon sa progression
            float acceleration = _rollEasing.Evaluate(GetRollProgress());
            // On applique le mouvement
            _rigidbody.velocity = new Vector2(_lastDirection.x, _lastDirection.y) * _rollSpeed * acceleration;
        }
    }

    private void SaveLastDirection()
    {
        // Si il y a un mouvement
        if (_input.HasMovement)
        {
            // On stocke la direction de celui-ci
            _lastDirection = _input.NormalizedMovement;
        }
    }


    private void ApplyMovement()
    {
        // On détermine le vecteur de velocité (si on est en sprint on ne veut pas pouvoir "doser" la vitesse avec la course du joystick, on utilise donc le vecteur normalisé, sinon on Clamp pour éviter le deplacement diagonal trop rapide)
        Vector2 veloc = _isSprinting ? _input.NormalizedMovement : _input.ClampedMovement;

        // On applique la velocité
        _rigidbody.velocity = veloc * _actualSpeed;
    }

    private float GetRollProgress()
    {
        // On calcule la progression de la roulade en divisant la durée actuelle par la durée totale
        return (_rollDuration - (_rollEndTime - Time.time)) / _rollDuration;
    }

    private void SetAnimatorParams()
    {
        // On modifie les paramètres de l'Animator
        _animator.SetFloat("Horizontal", _lastDirection.x);
        _animator.SetFloat("Vertical", _lastDirection.y);
        _animator.SetFloat("Speed", _input.ClampedMovement.magnitude);
        _animator.SetFloat("RollProgress", _isRolling ? GetRollProgress() : 0f);
    }

}
