using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SpawnItemsController : MonoBehaviour
    {

        public float minSpawnTime;
        public float maxSpawnTime;

        public GameObject powerUpTank;
        public GameObject powerUpShovel;
        public GameObject powerUpHelmet;


        public float topWrap;
        public float bottomWrap;
        public float leftWrap;
        public float rightWrap;


        private float randomTimeSpawnItem;
        private List<GameObject> spawnItems;
        private float spawnTime;

        void Start()
        {
            spawnTime = Time.time;  
            randomTimeSpawnItem = GetRandomTimeSpawn();
            spawnItems = new List<GameObject>()
            {
                powerUpShovel, powerUpTank, powerUpHelmet
            };

        }

        void Update()
        {
            if(spawnTime > randomTimeSpawnItem) 
            {
                SpawnItem();
                randomTimeSpawnItem = Time.time + GetRandomTimeSpawn();
            }
            else
            {
                spawnTime = Time.time;
            }
        }

        void SpawnItem()
        {
            Instantiate(GetRandomObjectItem(), GetRandomPositionItem(), Quaternion.identity);
        }

        private float GetRandomTimeSpawn()
        {
            return Random.Range(minSpawnTime, maxSpawnTime); ;
        }

        private Vector2 GetRandomPositionItem()
        {
            Vector2 result = new Vector2();
            result.x = Random.Range(leftWrap, rightWrap);
            result.y = Random.Range(bottomWrap, topWrap);
            return result;
        }

        private GameObject GetRandomObjectItem()
        {
            int random = Random.Range(0, spawnItems.Count - 1);
            return spawnItems[2];
        }

    }
}
