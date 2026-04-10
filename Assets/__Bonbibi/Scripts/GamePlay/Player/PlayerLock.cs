using System;
using Unity.Cinemachine;
using UnityEngine;
using StarterAssets;
using UnityEngine.SceneManagement;

namespace Bonbibi
{
    public class PlayerLock : MonoBehaviour
    {
        [SerializeField] private ThirdPersonController thirdPersonController;

        [SerializeField] private StarterAssetsInputs starterAssetsInputs;

        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        
        [SerializeField] private GameObject spawnPoint;

        private CharacterController _characterController;
        private DialogueManager _dialogueManager;
        
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }
        
        private void OnEnable()=> SceneManager.sceneLoaded += OnSceneLoaded;
        private void OnDisable()=> SceneManager.sceneLoaded -= OnSceneLoaded;
    
        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
           TeleportToSpawnPoint();
        }

        private void Start()
        {
            _dialogueManager = FindAnyObjectByType<DialogueManager>();
            if(_dialogueManager == null) print("_dialogueManager is null");
            _dialogueManager.OnChoiceOrSequenceStarted += DialogueManagerOnOnChoiceOrSequenceStarted;
            _dialogueManager.OnChoiceOrSequenceEnded += DialogueManagerOnOnChoiceOrSequenceEnded;
        }

        private void DialogueManagerOnOnChoiceOrSequenceStarted(object sender, EventArgs e) => LockPlayerControls();
        
        private void DialogueManagerOnOnChoiceOrSequenceEnded(object sender, EventArgs e)=> UnlockPlayerControls();


        private void LockPlayerControls()
        {
            if (thirdPersonController)
            {
                thirdPersonController.enabled = false;
                thirdPersonController.DisableAnimations();
            }
            
            if (starterAssetsInputs) { starterAssetsInputs.enabled = false; }

            // Release cursor for choice buttons
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void UnlockPlayerControls()
        {
            if (thirdPersonController) { thirdPersonController.enabled = true; }

            if (starterAssetsInputs) { starterAssetsInputs.enabled = true; }

            // Recapture cursor for camera look
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void TeleportToSpawnPoint()
        {
            // Must disable controller before moving to avoid physics conflicts
            if (_characterController) _characterController.enabled = false;

            transform.position = spawnPoint.transform.position;
            transform.rotation = spawnPoint.transform.rotation;

            if (_characterController) _characterController.enabled = true;

            // Reset Cinemachine to avoid camera snap
            if (virtualCamera)
            {
                virtualCamera.OnTargetObjectWarped(transform, transform.position - transform.position);
            }
        }

        private void OnDestroy()
        {
            _dialogueManager.OnChoiceOrSequenceStarted -= DialogueManagerOnOnChoiceOrSequenceStarted;
            _dialogueManager.OnChoiceOrSequenceEnded -= DialogueManagerOnOnChoiceOrSequenceEnded;
        }
    }
}