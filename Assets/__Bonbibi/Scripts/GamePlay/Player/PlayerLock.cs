using System;
using Unity.Cinemachine;
using UnityEngine;
using StarterAssets;

namespace Bonbibi
{
    public class PlayerLock : MonoBehaviour
    {
        [SerializeField] private ThirdPersonController thirdPersonController;

        [SerializeField] private StarterAssetsInputs starterAssetsInputs;

        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        private CharacterController _characterController;
        private SceneLoader  _sceneLoader;
        private DialogueManager _dialogueManager;
        
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            _sceneLoader = FindAnyObjectByType<SceneLoader>();
            if(_sceneLoader == null) print("_sceneLoader is null"); 
            _sceneLoader.OnRepositionPlayer += SceneLoaderOnOnRepositionPlayer;
            
            _dialogueManager = FindAnyObjectByType<DialogueManager>();
            if(_dialogueManager == null) print("_dialogueManager is null");
            _dialogueManager.OnChoiceOrSequenceStarted += DialogueManagerOnOnChoiceOrSequenceStarted;
            _dialogueManager.OnChoiceOrSequenceEnded += DialogueManagerOnOnChoiceOrSequenceEnded;
        }

        private void DialogueManagerOnOnChoiceOrSequenceStarted(object sender, EventArgs e) => Lock();
        
        private void DialogueManagerOnOnChoiceOrSequenceEnded(object sender, EventArgs e)=> Unlock();

        private void SceneLoaderOnOnRepositionPlayer(object sender, SceneLoader.OnRepositionPlayerEventArgs e)
        {
            TeleportTo(e.position, e.rotation);
        }

        public void Lock()
        {
            if (thirdPersonController)
            {
                thirdPersonController.enabled = false;
                thirdPersonController.DisableAnimations();
            }

            if (starterAssetsInputs)
            {
                starterAssetsInputs.enabled = false;
            }

            // Release cursor for choice buttons
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void Unlock()
        {
            if (thirdPersonController)
                thirdPersonController.enabled = true;

            if (starterAssetsInputs)
                starterAssetsInputs.enabled = true;

            // Recapture cursor for camera look
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void TeleportTo(Vector3 position, Quaternion rotation)
        {
            // Must disable controller before moving to avoid physics conflicts
            if (_characterController) _characterController.enabled = false;

            transform.position = position;
            transform.rotation = rotation;

            if (_characterController) _characterController.enabled = true;

            // Reset Cinemachine to avoid camera snap
            if (virtualCamera)
            {
                virtualCamera.OnTargetObjectWarped(
                    transform,
                    transform.position - transform.position
                );
            }
        }

        private void OnDestroy()
        {
            _sceneLoader.OnRepositionPlayer -= SceneLoaderOnOnRepositionPlayer;
            _dialogueManager.OnChoiceOrSequenceStarted -= DialogueManagerOnOnChoiceOrSequenceStarted;
            _dialogueManager.OnChoiceOrSequenceEnded -= DialogueManagerOnOnChoiceOrSequenceEnded;
        }
    }
}