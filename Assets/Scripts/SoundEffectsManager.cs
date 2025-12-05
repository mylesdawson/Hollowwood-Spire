using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectsManager: MonoBehaviour
{
    public static SoundEffectsManager Instance;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void PlayEffect(string name)
    {
        var clip = Resources.Load<AudioClip>($"SoundEffects/{name}");
        if(clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }   
}