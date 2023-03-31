using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _normalSpeed;
    [SerializeField] float _sprintSpeed;

    // Refs
    Transform _transform;
    CharacterController _charControl;
    AudioSource _audioSource;

    private void Awake()
    {
        _transform = transform;
        _charControl = GetComponent<CharacterController>();
        _audioSource = GetComponentInChildren<AudioSource>();
    }

    private void Update()
    {
        float speed = Input.GetButton("Sprint") ? _sprintSpeed : _normalSpeed;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 xMove = horizontal * _transform.right;
        Vector3 zMove = vertical * _transform.forward;
        Vector3 movement = xMove + zMove;
        movement.Normalize();

        // Move the Player
        _charControl.Move(movement * speed * Time.deltaTime);

        // Change step sound effect depending on the velocity of the Player
        if (_charControl.velocity.magnitude > 0.1f)
            _audioSource.pitch = _charControl.velocity.magnitude / _normalSpeed;
        else
            _audioSource.pitch = 0;
    }
}
