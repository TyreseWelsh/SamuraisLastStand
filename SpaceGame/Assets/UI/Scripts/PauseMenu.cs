using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    GameManager gameManager;
    private void Awake()
    {
        gameManager = GameObject.Find("GameManager")?.GetComponent<GameManager>();
    }

    public void Resume()
    {
        gameManager.TogglePause();
    }

    public void QuitToMainMenu()
    {
        gameManager.TogglePause();
        SceneManager.LoadScene("MainMenu");
    }
}
