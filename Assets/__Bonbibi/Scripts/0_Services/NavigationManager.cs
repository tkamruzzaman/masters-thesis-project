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
}

public enum Scenes
{
    Menu = 0,
    Scene1 = 1,
    Scene2 = 2,
    Scene3 = 3,
    Scene4 = 4,
    Scene5 = 5,
    Scene6 = 6,
    End = 7,
}
