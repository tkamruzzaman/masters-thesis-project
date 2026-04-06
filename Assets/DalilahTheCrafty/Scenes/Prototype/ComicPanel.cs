using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Prototype
{
    public class ComicPanel : MonoBehaviour
    {
        public int panelNumber;

        [SerializeField] SpriteRenderer backgroundSR;
        [SerializeField] GameObject[] interactableObjects; // Objects to show when active
        [SerializeField] Animator panelAnimator; // For animations
        private ComicPanelState currentState = ComicPanelState.Inactive;

        public ComicPanelState CurrentState
        {
            get => currentState;
            private set => currentState = value;
        }

        private bool isPlayingContent = false;
        public bool IsPlayingContent => isPlayingContent;
        [SerializeField] UnityEvent onPanelActivated;
        [SerializeField] UnityEvent onContentStarted;
        [SerializeField] UnityEvent onPanelCompleted;

        public Transform spawnTransform;


        void Awake()
        {
            backgroundSR = GetComponent<SpriteRenderer>();
            ShowInteractiveObjects(false);
        }

        public void Inactive()
        {
            currentState = ComicPanelState.Inactive;
            backgroundSR.color = ComicPanelManager.Instance.comicBackgroundColors[0];
            ShowInteractiveObjects(false);
        }

        public void Active()
        {
            currentState = ComicPanelState.Active;
            backgroundSR.color = ComicPanelManager.Instance.comicBackgroundColors[1];
            ShowInteractiveObjects(true);

            onPanelActivated.Invoke();


        }

        public void StartContent()
        {
            if (currentState == ComicPanelState.Active && !isPlayingContent)
            {
                isPlayingContent = true;
                // Zoom camera to this panel
                CameraController.Instance.ZoomToPanel(this);
                onContentStarted.Invoke();
                //StartCoroutine(PlayContent());
                // Wait for zoom to complete before starting content
                StartCoroutine(PlayPanelContentAfterZoom());
            }
        }

        private IEnumerator PlayPanelContentAfterZoom()
        {
            // Wait for zoom animation to complete
            yield return new WaitForSeconds(CameraController.Instance.zoomDuration);

            // Play animations
            if (panelAnimator != null)
            {
                panelAnimator.SetTrigger("Play");

                // Wait for animation to complete
                AnimatorStateInfo stateInfo = panelAnimator.GetCurrentAnimatorStateInfo(0);
                yield return new WaitForSeconds(stateInfo.length);
            }

            // Wait a bit more if needed
            yield return new WaitForSeconds(0.5f);

            // Now complete the panel
            Complete();
        }

        public void Complete()
        {
            currentState = ComicPanelState.Completed;
            backgroundSR.color = ComicPanelManager.Instance.comicBackgroundColors[2];
            isPlayingContent = false;

            onPanelCompleted.Invoke();
            // Activate next panel
            //ComicPanelManager.Instance.ActivateNextPanel(panelNumber);
            // Tell manager this panel is complete
            //ComicPanelManager.Instance.PanelCompleted(panelNumber);
            // Start completion sequence with zoom out
            StartCoroutine(CompleteWithZoomOut());
        }

        private IEnumerator CompleteWithZoomOut()
        {
            // Zoom out to show all panels
            CameraController.Instance.ZoomOut();

            // Wait for zoom out to complete
            yield return new WaitForSeconds(CameraController.Instance.zoomDuration);

            // Brief pause to show the overview
            yield return new WaitForSeconds(0.5f);

            // Tell manager this panel is complete
            ComicPanelManager.Instance.PanelCompleted(panelNumber);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (currentState == ComicPanelState.Inactive)
                {
                    //Active();
                }
            }
        }

        void ShowInteractiveObjects(bool status)
        {
            foreach (GameObject obj in interactableObjects)
            {
                obj.SetActive(status);
            }
        }

        //  completion testing 
        public void ForceComplete()
        {
            if (isPlayingContent)
            {
                StopAllCoroutines();
                Complete();
            }
        }
    }

    public enum ComicPanelState
    {
        Inactive = 0,
        Active = 1,
        Completed = 2,
    }
}