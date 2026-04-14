using Bonbibi;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionUI : MonoBehaviour
{
    [SerializeField] string[] sceneNames;

    [Header("UI Elements")]
    [SerializeField] Button[] chapterButtons;
    [SerializeField] Image[] chapterBgImages;
    [SerializeField] TMP_Text[] chapterNameTexts;
    [SerializeField] private Button menuButton;
   
    [Header("Zoom Settings")]
    [SerializeField] RectTransform selectionPanel;
    [SerializeField] float zoomScale = 4f;
    [SerializeField] float zoomDuration = 0.6f;
    [SerializeField] Ease zoomEase = Ease.InCubic;

    private bool _isTransitioning;

    private void Awake()
    {
        for (int i = 0; i < chapterNameTexts.Length; i++)
        {
            chapterNameTexts[i].text = sceneNames[i];
        }

        for (int i = 0; i < chapterButtons.Length; i++)
        {
            var button = chapterButtons[i];
            var sceneNumber = i + GameState.GAME_SCENE_INDEX_DELTA;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                if (_isTransitioning) return;
                ZoomToButton(button, sceneNumber);
            });
        }
        
        menuButton.onClick.AddListener(() =>
        {
            GameServices.Instance.navigationManager.LoadScene(Scenes.Menu);
        });
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

    private void ZoomToButton(Button button, int sceneNumber)
    {
        _isTransitioning = true;

        foreach (var b in chapterButtons)
        {
            var cg = b.GetComponent<CanvasGroup>();
            if (cg != null) cg.interactable = false;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)transform,
            button.transform.position,
            null,
            out Vector2 buttonLocalPos
        );

        Vector3 targetPosition = -(Vector3)(buttonLocalPos * zoomScale);

        selectionPanel.localScale = Vector3.one;
        selectionPanel.localPosition = Vector3.zero;

        Sequence zoomSequence = DOTween.Sequence();
        zoomSequence.Append(selectionPanel.DOScale(zoomScale, zoomDuration).SetEase(zoomEase));
        zoomSequence.Join(selectionPanel.DOLocalMove(targetPosition, zoomDuration).SetEase(zoomEase));
        zoomSequence.OnComplete(() =>
        {
            GameServices.Instance.navigationManager.LoadScene((Scenes)sceneNumber);
        });
    }
}