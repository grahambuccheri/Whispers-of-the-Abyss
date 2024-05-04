using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource[] audioSources;

    public void PlayRandomAudio(AudioSource target)
    {
        if (audioClips.Length == 0)
        {
            Debug.LogWarning("Audio clips array is empty!");
            return;
        }

        if (audioSources.Length == 0)
        {
            Debug.LogWarning("Audio sources array is empty!");
            return;
        }

        // Pick a random audio clip
        int randomClipIndex = Random.Range(0, audioClips.Length);
        AudioClip randomClip = audioClips[randomClipIndex];

        // Pick a random audio source
        

        // Play the random audio clip on the random audio source
        target.clip = randomClip;
        target.Play();
    }
}
