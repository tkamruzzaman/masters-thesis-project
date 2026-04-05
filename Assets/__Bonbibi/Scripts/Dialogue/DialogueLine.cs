    using UnityEngine;
    using Sirenix;
    using Sirenix.Serialization;

    namespace Bonbibi
{
    [CreateAssetMenu(fileName = "New DialogueLine", menuName = "Bonbibi/Dialogue Line")]
    public class DialogueLine : ScriptableObject
    {
        [Header("Content")]
        public bool isNarration;
        public CharacterNames speaker;
        //[TextArea(5, 15)]
        [Multiline(10)]
        public string text;
    }

    public enum CharacterNames
    {
        None = 0,
        Dhona,
        Mona,
        Dukhi,
        Mother,
        BonBibi,
        DokkhinRai,
        ShahJongoli,
    }
}