using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace PollenHub
{
    public class GameLoader : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private Slider progressBar;
        [SerializeField] private TextMeshProUGUI progressText;

        public static GameLoader Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadGameModule(string sceneName)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            // Activate the loading screen
            if(loadingScreen != null)
                loadingScreen.SetActive(true);

            // Start the asynchronous loading operation
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

            // While the scene is not completely loaded
            while (!operation.isDone)
            {
                // Calculate progress from 0 to 1
                float progress = Mathf.Clamp01(operation.progress / 0.9f);

                // Update UI elements
                if(progressBar != null)
                    progressBar.value = progress;

                if(progressText != null)
                    progressText.text = $"Loading... {progress * 100f:0}%";

                yield return null;
            }

            // Deactivate loading screen once finished
            if(loadingScreen != null)
                loadingScreen.SetActive(false);
        }
    }
}
