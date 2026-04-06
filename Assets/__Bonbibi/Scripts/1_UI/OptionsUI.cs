using UnityEngine;
using UnityEngine.UI;

    public class OptionsUI : MonoBehaviour
    {
        [Header("Music")]
        [SerializeField] Toggle musicToggle;
        [Header("Sound")]
        [SerializeField] Toggle soundToggle;
        [Header("BackButton")]
        [SerializeField] Button backButton;
        [Header("Controls Button")]
        [SerializeField] Button controlsButton;

        [Header("Controls Panel")]
        [SerializeField] GameObject controlsPanel;
        [SerializeField] Button controlsBackButton;

        void Awake()
        {
            backButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                GameServices.Instance.audioManager.PlayButtonClick();
            });

            musicToggle.onValueChanged.AddListener(MusicToggleAction);
            soundToggle.onValueChanged.AddListener(SoundToggleAction);

            controlsButton.onClick.AddListener(ControlButtonAction);

            CheckValues();

            controlsBackButton.onClick.AddListener(() =>
            {
                controlsPanel.SetActive(false);
                GameServices.Instance.audioManager.PlayButtonClick();
            });
            controlsPanel.SetActive(false);
        }

        void OnEnable()
        {
            CheckValues();
        }

        void CheckValues()
        {
            if (GameServices.Instance.audioManager == null) { return; }
            musicToggle.isOn = GameServices.Instance.audioManager.IsMusicOn;
            soundToggle.isOn = GameServices.Instance.audioManager.IsSFXOn;
        }

        private void MusicToggleAction(bool arg0)
        {
            print("Music Toggled");
            //GlobalEvents.SendMusicToggle(arg0);
        }

        private void SoundToggleAction(bool arg0)
        {
            print("SoundToggled   " + arg0);
            //GlobalEvents.SendSFXToggle(arg0);
        }

        private void ControlButtonAction()
        {
            controlsPanel.SetActive(true);
            GameServices.Instance.audioManager.PlayButtonClick();
        }
    }

