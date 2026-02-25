using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Mark a GameObject as a valid drop target.
/// Optionally restrict which items are accepted via a tag filter.
/// </summary>
public class DropZone : MonoBehaviour
{
    [Header("Filtering (optional)")]
    [Tooltip("Only accept draggable items with this tag. Leave empty to accept all.")]
    public string acceptTag = "";

    [Header("Visual Feedback")]
    [SerializeField] private Color idleColor    = new Color(1f, 1f, 1f, 0.4f);
    [SerializeField] private Color correctColor = new Color(0.3f, 1f, 0.3f, 0.8f);
    [SerializeField] private Color wrongColor   = new Color(1f, 0.3f, 0.3f, 0.8f);

    [Header("Events")]
    public UnityEvent<DraggableItem> onItemDropped;   // fires when an item is accepted
    public UnityEvent               onItemRemoved;    // fires when zone is cleared

    // ── State ────────────────────────────────────────────────────────────────

    public bool IsOccupied => _heldItem != null;
    private DraggableItem _heldItem;
    private SpriteRenderer _sr;

    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        SetColor(idleColor);
    }

    // ── Public API ───────────────────────────────────────────────────────────

    /// <summary>
    /// Called by DraggableItem.OnMouseUp – returns true if the item was accepted.
    /// </summary>
    public bool TryAccept(DraggableItem item)
    {
        // Tag filter
        if (!string.IsNullOrEmpty(acceptTag) && !item.CompareTag(acceptTag))
        {
            FlashColor(wrongColor);
            return false;
        }

        // Already occupied
        if (IsOccupied)
        {
            FlashColor(wrongColor);
            return false;
        }

        _heldItem = item;
        SetColor(correctColor);
        onItemDropped?.Invoke(item);
        Debug.Log($"[DropZone] '{name}' accepted '{item.name}'");
        return true;
    }

    /// <summary>Remove the current item and reset the zone.</summary>
    public void Clear()
    {
        if (_heldItem != null)
        {
            _heldItem.ReturnToStart();
            _heldItem = null;
        }
        SetColor(idleColor);
        onItemRemoved?.Invoke();
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    void SetColor(Color c)
    {
        if (_sr) _sr.color = c;
    }

    void FlashColor(Color c)
    {
        // Simple coroutine flash
        StopAllCoroutines();
        StartCoroutine(FlashRoutine(c));
    }

    System.Collections.IEnumerator FlashRoutine(Color c)
    {
        SetColor(c);
        yield return new WaitForSeconds(0.35f);
        SetColor(IsOccupied ? correctColor : idleColor);
    }
}
