using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomOneShotSound : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
    private AudioSource audioSource;

    [SerializeField] private float minPitch = 1;
    [SerializeField] private float maxPitch = 1;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        AudioClip clip = audioClips[(int)GetRandomInRange(0, audioClips.Count - 1)];

        audioSource.clip = clip;
        audioSource.pitch = Time.timeScale;//GetRandomInRange(minPitch, maxPitch);

        audioSource.Play();
    }

    private float GetRandomInRange(float min, float max)
    {
        float value = Random.Range(min, max);

        return value;
    }
}
