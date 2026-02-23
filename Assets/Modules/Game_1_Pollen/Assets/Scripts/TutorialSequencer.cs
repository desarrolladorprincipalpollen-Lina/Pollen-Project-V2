using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace PollenModule
{
    ﻿using System.Collections;

    public class TutorialSequencer : MonoBehaviour
    {
        [SerializeField] private List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
        [SerializeField] private TextMeshProUGUI loadingText;
        [SerializeField] private GameObject interactables;
        int index = 0;
        int lastIndex;
        float progress = 0f;
        enum Intent
        {
            next,
            back,
            skip
        }
        // Start is called before the first frame update
        void Start()
        {
            texts[index].gameObject.SetActive(false);
            index = 0;
            texts[index].gameObject.SetActive(true); 
            lastIndex = texts.Count-1;
            ChangeControlView(true);
        }

        void MoveTutorial(Intent intent){
            texts[index].gameObject.SetActive(false);
            switch (intent)
            {
                case Intent.next:
                index++;
                break;
                case Intent.back:
                index--;
                break;
                case Intent.skip:
                index = lastIndex;
                break;
                default:
                index = 0;
                break;
            }
            texts[index].gameObject.SetActive(true);
            if(index == 3){
                interactables.SetActive(true);
            }
            if (index == 0 || index == 3)
            {
                ChangeControlView(true);
            } else
            {
                ChangeControlView(false);
            }
        }

        void ChangeControlView(bool showController){
            HandPresence[] handPresences = FindObjectsOfType<HandPresence>() ;
            foreach (var handPresence in handPresences)
            {
                handPresence.showController = showController;
            }

        }

        public void SkipTutorial(){
            MoveTutorial(Intent.skip);
        }

        public void AdvanceTutorial(){
            if (lastIndex > index)
            {
                MoveTutorial(Intent.next);
            } else
            {
                //SceneManager.LoadScene(1);
                OnLoadLevelClick(1);
            }
        }

        public void ReverseTutorial(){
            if (index != 0)
            {
                MoveTutorial(Intent.back);
            }
        }

        public void OnLoadLevelClick(int sceneIndex){
            StartCoroutine(LoadAsync(sceneIndex));
        }

        IEnumerator LoadAsync(int sceneIndex){
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
            operation.allowSceneActivation = false;
            while (progress < 1f)
            {
                progress = Mathf.Clamp01(operation.progress / 0.9f);
                loadingText.gameObject.SetActive(true);
                loadingText.text = "Cargando... " + (int)(progress * 100f) + "%";
                Debug.Log("Loading... " + (int)(progress * 100f) + "%");
                yield return null;
            }
            operation.allowSceneActivation = true;
        }
    }
}
