using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

namespace Bonbibi
{
    public class DialogueUI : MonoBehaviour
    {
        [Header("Fade Panel")]
        [SerializeField] private CanvasGroup fadePanel;

        [Header("Narration Panel")]
        [SerializeField] private GameObject narrationPanel;
        [SerializeField] private TextMeshProUGUI narrationText;

        [Header("Dialogue Panel")]
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private TextMeshProUGUI speakerName;
        [SerializeField] private TextMeshProUGUI dialogueText;

        [Header("Choice Panel")]
        [SerializeField] private GameObject choicePanel;
        [SerializeField] private TextMeshProUGUI promptText;
        [SerializeField] private Button[] choiceButtons;
        [SerializeField] private TextMeshProUGUI[] choiceButtonTexts;

        [Header("Typewriter Settings")]
        [SerializeField] private float charDelay = 0.03f;
        [Range(0f, 1f)]
        [SerializeField] private float typeVolume = 0.4f;
        [Tooltip("Play sound every N characters. 1 = every character, 2 = every other, etc.")]
        [SerializeField] private int playEveryNChars = 1;
        [Tooltip("Skip sound for spaces and punctuation.")]
        [SerializeField] private bool skipSilentChars = true;

        public static DialogueUI Instance { get; private set; }
        public bool IsTyping { get; private set; } = false;

        private Coroutine _typewriterCoroutine;
        private string _currentFullText = string.Empty;
        private TextMeshProUGUI _currentTarget;
        private int _charCount = 0;

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
            dialogueText.text = string.Empty;
            dialoguePanel.SetActive(true);
            StartTypewriter(dialogueText, text);
        }

        // ─────────────────────────────────────────
        // Narration Panel
        // ─────────────────────────────────────────

        public void ShowNarration(string text)
        {
            HideAll();
            narrationText.text = string.Empty;
            narrationPanel.SetActive(true);
            StartTypewriter(narrationText, text);
        }

        // ─────────────────────────────────────────
        // Typewriter
        // ─────────────────────────────────────────

        private void StartTypewriter(TextMeshProUGUI target, string fullText)
        {
            _currentFullText = fullText;
            _currentTarget = target;
            _charCount = 0;

            if (_typewriterCoroutine != null)
                StopCoroutine(_typewriterCoroutine);

            _typewriterCoroutine = StartCoroutine(TypewriterRoutine(target, fullText));
        }

        private IEnumerator TypewriterRoutine(TextMeshProUGUI target, string fullText)
        {
            IsTyping = true;
            target.text = string.Empty;

            foreach (char c in fullText)
            {
                target.text += c;
                _charCount++;

                PlayTypeSound(c);

                yield return new WaitForSeconds(charDelay);
            }

            IsTyping = false;
            _typewriterCoroutine = null;
        }

        private void PlayTypeSound(char c)
        {
            // Optionally skip spaces, punctuation, newlines
            if (skipSilentChars && (c == ' ' || c == '\n' || char.IsPunctuation(c))) return;

            // Play every N characters
            if (_charCount % playEveryNChars != 0) return;

            GameServices.Instance.audioManager.PlayTypingSound(typeVolume);
        }

        public void CompleteTypewriter()
        {
            if (_typewriterCoroutine != null)
            {
                StopCoroutine(_typewriterCoroutine);
                _typewriterCoroutine = null;
            }

            if (_currentTarget != null)
                _currentTarget.text = _currentFullText;

            IsTyping = false;
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
                    int index = i;
                    choiceButtons[i].gameObject.SetActive(true);
                    choiceButtonTexts[i].text = options[i].label;
                    choiceButtons[i].onClick.RemoveAllListeners();
                    choiceButtons[i].onClick.AddListener(() => onChosen(index));
                }
                else
                {
                    choiceButtons[i].gameObject.SetActive(false);
                }
            }

            choicePanel.SetActive(true);
        }

        // ─────────────────────────────────────────
        // Hide
        // ─────────────────────────────────────────

        public void HideAll()
        {
            if (_typewriterCoroutine != null)
            {
                StopCoroutine(_typewriterCoroutine);
                _typewriterCoroutine = null;
            }

            IsTyping = false;
            _currentFullText = string.Empty;
            _currentTarget = null;

            narrationPanel.SetActive(false);
            dialoguePanel.SetActive(false);
            choicePanel.SetActive(false);
        }

        public CanvasGroup GetFadePanel() => fadePanel;
    }
}