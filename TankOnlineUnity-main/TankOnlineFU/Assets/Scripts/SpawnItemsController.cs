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

        void Start()
        {
            randomTimeSpawnItem = GetRandomTimeSpawn() + Time.time;
            spawnItems = new List<GameObject>()
            {
                powerUpHelmet, powerUpTank, powerUpHelmet
            };

        }

        void Update()
        {
            if(Time.time > randomTimeSpawnItem) 
            {
                SpawnItem();
                randomTimeSpawnItem = Time.time + GetRandomTimeSpawn();
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
            var random = Random.Range(0, spawnItems.Count);
            return spawnItems[random];
        }

    }
}
