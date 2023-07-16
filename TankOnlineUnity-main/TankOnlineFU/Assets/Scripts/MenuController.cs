using Assets.Scripts.Constants;
using Assets.Scripts.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{
	public new Camera camera;
	public GameObject buttonLevel;
	private void Start()
	{
		RenderButtonChoseLevelGame();
	}
	public void OpenSelectMap()
	{
		Vector3 selectMapCameraPostion = new Vector3(75, 0, -10);
		camera.transform.Translate(selectMapCameraPostion);
		LoadMap();
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void BackMenu()
	{
		Vector3 menuCameraPosition = new Vector3(0, 0, -10);
		camera.transform.position = menuCameraPosition;
	}

	public void StartGameDefaultMap1()
	{
		SceneManager.LoadScene("SampleScene");
	}

	private void LoadMap()
	{

	}

	public void OpenContructionSence()
	{
		SceneManager.LoadScene("ConstructionScene");
	}

	private void EnableButtonLevel(string level)
	{
		try
		{
			GameObject buttonlevel = this.gameObject.transform.Find("ButtonLevel" + level).gameObject;

			buttonlevel.SetActive(true);
		}
		catch (NullReferenceException)
		{
			return;
		}

	}

	public void RenderButtonChoseLevelGame()
	{
		ConstructController constructController = new ConstructController();
		int currentMapContructed = constructController.GetLastIndexFileInFolder();

		if (currentMapContructed == 0)
		{
			Vector3 position = this.gameObject.transform.Find("ButtonLevel2").position;
			RenderContructButton(position);
		}

		if (currentMapContructed == 8) return;

		for (int row = 0; row <= 1; row++)
		{
			for (int col = 1; col <= 4; col++)
			{

				int currentButton = row * 4 + col;

				if (currentButton == 1) continue;

				if (currentButton - 2 == currentMapContructed)
				{
					if (currentMapContructed < 8)
					{
						try
						{
							Vector3 position = this.gameObject.transform.Find("ButtonLevel" + currentButton).position;
							RenderContructButton(position);
						}
						catch (NullReferenceException)
						{

							continue;
						}

					}
					return;
				}

				EnableButtonLevel(currentButton.ToString());

			}
		}
	}

	public void RenderContructButton(Vector3 position)
	{
		GameObject button = GameObject.Find("ButtonContructionMap"); ;
		button.transform.position = position;
	}

	public void OpenContructedSecnce(int map)
	{
		Constants.mapContructChosed = map;
		SceneManager.LoadScene("MapContructedSence");
	}

	public void SetMode(int mode)
	{
		Constants.modeGameChosen = (ModeGame)mode;
		if(Constants.modeGameChosen == ModeGame.Solo)
		{
			SceneManager.LoadScene("SoloScene");
		}
	}
}
