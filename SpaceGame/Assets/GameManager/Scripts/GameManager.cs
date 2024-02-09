using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] AudioSource bgndMusicSrc;
    const float MAX_BGND_MUSIC_VOLUME = 0.04f;
    bool fadeInMusic = false;
    bool fadeOutMusic = false;

    [SerializeField] GameObject pauseMenu;

    public static bool isPaused = false;

    private void Awake()
    {
        fadeInMusic = true;
    }

    private void Update()
    {
        if (fadeInMusic)
        {
            if (bgndMusicSrc.volume <= MAX_BGND_MUSIC_VOLUME)
            {
                bgndMusicSrc.volume += Time.deltaTime / 4;
            }
            else
            {
                fadeInMusic = false;
            }
        }

        if(fadeOutMusic)
        {
            if(bgndMusicSrc.volume > 0)
            {
                bgndMusicSrc.volume -= Time.deltaTime / 4;
            }
            else
            {
                fadeOutMusic = false;
            }
        }
    }
    public void SetFadeInMusic(bool _fadeIn)
    {
        fadeInMusic= _fadeIn;
    }

    public void SetFadeOutMusic(bool _fadeOut)
    {
        fadeOutMusic = _fadeOut;
    }

    public void TogglePause()
    {
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        if (isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;
            pauseMenu.SetActive(false);

            foreach(AudioSource source in audioSources)
            {
                source.Play();
            }
        }
        else
        {
            Time.timeScale = 0;
            isPaused = true;
            pauseMenu.SetActive(true);

            foreach (AudioSource source in audioSources)
            {
                source.Pause();
            }
        }
    }
}
