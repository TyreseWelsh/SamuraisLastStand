using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public static bool isPaused = false;

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
