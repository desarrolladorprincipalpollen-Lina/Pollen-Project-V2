using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PollenModule
{
    ﻿using System.Collections;

    public class Scoreboard : MonoBehaviour
    {
        [SerializeField] TextMeshPro scoreText;
        private string score;
        private int rigth = 0;
        private int wrong = 0;
        private LevelCreator levelCreator;
        // Start is called before the first frame update
        void Start()
        {
            UpdateAndShowText();
            levelCreator = FindObjectOfType<LevelCreator>();
        }

        void UpdateAndShowText(){
            score = $"Correcto: {rigth}     Incorrecto: {wrong}";
            scoreText.text = score;
        }

        public void IncreaseRigthScore(){
            rigth++;
            UpdateAndShowText();
            levelCreator.AdvanceCurrentCompletedGoals();
        }

        public void IncreaseWrongScore(){
            wrong++;
            UpdateAndShowText();
        }

        public void ResetScore(){
            rigth = 0;
            wrong = 0;
            UpdateAndShowText();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
