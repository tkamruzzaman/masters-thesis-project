using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] CharacterController2D characterController;
    [SerializeField] Rigidbody2D  playerRigidbody;
    [SerializeField] Collider2D playerCollider;

    [SerializeField] float moveSpeed = 5f;
    float horizontalMove = 0f;
    
    //[SerializeField] InputActionAsset inputActionAsset;
    //InputAction moveAction;
    private Vector2 moveInput;

    private bool isJumping;
    
    private void Awake()
    {
        characterController = GetComponent<CharacterController2D>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        
        //moveAction = InputSystem.actions.FindAction("Move");
    }

    private void OnEnable()
    {
        //inputActionAsset.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        //inputActionAsset.FindActionMap("Player").Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        horizontalMove = moveInput.x;
        if (context.performed)
        {
            //Debug.Log($"{horizontalMove} MOVED!");
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isJumping = true;
            //print("DFDFHGKDHFGHKDH " + gameObject.name);
        }    
    }
    
    private void Update()
    {
        //moveAmount = moveAction.ReadValue<Vector2>();
        //horizontalMove = moveAmount.x * moveSpeed;
        //print(horizontalMove);
    }

    private void FixedUpdate()
    {
        //Vector2 move = moveInput * moveSpeed * Time.fixedDeltaTime;
        //playerRigidbody.MovePosition(playerRigidbody.position + move);
        characterController.Move(horizontalMove * moveSpeed * Time.fixedDeltaTime, false, isJumping);
        isJumping = false;
    }
}
