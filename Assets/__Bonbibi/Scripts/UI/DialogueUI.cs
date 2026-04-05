using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Bonbibi
{
    public class DialogueUI : MonoBehaviour
    {
        [Header("Fade Panel")]
        [SerializeField]  private CanvasGroup fadePanel;
        [Header("Narration Panel")] [SerializeField]
        private GameObject narrationPanel;

        [SerializeField] private TextMeshProUGUI narrationText;

        [Header("Dialogue Panel")] [SerializeField]
        private GameObject dialoguePanel;

        [SerializeField] private TextMeshProUGUI speakerName;
        [SerializeField] private TextMeshProUGUI dialogueText;

        [Header("Choice Panel")] [SerializeField]
        private GameObject choicePanel;

        [SerializeField] private TextMeshProUGUI promptText;
        [SerializeField] private Button[] choiceButtons;
        [SerializeField] private TextMeshProUGUI[] choiceButtonTexts;

        public static DialogueUI Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            HideAll();
        }

        // ─────────────────────────────────────────
        // Dialogue Panel
        // ─────────────────────────────────────────

        public void ShowDialogueLine(string speaker, string text)
        {
            HideAll();
            speakerName.text = speaker;
            dialogueText.text = text;
            dialoguePanel.SetActive(true);
        }

        // ─────────────────────────────────────────
        // Narration Panel
        // ─────────────────────────────────────────

        public void ShowNarration(string text)
        {
            HideAll();
            narrationText.text = text;
            narrationPanel.SetActive(true);
        }

        // ─────────────────────────────────────────
        // Choice Panel
        // ─────────────────────────────────────────

        public void ShowChoices(string prompt, ChoiceOption[] options, Action<int> onChosen)
        {
            HideAll();
            promptText.text = prompt;

            for (int i = 0; i < choiceButtons.Length; i++)
            {
                if (i < options.Length)
                {
                    int index = i; // capture for lambda
                    choiceButtons[i].gameObject.SetActive(true);
                    choiceButtonTexts[i].text = options[i].label;
                    choiceButtons[i].onClick.RemoveAllListeners();
                    choiceButtons[i].onClick.AddListener(() =>
                    {
                        onChosen(index);
                    });
                }
                else
                {
                    choiceButtons[i].gameObject.SetActive(false);
                }
            }

            choicePanel.SetActive(true);
        }

        public void HideAll()
        {
            narrationPanel.SetActive(false);
            dialoguePanel.SetActive(false);
            choicePanel.SetActive(false);
        }

        public CanvasGroup GetFadePanel() => fadePanel;
    }
}