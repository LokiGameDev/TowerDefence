using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if(_instance==null)
            {
                Debug.LogError("Audio Manager is null");
            }
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }

    [Header("Audio Sources")]
    public AudioSource sfxAudioSource;
    public AudioSource musicAudioSource;
    [Header("Audio Clips")]
    public AudioClip[] sfxAudioClips;
    public AudioClip[] bgmAudioClips;
    
    public void Start()
    {
        if(musicAudioSource != null)
        {
            musicAudioSource.clip = bgmAudioClips[0];
            musicAudioSource.Play();
        }
    }

    public void PlayTheAudioClip(AudioType audioType)
    {
        if(sfxAudioSource!=null)
        {
            switch(audioType)
            {
                case AudioType.MouseClick:
                    sfxAudioSource.clip = sfxAudioClips[0];
                    sfxAudioSource.Play();
                    Debug.Log("Played");
                    break;
                case AudioType.WaveCleared:
                    sfxAudioSource.clip = sfxAudioClips[1];
                    sfxAudioSource.Play();
                    break;
                case AudioType.EnemyDestroyed:
                    sfxAudioSource.clip = sfxAudioClips[2];
                    sfxAudioSource.Play();
                    break;
                case AudioType.TurretDestroyed:
                    sfxAudioSource.clip = sfxAudioClips[3];
                    sfxAudioSource.Play();
                    break;
            }
        }
    }
}

public enum AudioType
{
    MouseClick,
    EnemyDestroyed,
    TurretDestroyed,
    WaveCleared
}