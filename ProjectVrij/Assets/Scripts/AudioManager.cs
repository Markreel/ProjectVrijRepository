﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public enum ClipType { Effect, Music, UI, Ambience }

    [Header("Sources: ")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambienceSource;
    [SerializeField] private AudioSource effectSource;

    [Header("Ambience: ")]
    public AudioClip DefaultAmbienceClip;

	[Header("Player: ")]
	public AudioClip footstepsClip;
	public AudioClip dashClip;
	public AudioClip swordHitClip;
	public AudioClip swordHitClip1;
	public AudioClip swordSwingClip;
	public AudioClip swordSwingClip1;

	[Header("Drone: ")]
	public AudioClip damageTaken;

	[Header("Music: ")]
    public AudioClip DefaultMusicClip;
    public AudioClip MainMenuMusicClip;
    public AudioClip BossMusicClip;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);
        Instance = this;

        musicSource.loop = true;
        musicSource.ignoreListenerPause = true;

        ambienceSource.loop = true;
        ambienceSource.clip = DefaultAmbienceClip;
        ambienceSource.Play();


        PlayMusic(DefaultMusicClip);
    }

    public void PlayClip(AudioClip _clip, ClipType _type = ClipType.Effect)
    {
        switch (_type)
        {
            default:
            case ClipType.Effect:
                effectSource.PlayOneShot(_clip);
                break;
            case ClipType.Music:
                PlayMusic(_clip);
                break;
        }
    }

    public void StopAmbience()
    {
        ambienceSource.Stop();
    }

    public void PlayRandomClip(AudioClip[] _clips, ClipType _type = ClipType.Effect)
    {
        if (_clips == null || _clips.Length == 0) return;
        AudioClip _rClip = _clips[Random.Range(0, _clips.Length)];

        switch (_type)
        {
            default:
            case ClipType.Effect:
                effectSource.PlayOneShot(_rClip);
                break;
            case ClipType.Music:
                PlayMusic(_rClip);
                break;
        }
    }
    public void PlayMusic(AudioClip _clip)
    {
        musicSource.clip = _clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}