using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AxisInput
{
    // Variables priv�es
    private Vector3 _value;

    // Constructeur
    public AxisInput (Vector3 vector)
    {
        // On clampe la magnitude � 1 max, pas besoin de normaliser ensuite.
        _value = Vector3.ClampMagnitude(vector, 1f);
    }

    // Propri�t�s
    public Vector3 Value { get => _value; }
    public bool HasMovement { get => _value != Vector3.zero; }
}


public struct ButtonInput
{
    // Variables priv�es
    private bool _isActive;
    private bool _isDown;
    private bool _isUp;

    // Constructeur
    public ButtonInput(bool isActive, bool isDown, bool isUp)
    {
        _isActive = isActive;
        _isDown = isDown;
        _isUp = isUp;
    }

    //Propri�t�s
    public bool IsActive { get => _isActive; }
    public bool IsDown { get => _isDown; }
    public bool IsUp { get => _isUp; }
}

public class PlayerInput : MonoBehaviour
{
    // R�f�rence au Player pour cr�er des mouvements par rapport � son ofrward et non pas par rapport au forward du monde
    private Transform _transform;

    // R�f�rence � l'animator pour envoyer les infos de d�placements X / Z
    private Animator _animator;

    // Axes de d�placement et de rotation
    private AxisInput _movement;

    // Boutons de d�placement
    private ButtonInput _jumpButton;
    private ButtonInput _runButton;
    private ButtonInput _crouchButton;

    // Bouton de photographie
    private ButtonInput _aimButton;
    private ButtonInput _shootButton;

    // Propri�t�s publiques : on n'a besoin que des setters, car on va "get" les valeurs en Update
    public AxisInput Movement { get => _movement; }
    public ButtonInput JumpButton { get => _jumpButton; }
    public ButtonInput RunButton { get => _runButton; }
    public ButtonInput CrouchButton { get => _crouchButton; }
    public ButtonInput AimButton { get => _aimButton; }
    public ButtonInput ShootButton { get => _shootButton; }

    private void Awake()
    {
        // On r�cup�re les r�f�rences
        _transform = transform;
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        // On applique les Inputs de d�placements au tranform.forward du Player pour le d�placer dans son local space

        Vector3 vectorHorizontal = _transform.right * Input.GetAxisRaw("Horizontal");
        Vector3 vectorVertical = _transform.forward * Input.GetAxisRaw("Vertical");
        
        _movement = new AxisInput(vectorHorizontal + vectorVertical);

        // On stocke la valeur de l'input Jump
        _jumpButton = new ButtonInput(Input.GetButton("Jump"), Input.GetButtonDown("Jump"), Input.GetButtonUp("Jump"));

        // On stocke la valeur de l'input Jump
        _runButton = new ButtonInput(Input.GetButton("Run"), Input.GetButtonDown("Run"), Input.GetButtonUp("Run"));

        // On stocke la valeur de l'input Jump
        _crouchButton = new ButtonInput(Input.GetButton("Crouch"), Input.GetButtonDown("Crouch"), Input.GetButtonUp("Crouch"));

        // On stocke la valeur de l'input Aim
        _aimButton = new ButtonInput(Input.GetButton("Aim"), Input.GetButtonDown("Aim"), Input.GetButtonUp("Aim"));
        
        // On stocke la valeur de l'input Shoot
        _shootButton = new ButtonInput(Input.GetButton("Shoot"), Input.GetButtonDown("Shoot"), Input.GetButtonUp("Shoot"));

        // On envoie les infos X / Z � l'animator
        _animator.SetFloat("XAxis", Input.GetAxisRaw("Horizontal"));
        _animator.SetFloat("ZAxis", Input.GetAxisRaw("Vertical"));
    }
}
