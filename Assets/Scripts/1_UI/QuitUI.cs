using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif


    public class QuitUI : MonoBehaviour
    {
        [SerializeField] Button yesButton;
        [SerializeField] Button noButton;

        void Awake()
        {
            yesButton.onClick.AddListener(YesButtonAction);
            noButton.onClick.AddListener(NoButtonAction);
        }

        void YesButtonAction()
        {
            GameServices.Instance.audioManager.PlayButtonClick();
#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
            }
#else
        Application.Quit();
#endif
        }

        void NoButtonAction()
        {
            GameServices.Instance.audioManager.PlayButtonClick();
            gameObject.SetActive(false);
        }
    }

