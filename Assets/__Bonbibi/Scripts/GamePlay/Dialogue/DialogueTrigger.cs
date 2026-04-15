using UnityEngine;

namespace Bonbibi
{
    public class DialogueTrigger : MonoBehaviour
    {
        [Header("Content")] [Tooltip("Assign a sequence for linear dialogue. Leave null if using a choice.")]
        public DialogueSequence sequence;

        [Tooltip("Assign a choice for branching dialogue. Leave null if using a sequence.")]
        public DialogueChoice choice;

        [Header("Behaviour")] 
        [Tooltip("If true, this trigger fires once and then disables itself.")]
        public bool triggerOnce = true;
        [Tooltip("Optional. Only fires if this flag is true in GameState.")]
        public string requiresFlag;
        [Tooltip("Optional. Only fires if this flag is FALSE in GameState.")]
        public string requiresFlagFalse;
        [Tooltip("Optional. Only fires if conscience score is at or above this value. Set to -1 to ignore.")]
        public int requiresMinScore = -1;
        [Tooltip("Optional. Only fires if conscience score is at or below this value. Set to -1 to ignore.")]
        public int requiresMaxScore = -1;
        
        [Header("Sequence")]
        [Tooltip("The next trigger to enable after this dialogue completes. Leave null if none.")]
        public DialogueTrigger nextTrigger;
        public TriggerGroupEnabler nextTriggerGroup;
        [Tooltip("Optional. Disables this GameObject when this trigger completes.")]
        public TriggerGroupEnabler triggerGroupToDisable;
        
        private bool _hasTriggered = false;
        private DialogueManager _dialogueManager;

        private void Start()
        {
            _dialogueManager = FindAnyObjectByType<DialogueManager>();
            if (_dialogueManager == null) Debug.LogWarning("_dialogueManager is null");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (_hasTriggered && triggerOnce) return;
            if (_dialogueManager.IsPlaying) return;
            if (!PassesConditions()) return;

            if (triggerOnce) _hasTriggered = true;

            FireDialogueTrigger();
        }

        private void FireDialogueTrigger()
        {
            if (choice != null)
            {
                _dialogueManager.PlayChoice(choice);
            }
            else if (sequence != null)
            {
                _dialogueManager.PlaySequence(sequence);
            }
            else
            {
                Debug.LogWarning($"DialogueTrigger on {gameObject.name} has no sequence or choice assigned.");
                return;
            }

            // Subscribe to be notified when this dialogue finishes
            _dialogueManager.OnChoiceOrSequenceEnded += OnDialogueEnded;
        }

        private void OnDialogueEnded(object sender, System.EventArgs e)
        {
            _dialogueManager.OnChoiceOrSequenceEnded -= OnDialogueEnded;

            if (nextTrigger)
            {
                nextTrigger.gameObject.SetActive(true);
            }

            if (nextTriggerGroup)
            {
                nextTriggerGroup.EnableAll();
            }

            if (triggerGroupToDisable)
            {
                triggerGroupToDisable.gameObject.SetActive(false);
            }

            gameObject.SetActive(false);
        }

        private bool PassesConditions()
        {
            // Flag check
            if (!string.IsNullOrEmpty(requiresFlag) && !EvaluateFlag(requiresFlag)) return false;
            //test
            if (!string.IsNullOrEmpty(requiresFlagFalse) && EvaluateFlag(requiresFlagFalse)) return false;
            //test
            // Min score check
            if (requiresMinScore >= 0 && GameState.ConscienceScore < requiresMinScore) return false;
            // Max score check
            if (requiresMaxScore >= 0 && GameState.ConscienceScore > requiresMaxScore) return false;

            return true;
        }

        private bool EvaluateFlag(string flag)
        {
            switch (flag)
            {
                case "promisedMother": return GameState.PromisedMother;
                default:
                    Debug.LogWarning($"DialogueTrigger: Unknown flag '{flag}'");
                    return false;
            }
        }

        public void ResetTrigger() => _hasTriggered = false;

    }
}