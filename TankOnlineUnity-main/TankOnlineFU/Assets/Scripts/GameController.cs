using Assets.Scripts.Constants;
using Assets.Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public static bool gameIsPaused = false;
	public GameObject pauseMenuUI;
	private void Start()
	{
		if (Constants.modeGameChosen != ModeGame.TwoPlayer)
		{
			GameObject player2 = GameObject.FindGameObjectsWithTag("Player").Where(x => x.name == "Tank2").FirstOrDefault();
			player2?.SetActive(false);
		}
		Time.timeScale = 1;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (gameIsPaused)
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
		gameIsPaused = false;
	}

	public void OpenMenu()
	{
		SceneManager.LoadScene("MenuSence");
		Time.timeScale = 1;
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
