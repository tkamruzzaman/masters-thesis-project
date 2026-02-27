using UnityEngine;
using DG.Tweening;

namespace Prototype
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance;

        [SerializeField] Camera mainCamera;
        [SerializeField] float zoomedInSize = 3f;
        [SerializeField] float zoomedOutSize = 5f;
        [SerializeField] public float zoomDuration = 1f;
        [SerializeField] Ease zoomEase = Ease.OutCubic;

        private Vector3 originalPosition;
        private float originalSize;
        private bool isZoomedIn = false;

        void Awake()
        {
            Instance = this;

            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            originalPosition = mainCamera.transform.position;
            originalSize = mainCamera.orthographicSize;
            zoomedOutSize = originalSize;
        }

        public void ZoomToPanel(ComicPanel panel)
        {
            if (isZoomedIn)
            {
                return;
            }

            isZoomedIn = true;

            Vector3 targetPosition = new(
                panel.transform.position.x,
                panel.transform.position.y,
                originalPosition.z
            );

            // Zoom in sequence
            Sequence zoomSequence = DOTween.Sequence();

            zoomSequence.Append(mainCamera.transform.DOMove(targetPosition, zoomDuration).SetEase(zoomEase));
            zoomSequence.Join(DOTween.To(() => mainCamera.orthographicSize,
                x => mainCamera.orthographicSize = x, zoomedInSize, zoomDuration).SetEase(zoomEase));
        }

        public void ZoomOut()
        {
            if (!isZoomedIn)
            {
                return;
            }

            isZoomedIn = false;

            // Zoom out sequence
            Sequence zoomSequence = DOTween.Sequence();

            if (Draging.Instance.currentPanel.panelNumber > 1)
            {
                originalPosition.y = -6f;
            }

            if (Draging.Instance.currentPanel.panelNumber > 2)
            {
                originalPosition.y = -12f;
            }

            zoomSequence.Append(mainCamera.transform.DOMove(originalPosition, zoomDuration).SetEase(zoomEase));
            zoomSequence.Join(DOTween.To(() => mainCamera.orthographicSize,
                x => mainCamera.orthographicSize = x, zoomedOutSize, zoomDuration).SetEase(zoomEase));
        }

        public bool IsZoomedIn => isZoomedIn;
    }
}