using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FailureMenu : MonoBehaviour
{
    GameManager gameManager;
    ScoringSystem scoringSystem;
    int finalScore = 0;
    [SerializeField] TextMeshProUGUI finalScoreText;

    [SerializeField] AudioSource audioSrc;
    [SerializeField] AudioClip buttonHover, buttonClick;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        scoringSystem = GameObject.Find("ScoreManager").GetComponent<ScoringSystem>();
    }

    private void Update()
    {
        finalScoreText.text = "FINAL SCORE: " + scoringSystem.bankedScore;
    }

    public void Reload()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Quit()
    {
        gameManager.SetFadeOutMusic(true);
        SceneManager.LoadScene("MainMenuScene");
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
