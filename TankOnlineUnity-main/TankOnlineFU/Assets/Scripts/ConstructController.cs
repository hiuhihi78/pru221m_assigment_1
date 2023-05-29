using Assets.Scripts.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class ConstructController : MonoBehaviour
{
	public GameObject pointerGameObject;
	public GameObject wallBrickPrefab;
	public GameObject wallSteelPrefab;
	public GameObject grassPrefab;
	public GameObject waterPrefab;

	private string path;
	private string root = "Assets/Construct";

	// Start is called before the first frame update
	void Start()
	{
		path = root + "/test.json";
		if (!File.Exists(path))
		{
			File.Create(path).Close();
		}
		else
		{
			File.WriteAllText(path, string.Empty);
		}
	}

	// Update is called once per frame
	void Update()
	{

		Vector3 positionSpawn = pointerGameObject.transform.position;
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			CreateGameObject(wallSteelPrefab, positionSpawn);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			CreateGameObject(wallBrickPrefab, positionSpawn);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			CreateGameObject(waterPrefab, positionSpawn);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			CreateGameObject(grassPrefab, positionSpawn);
		}
		else if (Input.GetKeyDown(KeyCode.Backspace))
		{
			RemoveGameObjectSelected(positionSpawn);
		}
		if (Input.GetKeyDown(KeyCode.U))
		{
			Debug.Log("Save map");
			SaveIntoJson();
		}
	}

	private void CreateGameObject(GameObject gObject, Vector3 positionSpawn)
	{
		RemoveGameObjectSelected(positionSpawn);
		GameObject gameObject1 = Instantiate(gObject, positionSpawn, Quaternion.identity);
		if (gameObject1.tag == "wallBrick" || gameObject1.tag == "wallSteel")
		{
			for (int i = 0; i < 4; i++)
			{
				gameObject1.transform.GetChild(i).gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

			}
		}
		else
		{
			gameObject1.GetComponent<BoxCollider2D>().isTrigger = true;
		}
	}
	private void RemoveGameObjectSelected(Vector3 positionSpawn)
	{
		var gameObjects = OverlapGameObjects(positionSpawn);
		if (gameObjects.Count > 0)
		{
			GameObject parentObj = null;
			if (gameObjects.Count > 1)
			{
				parentObj = gameObjects[0].transform.parent.gameObject;
			}
			foreach (var g in gameObjects)
			{
				Destroy(g);
			}
			if (parentObj != null)
			{
				Destroy(parentObj.gameObject);
			}
		}
	}

	public List<GameObject> OverlapGameObjects(Vector3 postion)
	{
		var gameObjects = Physics2D.OverlapPointAll(postion)
			.Where(g => g.gameObject != pointerGameObject
			//&& g.gameObject.transform.position.x == postion.x
			//&& g.gameObject.transform.position.y == postion.y
			)
			.Select(x => x.gameObject).ToList();
		return gameObjects;
	}

	public void SaveIntoJson()
	{
		GameObject[] listWallBrick = GameObject.FindGameObjectsWithTag("wallBrick");
		GameObject[] listWallSteel = GameObject.FindGameObjectsWithTag("wallSteel");
		GameObject[] listWater = GameObject.FindGameObjectsWithTag("water");
		GameObject[] listGrass = GameObject.FindGameObjectsWithTag("grass");
		//GameObject baseGObject = GameObject.FindGameObjectWithTag("base");
		ListData lsdata = new ListData();
		if (listWallBrick.Length > 0)
		{
			foreach (var wb in listWallBrick)
			{
				lsdata.Data.Add(new Data
				{
					Position = wb.transform.position,
					Type = Assets.Scripts.Entity.Material.WallBrick
				});
			}
		}
		if (listWallSteel.Length > 0)
		{
			foreach (var ws in listWallSteel)
			{
				lsdata.Data.Add(new Data
				{
					Position = ws.transform.position,
					Type = Assets.Scripts.Entity.Material.WallSteel
				});
			}
		}

		if (listWater.Length > 0)
		{
			foreach (var wt in listWater)
			{
				lsdata.Data.Add(new Data
				{
					Position = wt.transform.position,
					Type = Assets.Scripts.Entity.Material.Water
				});
			}
		}

		if (listGrass.Length > 0)
		{
			foreach (var gr in listGrass)
			{
				lsdata.Data.Add(new Data
				{
					Position = gr.transform.position,
					Type = Assets.Scripts.Entity.Material.Grass
				});
			}
		}


		string dataJson = JsonUtility.ToJson(lsdata, true);
		File.WriteAllText(path, dataJson);
	}
	public void LoadFromJson()
	{
		string lsdata = File.ReadAllText(path + "/data.json");
		ListData dataLoaded = JsonUtility.FromJson(lsdata, typeof(ListData)) as ListData;
	}

	public int GetLastIndexFileInFolder()
	{
		DirectoryInfo d = new DirectoryInfo(root); //Assuming Test is your Folder

		FileInfo[] Files = d.GetFiles("*.json").OrderByDescending(c => c.Name).ToArray(); //Getting Text files
		if (Files.Length == 0)
		{
			return 0;
		}
		else
		{
			try
			{
				string index = (Files[0].Name).Substring(4, (Files[0].Name).LastIndexOf(".json"));
				return int.Parse(index);
			}
			catch (Exception)
			{

				return 0;
			}
		}
	}
}
