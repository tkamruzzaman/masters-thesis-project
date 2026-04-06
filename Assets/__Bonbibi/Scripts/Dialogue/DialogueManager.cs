using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bonbibi
{
    public class DialogueManager : MonoBehaviour
    {
        public event EventHandler OnChoiceOrSequenceStarted; 
        public event EventHandler OnChoiceOrSequenceEnded; 
        public bool IsPlaying { get; private set; } = false;

        private DialogueSequence _currentSequence;
        private int _currentLineIndex = 0;
        private bool _waitingForInput = false;
        private bool _waitingForChoice = false;

        public void PlaySequence(DialogueSequence dialogueSequence)
        {
            if (!dialogueSequence || dialogueSequence.lines.Length == 0)
            {
                Debug.LogWarning("DialogueManager: PlaySequence called with null or empty sequence.");
                return;
            }

            _currentSequence = dialogueSequence;
            _currentLineIndex = 0;
            IsPlaying = true;

            OnChoiceOrSequenceStarted?.Invoke(this, EventArgs.Empty);
            ShowCurrentLine();
        }

        private void ShowCurrentLine()
        {
            if (_currentLineIndex >= _currentSequence.lines.Length)
            {
                EndSequence();
                return;
            }

            DialogueLine dialogueLine = _currentSequence.lines[_currentLineIndex];

            switch (dialogueLine.speaker)
            {
                case CharacterNames.Narration:
                    DialogueUI.Instance.ShowNarration(dialogueLine.text);
                    break;
                default:
                    DialogueUI.Instance.ShowDialogueLine(dialogueLine.speaker.ToString(), dialogueLine.text);
                    break;
            }

            _waitingForInput = true;
        }

        private void EndSequence()
        {
            IsPlaying = false;
            _waitingForInput = false;
            DialogueUI.Instance.HideAll();
            OnChoiceOrSequenceEnded?.Invoke(this, EventArgs.Empty);

            // Cache before clearing so the event fires cleanly
            DialogueSequence completed = _currentSequence;
            _currentSequence = null;
            
            if (completed.loadNextSceneOnComplete)
            {
                SceneLoader.Instance.LoadNext();
            }
        }

        public void PlayChoice(DialogueChoice dialogueChoice)
        {
            if (dialogueChoice == null || dialogueChoice.options.Length == 0)
            {
                Debug.LogWarning("DialogueManager: PlayChoice called with null or empty choice.");
                return;
            }

            IsPlaying = true;
            _waitingForChoice = true;

            OnChoiceOrSequenceStarted?.Invoke(this, EventArgs.Empty);

            DialogueUI.Instance.ShowChoices(dialogueChoice.prompt, dialogueChoice.options,
                (int index) =>
                {
                    OnChoiceSelected(dialogueChoice.options[index]);
                });
        }

        private void OnChoiceSelected(ChoiceOption choiceOption)
        {
            _waitingForChoice = false;

            // Write to GameState
            if (choiceOption.conscienceValue != 0)
            {
                GameState.AddConscience(choiceOption.conscienceValue);
            }

            if (!string.IsNullOrEmpty(choiceOption.flagToSet))
            {
                HandleFlag(choiceOption.flagToSet);
            }

            DialogueUI.Instance.HideAll();

            // Play response first if one exists, then continue to nextSequence
            if (choiceOption.responseSequence != null)
            {
                StartCoroutine(PlaySequenceThenContinue(choiceOption.responseSequence, choiceOption.nextSequence));
            }
            else if (choiceOption.nextSequence != null)
            {
                PlaySequence(choiceOption.nextSequence);
            }
            else
            {
                FinishChoice();
            }
        }
        private IEnumerator PlaySequenceThenContinue(DialogueSequence responseSequence, DialogueSequence nextSequence)
        {
            PlaySequence(responseSequence);

            // Wait until the response sequence finishes
            yield return new WaitUntil(() => !IsPlaying);

            if (nextSequence)
            {
                PlaySequence(nextSequence);
            }
            else
            {
                FinishChoice();
            }
        }
        private void FinishChoice()
        {
            IsPlaying = false;
            OnChoiceOrSequenceEnded?.Invoke(this, EventArgs.Empty);
        }
        

        private void HandleFlag(string flag)
        {
            switch (flag)
            {
                case "promisedMother":
                    GameState.SetPromisedMother(true);
                    break;
                default:
                    Debug.LogWarning($"DialogueManager: Unknown flag '{flag}'");
                    break;
            }
        }

        private void Update()
        {
            if (!_waitingForInput || _waitingForChoice) { return; }

            if (!Keyboard.current.spaceKey.wasPressedThisFrame &&
                !Mouse.current.leftButton.wasPressedThisFrame) { return; }

            _waitingForInput = false;
            _currentLineIndex++;
            ShowCurrentLine();
        }
    }
}