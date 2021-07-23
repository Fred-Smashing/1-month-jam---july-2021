using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Tween;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip menuMusicClip;
    [SerializeField] private AudioClip gameMusicClip;

    [SerializeField] private AudioMixerGroup targetMixer;

    [SerializeField] private float targetVolume = 0.8f;
    [SerializeField] private float crossfadeTime;

    [SerializeField] private bool pitchBasedOnTime = true;

    private AudioSource menuMusicSource;
    private AudioSource gameMusicSource;

    private void Awake()
    {
        menuMusicSource = this.gameObject.AddComponent<AudioSource>();
        SetupAudioSource(menuMusicSource, menuMusicClip);

        gameMusicSource = this.gameObject.AddComponent<AudioSource>();
        SetupAudioSource(gameMusicSource, gameMusicClip);

        //menuMusicSource.Pause();
        //gameMusicSource.Pause();

        CrossfadeToTrack("menu");
    }

    private void SetupAudioSource(AudioSource source, AudioClip clip)
    {
        source.bypassEffects = true;
        source.bypassReverbZones = true;
        source.loop = true;

        source.volume = 0.3f;

        source.clip = clip;
        source.outputAudioMixerGroup = targetMixer;

        //source.Play();
    }

    private void Update()
    {
        if (pitchBasedOnTime)
        {
            var pitch = Time.timeScale;

            pitch = Mathf.Clamp(pitch, -3, 3);

            menuMusicSource.pitch = pitch;
            gameMusicSource.pitch = pitch;
        }
    }

    public void CrossfadeToTrack(string track = "none")
    {
        if (track == "menu")
        {
            menuMusicSource.Play();

            TweenVolume("Menu Volume Tween", menuMusicSource, menuMusicSource.volume, targetVolume, crossfadeTime);
            TweenVolume("Game Volume Tween", gameMusicSource, gameMusicSource.volume, 0, crossfadeTime);
        }

        if (track == "game")
        {
            gameMusicSource.Play();

            TweenVolume("Menu Volume Tween", menuMusicSource, menuMusicSource.volume, 0, crossfadeTime);
            TweenVolume("Game Volume Tween", gameMusicSource, gameMusicSource.volume, targetVolume, crossfadeTime);
        }

        if (track == "none")
        {
            TweenVolume("Menu Volume Tween", menuMusicSource, menuMusicSource.volume, 0, crossfadeTime);
            TweenVolume("Game Volume Tween", gameMusicSource, gameMusicSource.volume, 0, crossfadeTime);
        }
    }

    private void TweenVolume(string tweenName, AudioSource source, float from, float to, float time)
    {
        System.Action<ITween<float>> updateVolume = (t) =>
        {
            source.volume = t.CurrentValue;
        };

        System.Action<ITween<float>> updateVolumeComplete = (t) =>
        {
            if (to == 0)
            {
                source.Stop();
            }
        };

        var tween = source.gameObject.Tween(tweenName, from, to, time, TweenScaleFunctions.CubicEaseOut, updateVolume, updateVolumeComplete);

        tween.ForceUpdate = true;
    }
}
