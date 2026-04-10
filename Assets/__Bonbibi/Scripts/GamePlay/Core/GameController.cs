using UnityEngine;

namespace Bonbibi
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] Scenes[] gameScenes;
        [SerializeField] private int currentSceneIndex;

        public void LoadNextGameScene()
        {
            currentSceneIndex = (int)GameServices.Instance.navigationManager.GetCurrentScene();
            GameServices.Instance.navigationManager.LoadScene((Scenes)currentSceneIndex + 1);
        }
        
        public void ResetGame()
        {
            GameState.Reset();
            //Load Menu??
        }
    }
}