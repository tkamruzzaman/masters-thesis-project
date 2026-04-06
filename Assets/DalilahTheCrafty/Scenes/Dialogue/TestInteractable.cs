using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] bool isInteractable = true;
    [SerializeField] SpriteRenderer spriteRenderer;
    public void Interact()
    {
        if (!CanInteract()) return;
        print("Interacted >>>>>>>>>> ))");
        spriteRenderer.color =  Color.green;
        isInteractable = false;
    }

    public bool CanInteract()
    {
        return isInteractable;
    }
}
