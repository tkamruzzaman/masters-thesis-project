using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bonbibi
{
    public class DialogueManager : MonoBehaviour
    {
        //public static DialogueManager Instance { get; private set; }
        
        public event EventHandler OnChoiceOrSequenceStarted; 
        public event EventHandler OnChoiceOrSequenceEnded; 
        public bool IsPlaying { get; private set; } = false;

        private DialogueSequence _currentSequence;
        private int _currentLineIndex = 0;
        private bool _waitingForInput = false;
        private bool _waitingForChoice = false;
        
        private void Awake()
        {
            //Instance = this;
        }

        public void PlaySequence(DialogueSequence sequence)
        {
            if (!sequence || sequence.lines.Length == 0)
            {
                Debug.LogWarning("DialogueManager: PlaySequence called with null or empty sequence.");
                return;
            }

            _currentSequence = sequence;
            _currentLineIndex = 0;
            IsPlaying = true;

            OnChoiceOrSequenceStarted?.Invoke(this, EventArgs.Empty);
            //PlayerLock.Instance?.Lock();
            ShowCurrentLine();
        }

        private void ShowCurrentLine()
        {
            if (_currentLineIndex >= _currentSequence.lines.Length)
            {
                EndSequence();
                return;
            }

            DialogueLine line = _currentSequence.lines[_currentLineIndex];

            if (line.isNarration)
                DialogueUI.Instance.ShowNarration(line.text);
            else
                DialogueUI.Instance.ShowDialogueLine(line.speaker.ToString(), line.text);

            _waitingForInput = true;
        }

        private void EndSequence()
        {
            IsPlaying = false;
            _waitingForInput = false;
            DialogueUI.Instance.HideAll();
            OnChoiceOrSequenceEnded?.Invoke(this, EventArgs.Empty);
            //PlayerLock.Instance?.Unlock();

            // Cache before clearing so the event fires cleanly
            DialogueSequence completed = _currentSequence;
            _currentSequence = null;
            
            if (completed.loadNextSceneOnComplete)
                SceneLoader.Instance.LoadNext();
        }

        public void PlayChoice(DialogueChoice choice)
        {
            if (choice == null || choice.options.Length == 0)
            {
                Debug.LogWarning("DialogueManager: PlayChoice called with null or empty choice.");
                return;
            }

            IsPlaying = true;
            _waitingForChoice = true;

            OnChoiceOrSequenceStarted?.Invoke(this, EventArgs.Empty);
            //PlayerLock.Instance?.Lock();

            DialogueUI.Instance.ShowChoices(choice.prompt, choice.options,
                (int index) => { OnChoiceSelected(choice.options[index]); });
        }

        private void OnChoiceSelected(ChoiceOption option)
        {
            _waitingForChoice = false;

            // Write to GameState
            if (option.conscienceValue != 0)
                GameState.AddConscience(option.conscienceValue);

            if (!string.IsNullOrEmpty(option.flagToSet))
                HandleFlag(option.flagToSet);

            DialogueUI.Instance.HideAll();

            // Play response first if one exists, then continue to nextSequence
            if (option.responseSequence != null)
            {
                StartCoroutine(PlaySequenceThenContinue(option.responseSequence, option.nextSequence));
            }
            else if (option.nextSequence != null)
            {
                PlaySequence(option.nextSequence);
            }
            else
            {
                FinishChoice();
            }
        }
        private IEnumerator PlaySequenceThenContinue(DialogueSequence response, DialogueSequence next)
        {
            PlaySequence(response);

            // Wait until the response sequence finishes
            yield return new WaitUntil(() => !IsPlaying);

            if (next)
                PlaySequence(next);
            else
                FinishChoice();
        }
        private void FinishChoice()
        {
            IsPlaying = false;
            OnChoiceOrSequenceEnded?.Invoke(this, EventArgs.Empty);
            //PlayerLock.Instance?.Unlock();
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
            if (!_waitingForInput || _waitingForChoice) return;
            if (!Keyboard.current.spaceKey.wasPressedThisFrame &&
                !Mouse.current.leftButton.wasPressedThisFrame) return;
            _waitingForInput = false;
            _currentLineIndex++;
            ShowCurrentLine();
        }
    }
}