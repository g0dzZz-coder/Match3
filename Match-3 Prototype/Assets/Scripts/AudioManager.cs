using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static float Volume = 1f;

    public static void Play(AudioSource source, AudioClip clip, bool loop = false, bool flag = false)
    {
        if (source == null || clip == null)
            return;

        source.volume = Volume;
        source.clip = clip;
        source.loop = loop;

        if (flag)
        {
            if (source.isPlaying)
                return;
        }

        source.Stop();

        source.Play();
    }

    public static void Stop(AudioSource source)
    {
        if (source == null)
            return;

        source.Stop();
    }

    public static void UpdateVolume(float volume)
    {
        Volume = volume;

        PlayerPrefs.SetFloat("volume", Volume);
        PlayerPrefs.Save();
    }

    private void Awake()
    {
        CheckAudio();
    }

    private void CheckAudio()
    {
        if (PlayerPrefs.HasKey("volume"))
        {
            Volume = PlayerPrefs.GetFloat("volume");
        }
        else
        {
            Volume = 1f;
            PlayerPrefs.SetFloat("volume", Volume);
        }

        PlayerPrefs.Save();
    }
}
