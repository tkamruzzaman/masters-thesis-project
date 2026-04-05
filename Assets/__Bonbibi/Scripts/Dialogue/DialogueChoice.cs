using UnityEngine;

namespace Bonbibi
{
    [CreateAssetMenu(fileName = "New DialogueChoice", menuName = "Bonbibi/Dialogue Choice")]
    public class DialogueChoice : ScriptableObject
    {
        [Header("Prompt")]
        [TextArea(5, 15)]
        public string prompt;

        [Header("Options")]
        public ChoiceOption[] options;
    }

    [System.Serializable]
    public class ChoiceOption
    {
        [TextArea(5, 15)]
        public string label;

        public LeanTag lean;
        public int conscienceValue;

        [Tooltip("Optional. 'promisedMother' is the only flag in use.")]
        public string flagToSet;

        [Tooltip("Optional. Plays immediately after this choice is made.")]
        public DialogueSequence responseSequence;

        [Tooltip("Plays after responseSequence completes, or immediately if no response.")]
        public DialogueSequence nextSequence;
    }

    public enum LeanTag
    {
        Conscience,
        Neutral,
        Compliance
    }
}