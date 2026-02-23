using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PollenModule
{
    ﻿using System.Collections;

    public class SceneTimer : MonoBehaviour
    {
        [SerializeField] public float timeRemaining = 90.0f;
        private bool timeIsRunning = false;
        public bool TimeIsRunning => timeIsRunning;
        private bool last5Seconds = false;
        private bool firstStart = true;
        private bool last5SecPlay = true;
        [SerializeField] string timeEndMessage;
        [SerializeField] private TextMeshPro timerText;
        AudioClip startTimerSound;
        AudioClip last5secondssound;

        // Start is called before the first frame update
        void Start()
        {
            startTimerSound = Resources.Load("Audio/timer-tiking-start") as AudioClip;
            last5secondssound = Resources.Load("Audio/clock-last-5-seconds") as AudioClip;
            DisplayTime(timeRemaining);
        }

        // Update is called once per frame
        void Update()
        {
            CheckTimeLeft();
        }

        private void CheckTimeLeft(){
            if (timeIsRunning)
            {
                if (firstStart)
                {
                    this.GetComponent<AudioSource>().PlayOneShot(startTimerSound);
                    firstStart = false;
                }
                if (timeRemaining <= 5)
                {
                    last5Seconds = true;
                    if (last5SecPlay)
                    {
                        this.GetComponent<AudioSource>().PlayOneShot(last5secondssound);
                        last5SecPlay = false;
                    }
                }
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                    DisplayTime(timeRemaining);
                }
                else {
                    Debug.Log("Time's UP!");
                    timeRemaining = 0;
                    timeIsRunning = false;
                    timerText.text = timeEndMessage;
                    FindObjectOfType<EndGame>().StartEndGameSequence();
                }
            }
        }

        public void StartTimer(){
            timeIsRunning = true;
        }

        public void PauseTimer(){
            timeIsRunning = false;
        }

        private void DisplayTime(float timeToDisplay){
            timeToDisplay += 1;
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);

            //timerText.text = string.Format("{0:00}:{1:00}",minutes,seconds);
            FormatedStringOutput(minutes, seconds);
        }

        void FormatedStringOutput(float minutes, float seconds){
            Color textColor = Color.green;
            if (last5Seconds)
            {
                textColor = (seconds % 2 == 0) ? Color.red : Color.green;
            } 
            if (timeRemaining <=0 )
            {
                textColor = Color.red;
            }
            timerText.text = string.Format($"{"{0:00}:{1:00}".AddColor(textColor)}", 
            minutes, 
            seconds);
        }

        public void Restart(){
            firstStart = true;
            last5SecPlay = true;
            DisplayTime(timeRemaining);
        }
    }
}
