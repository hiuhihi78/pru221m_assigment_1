using Assets.Scripts.Constants;
using Assets.Scripts.Entity;
using DefaultNamespace;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TankController : MonoBehaviour
{
	// Start is called before the first frame update
	private Tank _tank;

	public Sprite tankUp;
	public Sprite tankDown;
	public Sprite tankLeft;
	public Sprite tankRight;
	private TankMover _tankMover;

	public string nameTank = "player1";
	//private CameraController _cameraController;
	private SpriteRenderer _renderer;

	public Tank Tank { get => _tank; }
	public TankMover TankMover { get => _tankMover; }
	public SpriteRenderer Renderer { get => _renderer; }

	public GameObject gameOverUI;
	public GameObject gameWinUI;


	private float timeStartPowerUpShovel;
	public GameObject wallStealPrefab;

	private Animation animation;

	protected virtual void Start()
	{
		_tank = new Tank
		{
			Name = "Default",
			Direction = Direction.Down,
			Hp = 10,
			Point = 0,
			Position = new Vector3(-6.49f, -3.42f, 0),
			//Guid = GUID.Generate()
		};
		//gameObject.transform.position = _tank.Position;
		_tankMover = gameObject.GetComponent<TankMover>();
		//_cameraController = camera.GetComponent<CameraController>();
		_renderer = gameObject.GetComponent<SpriteRenderer>();
		Move(Direction.Down);

		animation = GetComponent<Animation>();
	}

	// Update is called once per frame
	protected virtual void Update()
	{


		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
		{
			if (nameTank.ToLower() == "player1" && Input.GetKey(KeyCode.A))
			{
				Move(Direction.Left);
			}
			else if (nameTank.ToLower() == "player2" && Input.GetKey(KeyCode.LeftArrow))
			{
				Move(Direction.Left);
			}
		}
		else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
		{
			if (nameTank.ToLower() == "player1" && Input.GetKey(KeyCode.S))
			{
				Move(Direction.Down);
			}
			else if (nameTank.ToLower() == "player2" && Input.GetKey(KeyCode.DownArrow))
			{
				Move(Direction.Down);
			}
		}
		else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
		{
			if (nameTank.ToLower() == "player1" && Input.GetKey(KeyCode.D))
			{
				Move(Direction.Right);
			}
			else if (nameTank.ToLower() == "player2" && Input.GetKey(KeyCode.RightArrow))
			{
				Move(Direction.Right);
			}
		}
		else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
		{
			if (nameTank.ToLower() == "player1" && Input.GetKey(KeyCode.W))
			{
				Move(Direction.Up);
			}
			else if (nameTank.ToLower() == "player2" && Input.GetKey(KeyCode.UpArrow))
			{
				Move(Direction.Up);
			}
		}

		if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Keypad0))
		{
			if (nameTank.ToLower() == "player1" && Input.GetKey(KeyCode.Space))
			{
				Fire();
			}
			else if (nameTank.ToLower() == "player2" && Input.GetKey(KeyCode.Keypad0))
			{
				Fire();
			}
		}
		checkWining();


		if (Time.time > timeStartPowerUpShovel + 5)
		{
			HandleRemovePowerUpShovel();
		}
	}

	protected virtual void Move(Direction direction)
	{
		_tank.Position = _tankMover.Move(direction);
		_tank.Direction = direction;
		//_cameraController.Move(_tank.Position);
		_renderer.sprite = direction switch
		{
			Direction.Down => tankDown,
			Direction.Up => tankUp,
			Direction.Left => tankLeft,
			Direction.Right => tankRight,
			_ => _renderer.sprite
		};
	}

	private void Fire()
	{
		var bullet = new Bullet
		{
			Direction = _tank.Direction,
			Tank = _tank,
			InitialPosition = _tank.Position
		};
		var tankFirer = GetComponent<TankFirer>();
		tankFirer.isTankPlayer = true;
		tankFirer.Fire(bullet);
	}

	private void checkWining()
	{
		if (Constants.modeGameChosen == ModeGame.Solo) return;
		GameObject enemys = GameObject.Find("Enemys");
		var numberOfEnemys = enemys.transform.childCount;
		if (numberOfEnemys == 0)
		{
			gameWinUI.SetActive(true);
			Time.timeScale = 0;
		}
	}

	private void OnDestroy()
	{
		if (Constants.modeGameChosen == ModeGame.TwoPlayer)
		{
			if (GameObject.FindGameObjectsWithTag("Player").Count() != 0)
			{
				return;
			}
		}
		else if (Constants.modeGameChosen == ModeGame.Solo)
		{
			string namePlayer = transform.gameObject.name;
			gameOverUI.GetComponent<TextMeshProUGUI>().text = "Tank" == namePlayer ? "PLAYER 2 WIN" : "PLAYER 1 WIN";
			Time.timeScale = 0;
			gameOverUI.SetActive(true);
			return;
		}
		Time.timeScale = 0;
		gameOverUI.SetActive(true);
	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (ModeGame.Solo == Constants.modeGameChosen)
		{
			return;
		}
		var collisionTag = collision.gameObject.tag;
		switch (collisionTag)
		{
			case TagGameObject.powerUpTank:
				break;
			case TagGameObject.powerUpHelmet:
				HandlePowerUpHelmet();
				Console.Write("hieu");
				Destroy(collision.gameObject);
				break;
			case TagGameObject.powerUpShovel:
				HandlePowerUpShovel();
				Destroy(collision.gameObject);
				break;
		}
	}

	private void HandlePowerUpHelmet()
	{
		animation.Play("Tank_sheild");
	}

	private void HandlePowerUpShovel()
	{
		List<GameObject> wallBircks;
		wallBircks = GameObject.FindGameObjectsWithTag(TagGameObject.wallBrickParent)
			.Where(obj =>
				(obj.transform.position.x > -1f && obj.transform.position.x < 2f) &&
				(obj.transform.position.y > -4f && obj.transform.position.y < -2f)
			).ToList();
		foreach (GameObject gameObject in wallBircks)
		{
			Instantiate(wallStealPrefab, gameObject.transform.position, Quaternion.identity);
		}
		timeStartPowerUpShovel = Time.time;
	}

	private void HandleRemovePowerUpShovel()
	{
		List<GameObject> wallSteels;
		wallSteels = GameObject.FindGameObjectsWithTag(TagGameObject.wallSteel)
			.Where(obj =>
				(obj.transform.position.x > -1f && obj.transform.position.x < 2f) &&
				(obj.transform.position.y > -4f && obj.transform.position.y < -2f)
			).ToList();
		wallSteels.ForEach(wallSteel => { Destroy(wallSteel); });
	}

}