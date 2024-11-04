using KBCore.Refs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("References")]
    [SerializeField, Self] private AudioSource audioSource;
    [field: SerializeField] public AudioClip MineSFX { get; private set; }
    [field: SerializeField] public AudioClip ChaChingSFX { get; private set; }
    [field: SerializeField] public AudioClip SuccessSFX { get; private set; }
    [field: SerializeField] public AudioClip WrongSFX { get; private set; }

    private void OnValidate()
    {
        this.ValidateRefs();
    }

    private void Awake()
    {
        Instance = this;
    }

    public void Play(AudioClip clip, float volume)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.pitch = 1f;
        audioSource.Play();
    }

    public void Play(AudioClip clip, float volume, float pitch)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;    
        audioSource.pitch = pitch;
        audioSource.Play();
    }
}
