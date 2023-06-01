using Assets.Scripts.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

	private string root = "Assets/Construct";
	private string prefix = "Map";

	// Start is called before the first frame update
	void Start()
	{
		
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
		if (Input.GetKeyDown(KeyCode.Return))
		{
			Debug.Log("Save map");
			SaveIntoJson();
		}
	}

	private void CreateGameObject(GameObject gObject, Vector3 positionSpawn)
	{
		RemoveGameObjectSelected(positionSpawn);
		GameObject gameObject1 = Instantiate(gObject, positionSpawn, Quaternion.identity);
		if(gameObject1.transform.childCount > 0)
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
			.Where(g => g.gameObject != pointerGameObject)
			.Select(x => x.gameObject).ToList();
		return gameObjects;
	}

	public void SaveIntoJson()
	{
		GameObject[] listWallBrick = GameObject.FindGameObjectsWithTag("wallBrickParent");
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
		string path = $"{root}/{prefix}{GetLastIndexFileInFolder() + 1}.json";
		if (!File.Exists(path))
		{
			File.Create(path).Close();
		}
		else
		{
			File.WriteAllText(path, string.Empty);
		}
		string dataJson = JsonUtility.ToJson(lsdata, true);
		File.WriteAllText(path, dataJson);
	}
	public ListData LoadFromJson(int map)
	{
		string lsdata = File.ReadAllText(root + "/Map"+ map +".json");
		ListData dataLoaded = JsonUtility.FromJson(lsdata, typeof(ListData)) as ListData;
		return dataLoaded;
	}

	public int GetLastIndexFileInFolder()
	{
		DirectoryInfo d = new DirectoryInfo(root);
		Regex regex = new Regex("^Map[0-9]{1,}.json$");
		FileInfo[] files = d.GetFiles("*.json").Where(x => regex.IsMatch(x.Name)).OrderByDescending(c => c.CreationTime).ToArray(); //Getting Text files
		if (files.Length == 0)
		{
			return 0;
		}
		else
		{
			try
			{
				string filename = files[0].Name;
				string index = filename.Replace("Map", "").Replace(".json", "");
				return int.Parse(index);
			}
			catch (Exception)
			{

				return 0;
			}
		}
	}
}
