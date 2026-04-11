using Bonbibi;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SelectionUI : MonoBehaviour
{
    [SerializeField]string[] sceneNames;
    [Header("UI Elements")]
    [FormerlySerializedAs("sceneButtons")]
    [SerializeField] Button[] chapterButtons;
    [FormerlySerializedAs("sceneBgImages")] [SerializeField] Image[] chapterBgImages;
    [FormerlySerializedAs("sceneNameTexts")] [SerializeField] TMP_Text[] chapterNameTexts;
    
    private void Awake()
    {
        for (int i = 0; i < chapterNameTexts.Length; i++)
        {
            chapterNameTexts[i].text = sceneNames[i];
        }
        
        for (var i = 0; i < chapterButtons.Length; i++)
        {
            var button = chapterButtons[i];
            var sceneNumber = i + GameState.GAME_SCENE_INDEX_DELTA;
            button.onClick.AddListener((() =>
            {
                GameServices.Instance.navigationManager.LoadScene((Scenes)sceneNumber);
            }));
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        RefreshButtons();
    }

    private void RefreshButtons()
    {
        int unlocked = GameState.UnlockedChapters;

        for (int i = 0; i < chapterButtons.Length; i++)
        {
            bool isUnlocked = (i + GameState.GAME_SCENE_INDEX_DELTA) <= unlocked;

            CanvasGroup canvasGroup = chapterButtons[i].GetComponent<CanvasGroup>();
            canvasGroup.interactable = isUnlocked;
            canvasGroup.alpha = isUnlocked ? 1f : 0.4f;
        }
    }
}
