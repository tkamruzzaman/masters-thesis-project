using UnityEngine;
using UnityEngine.Serialization;

namespace Bonbibi
{
    [CreateAssetMenu(fileName = "New DialogueSequence", menuName = "Bonbibi/Dialogue Sequence")]
    public class DialogueSequence : ScriptableObject
    {
        [Header("Lines")] public DialogueLine[] lines;

        [FormerlySerializedAs("loadNextSceneOnComplete")] [Header("On Complete")]
        public bool loadChapterSelectOnComplete = false;
    }
}