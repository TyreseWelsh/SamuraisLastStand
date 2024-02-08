using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject titleScreen;
    [SerializeField] GameObject creditsScreen;

    [SerializeField] AudioSource audioSrc;
    [SerializeField] AudioClip buttonHover, buttonClick;

    public void Play()
    {
        StartCoroutine(WaitToLoadGame());
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
        audioSrc.PlayOneShot(buttonHover);
    }

    public void PlayOnButtonClick()
    {
        audioSrc.PlayOneShot(buttonClick);
    }
}
