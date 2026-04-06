using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationManager : MonoBehaviour
{
    [SerializeField] private Scenes scenes;
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        //print(scene.name + "   " + scene.buildIndex);
        //print(loadSceneMode);
    }

    public Scenes? GetCurrentScene()
    {
        return (Scenes)SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadScene(Scenes scene)
    {
        SceneManager.LoadScene((int)scene);
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
