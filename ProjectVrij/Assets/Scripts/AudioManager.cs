using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public AudioClip footstepsPlayer;

	[Header("Music: ")]
    public AudioClip DefaultMusicClip;
    public AudioClip MainMenuMusicClip;
    public AudioClip BossMusicClip;

    private void Awake()
    {
        Instance = this;

        musicSource.loop = true;
        musicSource.ignoreListenerPause = true;
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
