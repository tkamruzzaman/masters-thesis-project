using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Bonbibi
{
    public enum Scenes
    {
        Scene_1,
        Scene_2,
        Scene_3,
        Scene_4,
        Scene_5,
        Scene_6,
    }
    
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }

        public event EventHandler<OnRepositionPlayerEventArgs> OnRepositionPlayer;

        public class OnRepositionPlayerEventArgs : EventArgs
        {
            public Vector3 position;
            public Quaternion rotation;
        }
        
        [Header("Scene Order")] 
        [SerializeField] private string[] sceneNames;

        [Header("Fade")] 
        [SerializeField] private float fadeDuration = 1f;

        private int _currentSceneIndex = 0;
        private bool _isLoading = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void LoadNext()
        {
            if (_isLoading) return;

            int nextIndex = _currentSceneIndex + 1;

            if (nextIndex >= sceneNames.Length)
            {
                Debug.Log("SceneLoader: No more scenes. Game complete.");
                return;
            }

            StartCoroutine(TransitionTo(nextIndex));
        }

        public void LoadScene(string sceneName)
        {
            if (_isLoading) return;

            int index = System.Array.IndexOf(sceneNames, sceneName);

            if (index < 0)
            {
                Debug.LogWarning($"SceneLoader: Scene '{sceneName}' not found in sceneNames array.");
                return;
            }

            StartCoroutine(TransitionTo(index));
        }

        public void RestartGame()
        {
            if (_isLoading) return;

            GameState.Reset();
            StartCoroutine(TransitionTo(0));
        }
        

        private IEnumerator TransitionTo(int index)
        {
            _isLoading = true;

            // Fade out
            yield return StartCoroutine(Fade(0f, 1f));

            // Reposition player at new scene's start point
            SceneManager.sceneLoaded += OnSceneLoaded;

            // Load
            _currentSceneIndex = index;
            SceneManager.LoadScene(sceneNames[index]);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            StartCoroutine(FinishTransition());
        }

        private IEnumerator FinishTransition()
        {
            RepositionPlayer();

            // Fade in
            yield return StartCoroutine(Fade(1f, 0f));

            _isLoading = false;
        }
        

        private void RepositionPlayer()
        {
            GameObject spawnPoint = GameObject.FindWithTag("SpawnPoint");

            if (!spawnPoint)
            {
                Debug.LogWarning("SceneLoader: No SpawnPoint found in scene.");
                return;
            }
            OnRepositionPlayer?.Invoke(this, new OnRepositionPlayerEventArgs
            {
                position = spawnPoint.transform.position,
                rotation = spawnPoint.transform.rotation,
            });
        }

        private IEnumerator Fade(float from, float to)
        {
            CanvasGroup fadeCanvasGroup = FindAnyObjectByType<DialogueUI>().GetFadePanel();
            
            if (!fadeCanvasGroup) yield break;

            float elapsed = 0f;
            fadeCanvasGroup.alpha = from;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                fadeCanvasGroup.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
                yield return null;
            }

            fadeCanvasGroup.alpha = to;
        }
    }
}
