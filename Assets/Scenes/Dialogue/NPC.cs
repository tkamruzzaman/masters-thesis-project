using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
   public NPCDialogue dialogueData;
private DialogueController dialogueController;

   private int dialogueIndex;
   private bool isTyping, isDialogueActive;

   private void Start()
   {
      dialogueController = DialogueController.Instance;
   }

   public bool CanInteract()
   {
      return !isDialogueActive;
   }

   public void Interact()
   {

      if (dialogueData == null /*|| !isDialogueActive*/)
         return;
      print("Interacted >>>>>>>>>> ))");
      if (isDialogueActive)
      {
         NextLine();
      }
      else
      {
         StartDialogue();
      }
   }

   void StartDialogue()
   {
      isDialogueActive = true;
      dialogueIndex = 0;
      
      dialogueController.SetNPCInfo(dialogueData.npcName, dialogueData.npcSprite);
      dialogueController.ShowDialogueUI(true);

      StartCoroutine(TypeLine());
   }

   void NextLine()
   {
      if (isTyping)
      {
         //Skip typing animation and show the full line
         StopAllCoroutines();

         dialogueController.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);
         isTyping = false;
      }
      else if (++dialogueIndex < dialogueData.dialogueLines.Length)
      {
         //If another line, type next line 
         StartCoroutine(TypeLine());
      }
      else
      {
         EndDialogue();
      }
   }

   IEnumerator TypeLine()
   {
      isTyping = true;
      dialogueController.SetDialogueText("");
      foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
      {
         dialogueController.SetDialogueText(dialogueController.dialogueText.text += letter);
         //playSoundEffect
         yield return new WaitForSeconds(dialogueData.typingSpeed);
      }

      isTyping = false;
      if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
      {
         yield return new WaitForSeconds(dialogueData.autoProgressDelay);
         NextLine();

      }
   }

   public void EndDialogue()
   {
      StopAllCoroutines();
      isDialogueActive = false;
    dialogueController.SetDialogueText("");
    dialogueController.ShowDialogueUI(false);
   }
}
