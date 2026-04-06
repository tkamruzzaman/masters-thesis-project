using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
namespace Prototype{
    public class Draging : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public static Draging Instance;
        [SerializeField] bool isDraging;
        Vector3 initialScale;
        [SerializeField] float selectionScale = 1.1f;
        [SerializeField] SpriteRenderer playerSprite;
        public ComicPanel currentPanel; // Track current panel move to comic panel manager??

        [SerializeField] Sprite[] playerSprites;
        readonly float[] playerSpriteScales = { 0.6f, 0.5f, 0.55f, 0.5f, 0.15f, 0.3f };

        void Awake()
        {
            Instance = this;
            initialScale = playerSprite.transform.localScale;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isDraging = true;
            playerSprite.transform.DOScale(initialScale * selectionScale, 0.5f);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isDraging = false;

            if (currentPanel == null
                || currentPanel.CurrentState != ComicPanelState.Active
                || currentPanel.IsPlayingContent) return;
            // Snap to spawn position
            transform.DOMove(currentPanel.spawnTransform.position, 0.3f).OnComplete(() =>
            {
                playerSprite.transform.DOScale(playerSpriteScales[currentPanel.panelNumber], 0.2f);
            });
            playerSprite.DOFade(0, 0.2f).OnComplete(() =>
            {
                playerSprite.sprite = playerSprites[currentPanel.panelNumber];
                playerSprite.DOFade(1, 0.2f);
            });
            // Start the panel's content (animations + dialogue) if any
            currentPanel.StartContent();
        }

        void Update()
        {
            if (isDraging)
            {
                transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            ComicPanel panel = collision.GetComponent<ComicPanel>();
            if (panel != null)
            {
                currentPanel = panel;
                if (panel.CurrentState == ComicPanelState.Inactive)
                {
                    panel.Active();
                }
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            ComicPanel panel = collision.GetComponent<ComicPanel>();
            if (panel != null && panel == currentPanel)
            {
                currentPanel = null;
            }
        }

    }
}