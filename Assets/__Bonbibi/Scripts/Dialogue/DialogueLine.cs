    using UnityEngine;

    namespace Bonbibi
{
    [CreateAssetMenu(fileName = "New DialogueLine", menuName = "Bonbibi/Dialogue Line")]
    public class DialogueLine : ScriptableObject
    {
        [Header("Content")]
        public CharacterNames speaker;
        [Multiline(10)]
        public string text;
    }

    public enum CharacterNames
    {
        Narration = 0,
        Dhona,
        Mona,
        Dukhi,
        Mother,
        BonBibi,
        DokkhinRai,
        ShahJongoli,
    }
}