using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gameIsPaused )
            {
                PauseGame();
            }
            else
            {
                ResumeGame();   
            }
        }
    }
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        gameIsPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused=false;
    }

    public void OpenMenu()
    {
        SceneManager.LoadScene("0");
    }

    public void QuitGame()
    {
        Application.Quit(); 
    }
}
