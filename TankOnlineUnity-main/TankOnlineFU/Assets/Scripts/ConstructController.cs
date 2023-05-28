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
	public GameObject treePrefab;
	public GameObject waterPrefab;

	private string path;

	// Start is called before the first frame update
	void Start()
	{
		path = Path.Combine(Application.dataPath+"/Construct/", "/data.json");
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
			CreateGameObject(treePrefab, positionSpawn);
		} else if(Input.GetKeyDown(KeyCode.Backspace)) {
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
		Instantiate(gObject, positionSpawn, Quaternion.identity);
	}
	private void RemoveGameObjectSelected(Vector3 positionSpawn)
	{
		var gameObjects = OverlapGameObjects(positionSpawn);
		if (gameObjects.Count > 0)
		{
			foreach (var g in gameObjects)
			{
				Destroy(g);
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
		ListData lsdata = new ListData();
		string dataJson = JsonUtility.ToJson(lsdata, true);
		File.WriteAllText(path, dataJson);
	}
	public void LoadFromJson()
	{
		string lsdata = File.ReadAllText(Application.dataPath + "/data.json");
		ListData dataLoaded = JsonUtility.FromJson(lsdata, typeof(ListData)) as ListData;
	}
}
