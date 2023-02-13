using UnityEngine;
using UnityEngine.Audio;

public class Door : MonoBehaviour, IUsable
{
    public enum DoorState
    {
        Closed,
        Opening,
        Opened,
        Closing
    }

    [SerializeField] float _animDuration;
    [SerializeField] Transform _bodyTransform;
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] float _closedDoorCutoff;
    [SerializeField] float _openedDoorCutoff;
    
    DoorState _state;
    float _animTime;

    private void Awake()
    {
        _state = DoorState.Closed;
        _audioMixer.SetFloat("LowpassCutoff", _closedDoorCutoff);
    }

    public void Use()
    {
        switch (_state)
        {
            case DoorState.Closed:
                _state = DoorState.Opening;
                _animTime = Time.time;
                break;
            case DoorState.Opened:
                _state = DoorState.Closing;
                _animTime = Time.time;
                break;
        }
    }

    private void Update()
    {
        if (_state == DoorState.Opening)
        {
            float fractionOfDistance = (Time.time - _animTime) / _animDuration;
            _bodyTransform.localPosition = Vector3.Lerp(Vector3.zero, Vector3.up, fractionOfDistance);
            _audioMixer.SetFloat("LowpassCutoff", Mathf.Lerp(_closedDoorCutoff, _openedDoorCutoff, fractionOfDistance));

            if(fractionOfDistance > 1)
                _state = DoorState.Opened;
        }

        if (_state == DoorState.Closing)
        {
            float fractionOfDistance = (Time.time - _animTime) / _animDuration;
            _bodyTransform.localPosition = Vector3.Lerp(Vector3.up, Vector3.zero, fractionOfDistance);
            _audioMixer.SetFloat("LowpassCutoff", Mathf.Lerp(_openedDoorCutoff, _closedDoorCutoff, fractionOfDistance));

            if(fractionOfDistance > 1)
                _state = DoorState.Closed;
        }

    }
}
