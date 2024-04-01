using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private bool isMainMenuMusic = false;
    [SerializeField] private bool loopMusic = false;
    [SerializeField] private float volume;
    private AudioSource musicSource;
    

    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        if (isMainMenuMusic) startMusic();
    }

    public void startMusic()
    {
        musicSource.clip = musicClip;
        musicSource.loop = loopMusic;

        StartCoroutine(StartVolumeCoroutine());
    }

    public void stopMusic()
    {
        StartCoroutine(StopVolumeCoroutine());
    }

    private IEnumerator StartVolumeCoroutine()
    {
        musicSource.volume = 0f;
        musicSource.Play();

        while (musicSource.volume < volume)
        {
            musicSource.volume += 0.0003f;
            yield return null;
        }
    }

    private IEnumerator StopVolumeCoroutine()
    {
        while ( 0 < musicSource.volume)
        {
            musicSource.volume -= 0.00003f;
            yield return null;
        }

        musicSource.Stop();
    }
}
