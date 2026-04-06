using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "ScriptableObjects/NPCDialogue")]
public class NPCDialogue : ScriptableObject
{
   public string npcName;
   public Sprite npcSprite;
   public string[]  dialogueLines;
   public bool[] autoProgressLines;
   public bool[] endDialogueLines;
   public float autoProgressDelay = 1.5f;
   public float typingSpeed = 0.05f;
   public AudioClip voiceSound;
   public float voicePitch = 1;
   
   public DialogueChoice[] choices;
}

[System.Serializable]
public class DialogueChoice
{
   public int dialogueLineIndex; //dialogue line where choice appear
   public string[] choices; // player response options
   public int[] nextDialogueLineIndexes; //where choice leads
}
