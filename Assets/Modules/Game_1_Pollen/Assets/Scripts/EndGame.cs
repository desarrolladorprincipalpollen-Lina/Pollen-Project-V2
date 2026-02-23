using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PollenModule
{
    ﻿using System.Collections;

    public class EndGame : MonoBehaviour
    {
        [SerializeField] Canvas canvas;
        //[SerializeField] GameObject sound;
        private AudioClip sound;
        private TextMeshPro[] allTexts;
        // Start is called before the first frame update
        void Start()
        {
            canvas.gameObject.SetActive(false);
            sound = Resources.Load("Audio/train-whistle") as AudioClip;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartEndGameSequence() {
            this.GetComponent<AudioSource>().PlayOneShot(sound);
            ClearBoard();
            canvas.gameObject.SetActive(true);
        }

        public void ExitGame(){
            Application.Quit();
        }

        public void RestartGame(){
            foreach (var text in allTexts)
            {
                text.gameObject.SetActive(true);
            }
            canvas.gameObject.SetActive(false);
            LevelCreator levelCreator = FindObjectOfType<LevelCreator>();
            levelCreator.Restart();
            Scoreboard scoreboard = FindObjectOfType<Scoreboard>();
            scoreboard.ResetScore();
            RandomSpawner randomSpawner = FindObjectOfType<RandomSpawner>();
            randomSpawner.Restart();
            InstructionBuilder instructionBuilder = FindObjectOfType<InstructionBuilder>();
            instructionBuilder.Restart();

        }

        void ClearBoard(){
            PickableHandler[] pickables = FindObjectsOfType<PickableHandler>();
            ContainerHandler[] containers = FindObjectsOfType<ContainerHandler>();
            allTexts = FindObjectsOfType<TextMeshPro>();
            foreach (var pickable in pickables)
            {
                pickable.gameObject.SetActive(false);
            }

            foreach (var container in containers)
            {
                container.gameObject.SetActive(false);
            }
            foreach (TextMeshPro text in allTexts)
            {
                if (text.name != "Scoreboard")
                {
                    text.gameObject.SetActive(false);   
                } 
            }
        }


    }
}
