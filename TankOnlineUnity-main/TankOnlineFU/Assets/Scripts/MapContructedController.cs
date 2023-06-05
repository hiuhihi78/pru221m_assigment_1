using Assets.Scripts.Constants;
using Assets.Scripts.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapContructedController : MonoBehaviour
{

    private int currentMap;
    private string root = "Assets/Construct";
    private ListData listObjectWillCreate;

    public GameObject wallBrickPrefab;
    public GameObject wallSteelPrefab;
    public GameObject grassPrefab;
    public GameObject waterPrefab;
    public GameObject tankEnemyPrefab;
    public GameObject tankPlayerPrefab;
    public GameObject gameOverUI;
    public GameObject gameWinUI;
    private int numberEnemyTank;

    private void Awake()
    {
        currentMap = Constants.mapContructChosed;
    }

    // Start is called before the first frame update
    void Start()
    {
        numberEnemyTank = Random.Range(3, 5);
        listObjectWillCreate = LoadFromJson(currentMap);
        
        RenderRandomTank();
        RenderTankPlayer();

        RenderObjectInToMap(listObjectWillCreate);
        Time.timeScale = 1;
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
        checkWining();
        checkLosing();
    }

    private void CreateGameObject(GameObject gObject, Vector3 positionSpawn)
    {
        GameObject gameObject1 = Instantiate(gObject, positionSpawn, Quaternion.identity);
    }

    private ListData LoadFromJson(int map)
    {
        string lsdata = File.ReadAllText(root + "/Map" + map + ".json");
        ListData dataLoaded = JsonUtility.FromJson(lsdata, typeof(ListData)) as ListData;
        return dataLoaded;
    }

    private void RenderRandomTank()
    {
        for (int i = 0; i < numberEnemyTank; i++)
        {
            Vector3 position = GetRandomPosition(3,1,-6,7);
            RemoveObjectsOverlap(position);
            //RemoveGameObjectSelected(position);
            CreateGameObject(tankEnemyPrefab, position);
        }
    }

    private void RenderTankPlayer()
    {
        Vector3 position = GetRandomPosition(0,-3,-6,7);
        RemoveObjectsOverlap(position);
        //RemoveGameObjectSelected(position);
        CreateGameObject(tankPlayerPrefab, position);
    }

    private Vector3 GetRandomPosition(int maxPositionTop, int maxPositionBottom, int maxPositionLeft, int maxPositionRight)
    {
        float reduce = 0.5f;

        Vector2 basePosition = new Vector2(0.5f, -3.5f);

        float positionX = UnityEngine.Random.Range(maxPositionLeft, maxPositionRight);
        positionX = positionX > 0 ? positionX - reduce : positionX + reduce;
        float positionY = UnityEngine.Random.Range(maxPositionBottom, maxPositionTop);
        positionY = positionY > 0 ? positionY - reduce : positionY + reduce;

        if (positionX == basePosition.x && positionY == basePosition.y)
        {
            positionX = -6.46f;
            positionY = -3.57f;
        }

        return new Vector3(positionX, positionY, 0);
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
            .Select(x => x.gameObject).ToList();
        return gameObjects;
    }

    public void RemoveObjectsOverlap(Vector3 postion)
    {
        //postion = new Vector3(2.5f, -1.5f, 0);
        List<Data> listRemoveObjects = new List<Data>();
        foreach (var item in listObjectWillCreate.Data)
        {
            if (postion.x == RoundNumber(item.Position.x) && postion.y == RoundNumber(item.Position.y))
            {
                listRemoveObjects.Add(item);
            }
        }

        foreach (var item in listRemoveObjects)
        {
            listObjectWillCreate.Data.Remove(item);
        }
        
    }

    private double RoundNumber(float number)
    {
        double result = Math.Round(number, 1);
        return result;
    }

    private void checkWining()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        var numberOfEnemys = enemys.Length;
        if (numberOfEnemys == 0)
        {
            gameWinUI.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void checkLosing()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player == null)
        {
            gameOverUI.SetActive(true);
            Time.timeScale = 0;
        }
    }

}
