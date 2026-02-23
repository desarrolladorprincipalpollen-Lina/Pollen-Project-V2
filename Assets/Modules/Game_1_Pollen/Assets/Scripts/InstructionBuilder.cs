using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PollenModule
{
    ﻿using System.Collections;

    public class InstructionBuilder : MonoBehaviour
    {
        public TextMeshPro displayText;
        private RandomSpawner randomSpawner;
        private List<GameObject> containersInScene;
        private List<GameObject> pickablesInScene;
        private int pickableID;
        public int PickableID => pickableID;
        private int containerID;
        public int ContainerID => containerID;
        private string pickableName, pickableColor, containerName, containerColor, pickableDet, pickableCharacteristic, containerMaterial;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Prepare());
            //displayText.text = "Tome un objeto y pongalo dentro de un contenedor";
        }

        public IEnumerator Prepare(){
            randomSpawner = FindObjectOfType<RandomSpawner>();
            yield return new WaitUntil(() => randomSpawner.InstantiatedContainers.Count > 0);
            containersInScene = randomSpawner.InstantiatedContainers;
            pickablesInScene = randomSpawner.InstantiatedPickables;
            Debug.Log( "Number of pickables: " +pickablesInScene.Count);
            Debug.Log( "Number of containers: " +containersInScene.Count);
            WriteInstruction();
        }

        public void WriteInstruction(){
            int ic = Random.Range(0,containersInScene.Count);
            int ip = Random.Range(0,pickablesInScene.Count);
            GameObject pickable = pickablesInScene[ip];
            GameObject container = containersInScene[ic];

            pickableID = pickable.GetComponent<PickableHandler>().GetInstanceID();
            containerID = container.GetComponentInChildren<ContainerHandler>().GetInstanceID();

            //Debug.Log($"Pickable instanceID: {pickableID}, container instance ID: {containerID}");

            pickableName = pickable.GetComponent<PickableHandler>().pickableName;
            pickableColor = pickable.GetComponent<PickableHandler>().pickableColor;

            containerName = container.GetComponentInChildren<ContainerHandler>().containerName;
            containerColor = container.GetComponentInChildren<ContainerHandler>().containerColor;

            pickableDet = pickable.GetComponent<PickableHandler>().determinant;

            pickableCharacteristic = pickable.GetComponent<PickableHandler>().pickableCharacteristic;
            containerMaterial = container.GetComponentInChildren<ContainerHandler>().containerMaterial;

            //string instruction = $"Por favor tome {pickableDet} {pickableName} {pickableColor} y pongalo en la {containerName} {containerColor}";
            string instruction = randomizeInstruction(pickableName,pickableColor,containerName, containerColor,pickableDet,pickableCharacteristic,containerMaterial);

            //displayText.text = instruction;
            DisplayTextOnScreen(instruction);
        }

        string randomizeInstruction(string pn, string pc, string cn, string cc, string pd, string pch, string cm){
            LevelCreator levelInfo = FindObjectOfType<LevelCreator>();
            int currentLevel = levelInfo.currentLevel;
            int maxLevels = levelInfo.levelInfo.Count;
            int pickableRange = Random.Range(0,maxLevels);
            int containerRange = Random.Range(0,maxLevels);
            string pickablePart = "";
            string containerPart = "";
            string instruction;
            if(pickableRange > currentLevel){
                pickablePart = $"{pd} {pn} {pc}";
            } else
            {
                pickablePart = $" la herramienta para {pch} de color {pc}";
            }
            if (containerRange > currentLevel)
            {
                //Debug.Log("container name " + cn + "container color" + cc);
                containerPart = $"la {cn} {cc}";
            } else
            {
                //Debug.Log("container material " + cm + "container color " + cc);
                containerPart = $"el contenedor de {cm} {cc}";
            }

            instruction = $"Por favor tome {pickablePart} y póngalo en {containerPart}";
            return instruction;
        }

        public void DisplayTextOnScreen(string textToDisplay){
            displayText.text = textToDisplay;
        }

        public void ShowWrongInstructionFollowed(bool rightPickable, bool rightContainer){
            if(rightPickable && !rightContainer){
                string message = $"Incorrecto! Eso no es una {containerName} {containerColor}";
                DisplayTextOnScreen(message);
            } else if(!rightPickable && rightContainer){
                string message = $"Incorrecto! Eso no es un(a) {pickableName} {pickableColor}";
                DisplayTextOnScreen(message);
            } else {
                string message = $"Incorrecto! Eso no es un(a) {pickableName} {pickableColor} ni una {containerName} {containerColor}";
                DisplayTextOnScreen(message);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Restart(){
            StartCoroutine(Prepare());
        }
    }
}
