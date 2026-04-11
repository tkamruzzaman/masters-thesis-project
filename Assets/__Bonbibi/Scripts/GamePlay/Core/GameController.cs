using Sirenix.OdinInspector;
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

        [Button]
        public void ResetGame()
        {
            GameState.Reset();
            //Load Menu??
        }

        [Button]
        public void ResetProgress()
        {
            GameState.ResetProgress();
        }

        public void LoadChapterSelection()
        {
            GameServices.Instance.navigationManager.LoadScene(Scenes.Selection);
        }

        public void LoadEndScene()
        {
            GameServices.Instance.navigationManager.LoadScene(Scenes.End);
        }
    }
}