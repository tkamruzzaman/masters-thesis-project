using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NavigationManager : MonoBehaviour
{
    [SerializeField] private int currentSceneIndex;
    [Header("Loading Effect")]
    [SerializeField] private Image loadingImage;
    [SerializeField] float fadeInDuration = 1.0f;
    [SerializeField] float fadeOutDuration = 1.0f;
    [SerializeField] Ease fadeInEase = Ease.Linear;
    [SerializeField] Ease fadeOutEase = Ease.Linear;

    private void Awake()
    {
        loadingImage.DOFade(0, 0).OnComplete(() =>
        {
            loadingImage.gameObject.SetActive(false);
        });
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        currentSceneIndex = scene.buildIndex;
        //print(scene.name + "   " + scene.buildIndex);
        loadingImage.DOFade(0, fadeOutDuration).SetEase(fadeOutEase).OnComplete(() =>
        {
            loadingImage.gameObject.SetActive(false);
        });
    }

    public Scenes GetCurrentScene() => (Scenes)SceneManager.GetActiveScene().buildIndex;

    public void LoadScene(Scenes scene)
    {
        loadingImage.gameObject.SetActive(true);
        loadingImage.DOFade(1, fadeInDuration).SetEase(fadeInEase).OnComplete(() =>
        {
            SceneManager.LoadScene((int)scene);
        });
    }
    
    public int GetCurrentGameSceneIndex()
    {
        Scenes currentScene = GetCurrentScene();
        return currentScene != Scenes.Menu 
               || currentScene != Scenes.Selection 
               || currentScene != Scenes.End
            ? (int)currentScene
            : -1;
    }
}

public enum Scenes
{
    Menu = 0,
    Selection = 1,
    Scene1 = 2,
    Scene2 = 3,
    Scene3 = 4,
    Scene4 = 5,
    Scene5 = 6,
    Scene6 = 7,
    End = 8,
}
