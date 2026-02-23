using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PollenModule
{
    ﻿using System.Collections;

    public class RandomSpawner : MonoBehaviour
    {
        [SerializeField] List<GameObject> pickableSpawnPoints;
        [SerializeField] List<GameObject> pickablesToSpawn;
        [SerializeField] List<GameObject> containerSpawnPoints;
        [SerializeField] List<GameObject> containersToSpawn;
        private List<GameObject> instantiatedPickables = new List<GameObject>();
        public List<GameObject> InstantiatedPickables => instantiatedPickables;
        private List<GameObject> instantiatedContainers = new List<GameObject>();
        public List<GameObject> InstantiatedContainers => instantiatedContainers;
        private LevelCreator levelCreator;
        private List<GameObject> pickablePool = new List<GameObject>();
        // Start is called before the first frame update
        void Awake()
        {
            StartCoroutine(Prepare());
        }

        public IEnumerator Prepare(){
            FillPickablePool();
            levelCreator = FindObjectOfType<LevelCreator>();
            yield return new WaitUntil(() => levelCreator.levelInfo[0].numberOfPickables > 1);
            int currentLevel = levelCreator.currentLevel;
            int numberOfPickables = levelCreator.levelInfo[currentLevel].numberOfPickables;
            InstantiatePickables(numberOfPickables);
            InstantiateContainers();
        }

        private void FillPickablePool(){
            foreach (var pickable in pickablesToSpawn)
            {
                GameObject item = Instantiate(pickable);
                item.SetActive(false);
                pickablePool.Add(item);
            }
        }

        void InstantiatePickables(int amount){
            //instantiatedPickables = GenericInstantiator(pickablesToSpawn,pickableSpawnPoints,amount);
            List<int> indexes = RandomListGenerator(pickablesToSpawn.Count, amount);
            for (int i = 0; i < amount && i< pickableSpawnPoints.Count; i++)
            {
                var spawnPoint = pickableSpawnPoints[i];
                GameObject pickable = pickablePool[indexes[i]];
                pickable.transform.position = spawnPoint.transform.position;
                pickable.transform.rotation = spawnPoint.transform.rotation;
                pickable.SetActive(true);
                instantiatedPickables.Add(pickable);
            }
        }

        void InstantiateContainers(){
            instantiatedContainers = GenericInstantiator(containersToSpawn,containerSpawnPoints, containerSpawnPoints.Count);
        }

        List<GameObject> GenericInstantiator(List<GameObject> objectsToInstantiate, List<GameObject> spawnPoints, int max){
            List<GameObject> intstantiatedObjects = new List<GameObject>();
            //List<int> indexes = RandomListGenerator(objectsToInstantiate.Count, spawnPoints.Count);
            List<int> indexes = RandomListGenerator(objectsToInstantiate.Count, max);
            /*int i = 0;
            foreach (var spawnPoint in spawnPoints)
            {
                GameObject thing = Instantiate(objectsToInstantiate[indexes[i]],spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
                intstantiatedObjects.Add(thing);
                i += 1;
            }*/

            for (int i = 0; i < max && i < spawnPoints.Count; i++)
            {
                var spawnPoint = spawnPoints[i];
                GameObject thing = Instantiate(objectsToInstantiate[indexes[i]],spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
                intstantiatedObjects.Add(thing);
            }
            return intstantiatedObjects;
        }

    //This method creates a list of non-repeating int numbers, given a random range and a size
    //Range is the max random number to generate, size is the number of random numbers to generate
        private List<int> RandomListGenerator(int range, int size){
            List<int> randomNumbers = new List<int>();
            int number;
            for (int i = 0; i < size; i++)
            {
                do
                {
                    number = Random.Range(0, range-1);
                } while (randomNumbers.Contains(number));
                randomNumbers.Add(number);
            }
            return randomNumbers;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Restart() {
            foreach (var pickable in instantiatedPickables)
            {
                //Destroy(pickable);
                pickable.SetActive(false);
            }

            /*for (int i = 0; i < instantiatedPickables.Count; i++)
            {
                GameObject thing = instantiatedPickables[i];
                thing.SetActive(false);
                instantiatedPickables.RemoveAt(i);
                i--;
            }*/
            instantiatedPickables.Clear();
            foreach (var container in instantiatedContainers)
            {
                Destroy(container);
            }
            StartCoroutine(Prepare());
        }
    }
}
