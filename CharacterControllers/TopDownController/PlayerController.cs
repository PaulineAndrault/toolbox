using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // On d�clare nos variables serialis�es
    [SerializeField] private float _regularSpeed = 5f;
    [SerializeField] private float _sprintSpeed = 10f;
    [SerializeField] private float _rollSpeed = 10f;
    [SerializeField] private float _rollDuration = 1f;
    [SerializeField] private AnimationCurve _rollEasing;

    // On d�clare nos composants
    private PlayerInput _input;
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    // On d�clare nos variables priv�es
    private bool _isRolling;            // Roulade en cours ou non
    private bool _isSprinting;          // Sprint en cours ou non
    private Vector2 _lastDirection;     // Derni�re direction emprunt�e par le joueur
    private float _rollEndTime;         // Temps de fin de roulade
    private float _actualSpeed;         // Vitesse r�elle

    private void Awake()
    {
        // Mise en cache des composants
        _input = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // On d�termine la vitesse r�elle sur la vitesse normale
        _actualSpeed = _regularSpeed;
    }

    private void Update()
    {
        // On modifie les param�tres de l'Animator
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
            // On stocke la direction emprunt�e par le joueur
            SaveLastDirection();
            // On applique les mouvements de d�placement
            ApplyMovement();
        }
    }

    public void StartRoll()
    {
        // On d�finit l'�tat de la roulade
        _isRolling = true;
        // On d�finit la vitesse r�elle sur la vitesse de roulade
        _actualSpeed = _rollSpeed;
        // On d�finit le temps de fin de roulade
        _rollEndTime = Time.time + _rollDuration;
    }

    public bool IsRollEnded()
    {
        // On retourne si la roulade est termin�e ou non
        return !_isRolling;
    }

    public void StartSprinting()
    {
        // On d�finit la vitesse r�elle sur la vitesse de sprint
        _actualSpeed = _sprintSpeed;
        // On d�finit l'�tat du sprint sur true
        _isSprinting = true;
    }
    public void StopSprinting()
    {
        // On d�finit la vitesse r�elle sur la vitesse normale
        _actualSpeed = _regularSpeed;
        // On d�finit l'�tat du sprint sur true
        _isSprinting = false;
    }

    private void ContinueRoll()
    {
        // Si la roulade est termin�e
        if (Time.time >= _rollEndTime)
        {
            // On d�finit l'�tat de la roulade
            _isRolling = false;
            // On d�finit la vitesse r�elle sur la vitesse normale
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
        // On d�termine le vecteur de velocit� (si on est en sprint on ne veut pas pouvoir "doser" la vitesse avec la course du joystick, on utilise donc le vecteur normalis�, sinon on Clamp pour �viter le deplacement diagonal trop rapide)
        Vector2 veloc = _isSprinting ? _input.NormalizedMovement : _input.ClampedMovement;

        // On applique la velocit�
        _rigidbody.velocity = veloc * _actualSpeed;
    }

    private float GetRollProgress()
    {
        // On calcule la progression de la roulade en divisant la dur�e actuelle par la dur�e totale
        return (_rollDuration - (_rollEndTime - Time.time)) / _rollDuration;
    }

    private void SetAnimatorParams()
    {
        // On modifie les param�tres de l'Animator
        _animator.SetFloat("Horizontal", _lastDirection.x);
        _animator.SetFloat("Vertical", _lastDirection.y);
        _animator.SetFloat("Speed", _input.ClampedMovement.magnitude);
        _animator.SetFloat("RollProgress", _isRolling ? GetRollProgress() : 0f);
    }

}
