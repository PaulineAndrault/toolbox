using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTalker : MonoBehaviour, IUsable
{
    [SerializeField] AudioClip[] _audioClips;
    AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponentInChildren<AudioSource>();
    }

    public void Use() 
    {
        _audioSource.PlayOneShot(_audioClips[Random.Range(0, _audioClips.Length)]);
    }
}
