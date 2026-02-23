using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace PollenModule
{
    ﻿using System.Collections;

    public class LevelCreator : MonoBehaviour
    {
        [System.Serializable]
        public struct LevelInfo{
            public int levelNumber;
            public int numberOfObjectives;
            public float timeLimit;
            public int numberOfPickables;
        }
        public List<LevelInfo> levelInfo;
        public int currentLevel = 0;
        private SceneTimer timer;
        [SerializeField] TextMeshPro goalText;
        [SerializeField] TextMeshPro levelText;
        private int currentCompletedGoals;
        // Start is called before the first frame update
        void Awake()
        {
            timer = FindObjectOfType<SceneTimer>();
            Initialize();
        }

        void Initialize(){
            currentCompletedGoals = 0;
            SetTimerTime();
            SetGoalsNumbers();
            SetLevelNumber();
        }

        void SetTimerTime(){
            timer.timeRemaining = levelInfo[currentLevel].timeLimit;
            timer.Restart();
        }

        void SetGoalsNumbers(){
            int numberGoal = levelInfo[currentLevel].numberOfObjectives;
            goalText.text = $"{currentCompletedGoals}/{numberGoal}";
        }

        void SetLevelNumber(){
            int numberLevel = levelInfo[currentLevel].levelNumber;
            levelText.text = $"{numberLevel}";
        }

        public void AdvanceCurrentCompletedGoals(){
            currentCompletedGoals += 1;
            SetGoalsNumbers();
            CheckCompletedGoals();
        }

        void CheckCompletedGoals(){
            if (currentCompletedGoals >= levelInfo[currentLevel].numberOfObjectives)
            {
                AdvanceCurrentLevel();
            }
        }

        void AdvanceCurrentLevel(){
            currentLevel += 1;
            if (currentLevel >= levelInfo.Count)
            {
                currentLevel -= 1;
            }
            LevelTransition transitioner = FindObjectOfType<LevelTransition>();
            StartCoroutine(transitioner.StartTransitionSequence());
            StartCoroutine(AdvanceLevel());
        }

        IEnumerator AdvanceLevel(){

            InstructionBuilder instructions = FindObjectOfType<InstructionBuilder>();

            yield return new WaitForSeconds(1f);
            Initialize();
            FindObjectOfType<RandomSpawner>().Restart();
            instructions.Restart();
        }

        public void Restart(){
            currentLevel = 0;
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
