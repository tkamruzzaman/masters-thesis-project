using UnityEngine;
using UnityEngine.UI;


    public class MainMenuUI : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] Button playButton;
        [SerializeField] Button optionButton;
        [SerializeField] Button creditButton;
        [SerializeField] Button quitButton;
        [Header("Modal Prefabs")]
        [SerializeField] GameObject optionPanelPrefab;
        [SerializeField] GameObject creditPanelPrefab;
        [SerializeField] GameObject quitPanelPrefab;
        [Header("Modals")]
        [SerializeField] OptionsUI optionsUI;
        [SerializeField] CreditsUI creditUI;
        [SerializeField] QuitUI quitUI;


        void Awake()
        {
            playButton.onClick.AddListener(PlayButtonAction);
            optionButton.onClick.AddListener(OptionButtonAction);
            creditButton.onClick.AddListener(CreditButtonAction);
            quitButton.onClick.AddListener(QuitButtonAction);

            optionsUI = Instantiate(optionPanelPrefab, null).GetComponent<OptionsUI>();
            creditUI = Instantiate(creditPanelPrefab, null).GetComponent<CreditsUI>();
            quitUI = Instantiate(quitPanelPrefab, null).GetComponent<QuitUI>();

            optionsUI.gameObject.SetActive(false);
            creditUI.gameObject.SetActive(false);
            quitUI.gameObject.SetActive(false);
        }

        void PlayButtonAction()
        {
            //TODO: go to game scene
            GameServices.Instance.navigationManager.LoadScene(Scenes.Gameplay);
            GameServices.Instance.audioManager.PlayButtonClick();
        }

        void OptionButtonAction()
        {
            optionsUI.gameObject.SetActive(true);
            GameServices.Instance.audioManager.PlayButtonClick();
        }

        void CreditButtonAction()
        {
            creditUI.gameObject.SetActive(true);
            GameServices.Instance.audioManager.PlayButtonClick();
        }

        void QuitButtonAction()
        {
            quitUI.gameObject.SetActive(true);
            GameServices.Instance.audioManager.PlayButtonClick();
        }


    }


