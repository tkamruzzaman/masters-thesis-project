using UnityEngine;

namespace Bonbibi
{
    [CreateAssetMenu(fileName = "New DialogueSequence", menuName = "Bonbibi/Dialogue Sequence")]
    public class DialogueSequence : ScriptableObject
    {
        [Header("Lines")]
        public DialogueLine[] lines;

        [Header("On Complete")]
        [Tooltip("If true, loads the next scene when this sequence finishes.")]
        public bool loadNextSceneOnComplete = false;
    }
}