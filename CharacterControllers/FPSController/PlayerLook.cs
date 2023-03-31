using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] float _mouseSensitivity;
    [SerializeField] float _padSensitivity;
    [SerializeField] float _mouseYMin;
    [SerializeField] float _mouseYMax;

    Transform _transform;
    Transform _camTransform;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _transform = transform;
        _camTransform = Camera.main.transform;
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * _mouseSensitivity * Time.deltaTime;
        float gamePadX = Input.GetAxisRaw("GamePad X") * _padSensitivity * Time.deltaTime;
        float gamePadY = Input.GetAxisRaw("GamePad Y") * _padSensitivity * Time.deltaTime;

        float _horizontal = mouseX + gamePadX;
        float _vertical = mouseY + gamePadY;

        // Horizontal rotation, no need to clamp
        _transform.Rotate(new Vector3(0, _horizontal, 0));

        // Vertical rotation, need to clamp between y min / y max
        if (_camTransform.localEulerAngles.x <= _mouseYMax + 0.1f || _camTransform.localEulerAngles.x >= _mouseYMin - 0.1f)
        {
            _camTransform.Rotate(new Vector3(_vertical, 0f, 0f));

            // If the rotation makes eulerAngle x > y max or < y min, we need to adjust the euler angle
            if (_camTransform.localEulerAngles.x > _mouseYMax && _camTransform.localEulerAngles.x < 180f)
            {
                _camTransform.localEulerAngles = new Vector3 (_mouseYMax, 0f, 0f);
            }
            else if (_camTransform.localEulerAngles.x < _mouseYMin && _camTransform.localEulerAngles.x > 180f)
            {
                _camTransform.localEulerAngles = new Vector3 (_mouseYMin, 0f, 0f);
            }
        }
    }

}
