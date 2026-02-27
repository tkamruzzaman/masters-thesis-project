using System.Collections;
using UnityEngine;

namespace Prototype
{
    public class ComicPanelManager : MonoBehaviour
    {
        public static ComicPanelManager Instance;

        [SerializeField] ComicPanel[] comicPanels;
        public Color[] comicBackgroundColors;
        private int currentActivePanel = 0;
        private int completedPanelCount = 0;
        [SerializeField] float pauseBetweenPanels = 1f;


        void Awake()
        {
            Instance = this;
            comicPanels = transform.GetComponentsInChildren<ComicPanel>();
        }

        void Start()
        {
            for (int i = 0; i < comicPanels.Length; i++)
            {
                comicPanels[i].panelNumber = i;
                comicPanels[i].Inactive();
            }

            if (comicPanels.Length > 0)
            {
                comicPanels[0].Active();
            }
        }

        public void PanelCompleted(int panelNumber)
        {
            completedPanelCount++;

            // Check if all panels are completed
            if (completedPanelCount >= comicPanels.Length)
            {
                StartCoroutine(AllPanelsCompleted());
            }
            else
            {
                // Activate next panel
                //ActivateNextPanel(panelNumber);
                // Activate next panel after a brief pause
                StartCoroutine(ActivateNextPanelDelayed(panelNumber + 1));
            }
        }

        private IEnumerator ActivateNextPanelDelayed(int nextPanelIndex)
        {
            // Wait before activating next panel (camera is already zoomed out)
            yield return new WaitForSeconds(pauseBetweenPanels);

            if (nextPanelIndex < comicPanels.Length)
            {
                comicPanels[nextPanelIndex].Active();
                currentActivePanel = nextPanelIndex;
            }
        }

        private IEnumerator AllPanelsCompleted()
        {
            Debug.Log("All panels completed!");

            // Wait a moment before zooming out
            yield return new WaitForSeconds(2f);

            // Zoom out to show all panels
            CameraController.Instance.ZoomOut();

            // Add completion effects, UI, or restart options here
            yield return new WaitForSeconds(CameraController.Instance.zoomDuration + 1f);

            // You could add completion UI, sound effects, or restart logic here
            OnAllPanelsComplete();
        }

        private void OnAllPanelsComplete()
        {
            Debug.Log("All panels completed!");
            // any completion logic
            // Show completion UI
            Debug.Log("Comic story complete!");
        }

        //  Reset function for replaying
        public void ResetComic()
        {
            completedPanelCount = 0;
            currentActivePanel = 0;

            for (int i = 0; i < comicPanels.Length; i++)
            {
                comicPanels[i].Inactive();
            }

            if (comicPanels.Length > 0)
            {
                comicPanels[0].Active();
            }

            CameraController.Instance.ZoomOut();
        }
    }
}