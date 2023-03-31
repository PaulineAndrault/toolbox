using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    //[SerializeField] Rigidbody2D _rb;
    [SerializeField] Transform _tr;
    //[SerializeField] Transform _target;
    
    [SerializeField] Transform _waypointA;
    [SerializeField] Transform _waypointB;

    [SerializeField] float _speed;

    private Vector3 _startPos;
    private Vector3 _targetPos;
    private float _fraction = 0f;
    // private bool _goBack = false;

    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {

        _targetPos = _waypointB.position;
        _startPos = _waypointA.position;
        _tr.position = _waypointA.position;
        //_startPos = _tr.position;
        //_targetPos = _target.position;
    }

    void FixedUpdate()
    {
        Moving();
        ChangingDirection();
    }

    private void Moving()
    {
        _tr.position = Vector3.Lerp(_startPos, _targetPos, _fraction);

        _fraction += _speed * Time.deltaTime;

        //_fraction += !_goBack ? _speed * Time.deltaTime : (-1 * _speed * Time.deltaTime);
    }

    private void ChangingDirection()
    {
        if(_fraction > 0.99f)
        {
            _targetPos = _targetPos == _waypointA.position ? _waypointB.position : _waypointA.position;
            _startPos = _startPos == _waypointA.position ? _waypointB.position : _waypointA.position;
            _fraction = 0;
        }
        
        //if(_fraction > 0.99f && !_goBack)
        //{
        //    _goBack = true;
        //}
        //else if(_fraction < 0.01f && _goBack)
        //{
        //    _goBack = false;
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == _player)
        {
            _player.transform.parent = transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == _player)
        {
            _player.transform.parent = null;
        }
    }
}
