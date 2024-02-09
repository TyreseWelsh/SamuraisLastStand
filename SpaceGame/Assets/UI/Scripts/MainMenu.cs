using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject titleScreen;
    [SerializeField] GameObject creditsScreen;

    [SerializeField] AudioSource bgndAudioSrc, buttonAudioSrc;
    [SerializeField] AudioClip buttonHover, buttonClick;

    bool fadeOutBgndMusic = false;

    private void Update()
    {
        if (fadeOutBgndMusic)
        {
            FadeOutBgndMusic();
        }
    }

    private void FadeOutBgndMusic()
    {
        if(bgndAudioSrc.volume > 0)
        {
            bgndAudioSrc.volume -= Time.deltaTime / 1.5f;
        }
    }

    public void Play()
    {
        StartCoroutine(WaitToLoadGame());
        fadeOutBgndMusic = true;
    }

    IEnumerator WaitToLoadGame()
    {
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene("GameScene");
    }

    public void Credits()
    {
        titleScreen.SetActive(false);
        creditsScreen.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Back()
    {
        titleScreen.SetActive(true);
        creditsScreen.SetActive(false);
    }

    public void PlayOnButtonHover()
    {
        buttonAudioSrc.PlayOneShot(buttonHover);
    }

    public void PlayOnButtonClick()
    {
        buttonAudioSrc.PlayOneShot(buttonClick);
    }
}
