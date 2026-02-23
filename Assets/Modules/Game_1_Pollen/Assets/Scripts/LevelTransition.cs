using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PollenModule
{
    ﻿using System.Collections;

    public class LevelTransition : MonoBehaviour
    {
        [SerializeField] TextMeshPro transitionText;
        [SerializeField] List<GameObject> confetiSFX;
        TextMeshPro[] allTexts;

        // Start is called before the first frame update
        void Start()
        {

        }
        public IEnumerator StartTransitionSequence(){
            foreach (var confeti in confetiSFX) { confeti.GetComponent<InteractableSFX>().Play(); }
            ClearScreen();
            SetTransitionText();
            transitionText.gameObject.SetActive(true);
            yield return new WaitForSeconds(3f);
            RestoreScreen();
            transitionText.gameObject.SetActive(false);
        }

        void SetTransitionText(){
            LevelCreator levelInfo = FindObjectOfType<LevelCreator>();
            int currentLevel = levelInfo.currentLevel;
            string textString = "";
            if (currentLevel <= levelInfo.levelInfo.Count)
            {
                textString = $"Nivel {currentLevel + 1}";
            } else
            {
                textString = $"Nivel infinito!";
            }

            transitionText.text = textString;
        }

        void ClearScreen(){
            allTexts = FindObjectsOfType<TextMeshPro>();
            foreach (var text in allTexts)
            {
                text.gameObject.SetActive(false);
            }
        }

        void RestoreScreen(){
            foreach (var text in allTexts)
            {
                text.gameObject.SetActive(true);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
