using Bonbibi;
using Sirenix.OdinInspector;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class GameServices : MonoBehaviour
{
   public static GameServices Instance { get; private set; }

   public AudioManager audioManager;
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
      if (audioManager == null) audioManager = FindAnyObjectByType<AudioManager>();
      if (effectsManager == null) effectsManager = FindAnyObjectByType<EffectsManager>();
      if (navigationManager == null) navigationManager = FindAnyObjectByType<NavigationManager>();
      if (uiManager == null) uiManager = FindAnyObjectByType<UIManager>();
   }

   [Button(ButtonSizes.Large)]
   public void ResetGame()
   {
      GameState.Reset();
   }
}
