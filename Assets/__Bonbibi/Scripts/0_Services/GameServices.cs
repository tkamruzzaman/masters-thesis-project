using UnityEngine;

public class GameServices : MonoBehaviour
{
   public static GameServices Instance { get; private set; }

   public AudioManager audioManager;
   //public DialogueManager dialogueManager;
   public EffectsManager effectsManager;
   public NavigationManager navigationManager;
   public UIManager uiManager;

   private void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
         DontDestroyOnLoad(gameObject);
      }
      else
      {
         Destroy(gameObject);
      }

      AssignReferences();
   }

   private void AssignReferences()
   {
      audioManager = FindFirstObjectByType<AudioManager>();
      //dialogueManager = FindFirstObjectByType<DialogueManager>();
      effectsManager = FindFirstObjectByType<EffectsManager>();
      navigationManager = FindFirstObjectByType<NavigationManager>();
      uiManager = FindFirstObjectByType<UIManager>();
   }
}
