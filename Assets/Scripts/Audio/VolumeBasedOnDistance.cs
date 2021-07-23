using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeBasedOnDistance : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private bool distanceAttenuation = true;
    [SerializeField] private bool timeBasedPitch = true;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (distanceAttenuation)
        {
            audioSource.volume = 0;
        }
    }

    void Update()
    {
        if (distanceAttenuation)
        {
            var distance = Vector3.Distance(transform.position, Vector3.zero);

            var volume = 1 - (distance * 0.05f);

            volume = Mathf.Clamp(volume, 0, 1);

            audioSource.volume = volume;
        }

        if (timeBasedPitch)
        {
            audioSource.pitch = Time.timeScale;
        }
    }
}
