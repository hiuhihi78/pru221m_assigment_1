using Assets.Scripts.Constants;
using Assets.Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapContructedController : MonoBehaviour
{

    private int currentMap;
    private string root = "Assets/Construct";

    public GameObject wallBrickPrefab;
    public GameObject wallSteelPrefab;
    public GameObject grassPrefab;
    public GameObject waterPrefab;

    private void Awake()
    {
        currentMap = Constants.mapContructChosed;
    }

    // Start is called before the first frame update
    void Start()
    {
        ListData listObject = LoadFromJson(currentMap);
        RenderObjectInToMap(listObject);
    }

    private void RenderObjectInToMap(ListData objects)
    {
        foreach (var item in objects.Data)
        {
            switch (item.Type)
            {
                case Assets.Scripts.Entity.Material.Water:
                    CreateGameObject(waterPrefab, item.Position);
                    break;
                case Assets.Scripts.Entity.Material.WallBrick:
                    CreateGameObject(wallBrickPrefab, item.Position);
                    break;
                case Assets.Scripts.Entity.Material.Grass:
                    CreateGameObject(grassPrefab, item.Position);
                    break;
                case Assets.Scripts.Entity.Material.WallSteel:
                    CreateGameObject(wallSteelPrefab, item.Position);
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateGameObject(GameObject gObject, Vector3 positionSpawn)
    {
        GameObject gameObject1 = Instantiate(gObject, positionSpawn, Quaternion.identity);
        if (gameObject1.transform.childCount > 0)
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

    public ListData LoadFromJson(int map)
    {
        string lsdata = File.ReadAllText(root + "/Map" + map + ".json");
        ListData dataLoaded = JsonUtility.FromJson(lsdata, typeof(ListData)) as ListData;
        return dataLoaded;
    }
}
