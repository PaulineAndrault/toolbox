using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform _cam;
    // [SerializeField] private float _camSpeed = 1f;
    [SerializeField] Rigidbody2D _rbPlayer;

    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] private float _camOffset = 3f;

    [SerializeField] Vector2 _max;
    [SerializeField] Vector2 _min;

    private void FixedUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        Vector3 targetPos = new Vector3(_rbPlayer.position.x, _rbPlayer.position.y + _camOffset, _cam.position.z);

        targetPos.x = Mathf.Clamp(targetPos.x, _min.x, _max.x);
        targetPos.y = Mathf.Clamp(targetPos.y, _min.y, _max.y);

        //// Vector3 targetPos = new Vector3(_rbPlayer.position.x + camOffset * (_transformPlayer.localEulerAngles.y == 0 ? 1 : -1), _rbPlayer.position.y + camOffset, _cam.position.z);
        // _cam.position = Vector3.Lerp(_cam.position, targetPos, _camSpeed * Time.deltaTime);


        // Smoothly move the camera towards that target position
        _cam.position = Vector3.SmoothDamp(_cam.position, targetPos, ref velocity, smoothTime);

    }
}
