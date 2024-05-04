using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientNoises : MonoBehaviour
{
    public AudioClip[] sounds;
    public AudioSource audioSource;
    public AudioSource audioSourceConstant;

    private float minInterval = 10f;
    private float maxInterval = 30f;
    private float nextPlayTime;
    private float minPitch = 0.4f;
    private float maxPitch = 1f;

    void Start()
    {
        audioSourceConstant.Play();
        nextPlayTime = Time.time + Random.Range(minInterval, maxInterval);
    }

    void Update()
    {
        if (Time.time >= nextPlayTime)
        {
            PlayRandomSound();
            nextPlayTime = Time.time + Random.Range(minInterval, maxInterval);
        }
    }

    void PlayRandomSound()
    {
        if (sounds.Length == 0) return;
        int randomIndex = Random.Range(0, sounds.Length);
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(sounds[randomIndex]);
    }
}

