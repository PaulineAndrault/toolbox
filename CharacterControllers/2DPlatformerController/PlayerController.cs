using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Transform _transform;
    [SerializeField] private Animator _animator;

    // Déplacement
    [SerializeField] private float _speed;
    private float _movementInput;

    // Saut
    [SerializeField] private int _jumpCountMax;
    private int _jumpCount = 0;
    [SerializeField] private float _jumpForce = 7f;
    [SerializeField] private GroundChecker _groundChecker;

    // Gravity modifiers
    [SerializeField] float _fallMultiplier = 2.5f;
    [SerializeField] float _lowJumpMultiplier = 2f;



    private void Update()
    {
        GetHorizontalInput();
        Flip();
        // Jump();
        // CheckGround();
        Attack();
    }

    private void FixedUpdate()
    {
        BetterJump();
        HorizontalMove();
    }

    private void GetHorizontalInput()
    {
        _movementInput = Input.GetAxisRaw("Horizontal");
    }

    private void Flip()
    {
        if(_movementInput < 0)
        {
            _transform.right = Vector2.left;
        }
        else if (_movementInput > 0)
        {
            _transform.right = Vector2.right;
        }
    }

    private void HorizontalMove()
    {
        _rigidbody.velocity = new Vector2(_movementInput * _speed, _rigidbody.velocity.y);
    }

    public float GetHorizontalVelocity()
    {
        return _rigidbody.velocity.x;
    }
    public float GetVerticalVelocity()
    {
        return _rigidbody.velocity.y;
    }

    public bool CanJump()
    {
        return _jumpCount < _jumpCountMax;
    }

    public void Jump()
    {
        _rigidbody.velocity = Vector2.up * _jumpForce;
        _jumpCount++;

        //if (Input.GetButtonDown("Jump") && _jumpCount < _jumpCountMax)
        //{
        //    _rigidbody.velocity = Vector2.up * _jumpForce;
        //    _jumpCount++;
        //}
    }

    private void BetterJump()
    {
        if(_rigidbody.velocity.y < -0.1f)
        {
            _rigidbody.gravityScale = _fallMultiplier;
        }
        //else if (_rigidbody.velocity.y > 0.1f && Mathf.Abs(_rigidbody.velocity.x) >= 1f)
        //{
        //    _rigidbody.gravityScale = _lowJumpMultiplier;
        //}
        else if(_rigidbody.velocity.y > 0.1f && !Input.GetButton("Jump"))
        {
            _rigidbody.gravityScale = _lowJumpMultiplier;
        }
        else
        {
            _rigidbody.gravityScale = 1f;
        }
    }

    //public void CheckGround()
    //{
    //    if (_rigidbody.velocity.y < 0f && _groundChecker.IsGrounded())
    //    {
    //        _jumpCount = 0;
    //    }
    //}

    public bool IsGrounded()
    {
        return _groundChecker.IsGrounded();
    }

    public void ResetJumpCount()
    {
        _jumpCount = 0;
    }

    public void Attack()
    {
        if (Input.GetButtonDown("Attack"))
        {
            _animator.SetTrigger("Attack");
            
        }
    }
}
