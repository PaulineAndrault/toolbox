using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float _maxDistance;
    [SerializeField] Image _crosshair;
    IUsable _target;
    Transform _camTransform;

    private void Awake()
    {
        _camTransform = Camera.main.transform;
    }

    private void Update()
    {
        FindTarget();

        if(Input.GetButtonDown("Use") && _target != null)
        {
            UseTarget();
        }

        ChangeCrossHairState();

    }

    void FindTarget()
    {
        RaycastHit hit;

        if (Physics.Raycast(_camTransform.position, _camTransform.forward, out hit, _maxDistance) && hit.collider.GetComponent<IUsable>() != null)
        {
            _target = hit.collider.GetComponent<IUsable>();
        }
        else
        {
            _target = null;
        }

    }

    void UseTarget()
    {
        _target.Use();
        _target = null;
    }

    void ChangeCrossHairState()
    {
        if (_target != null)
        {
            _crosshair.color = new Color(255, 0, 0, 0.15f);
        }
        else
        {
            _crosshair.color = new Color(255, 255, 255, 0.15f);
        }
    }

}
