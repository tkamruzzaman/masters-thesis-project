using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Attach to any sprite you want the player to drag.
/// Requires:
///   - A Collider2D on this GameObject (for physics raycasting)
///   - A Physics2DRaycaster component on the Camera
///   - An EventSystem in the scene (added automatically with the New Input System)
/// </summary>
public class DraggableItem : MonoBehaviour,
    IPointerDownHandler,
    IDragHandler,
    IPointerUpHandler
{
    [Header("Settings")]
    [Tooltip("How close (world units) the item must be to a drop zone to snap")]
    public float snapDistance = 1.5f;

    [Tooltip("Smoothing speed while dragging (higher = snappier follow)")]
    public float dragSpeed = 20f;

    // ── Private state ─────────────────────────────────────────────────────────
    private Vector3        _startPosition;
    private Vector3        _dragOffset;    // world-space offset so sprite doesn't jump on grab
    private Camera         _cam;
    private SpriteRenderer _sr;

    private Color _defaultColor;
    [SerializeField] private Color hoverColor = new Color(1f, 1f, 0.4f);   // yellow – dragging
    [SerializeField] private Color snapColor  = new Color(0.4f, 1f, 0.4f); // green  – near zone

    // ── Unity lifecycle ───────────────────────────────────────────────────────

    void Awake()
    {
        _cam           = Camera.main;
        _sr            = GetComponent<SpriteRenderer>();
        _defaultColor  = _sr ? _sr.color : Color.white;
        _startPosition = transform.position;
    }

    // ── IPointer handlers ─────────────────────────────────────────────────────

    /// <summary>Fires when the pointer is pressed down on this object.</summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        // Store offset so the sprite centre doesn't jump to the cursor
        _dragOffset = transform.position - ScreenToWorld(eventData.position);

        // Render on top while dragging
        if (_sr) _sr.sortingOrder += 10;
    }

    /// <summary>Fires every frame the pointer moves while held down.</summary>
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 target = ScreenToWorld(eventData.position) + _dragOffset;

        // Smooth follow
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * dragSpeed);

        // Tint based on proximity to nearest free drop zone
        if (_sr)
            _sr.color = FindNearestDropZone() != null ? snapColor : hoverColor;
    }

    /// <summary>Fires when the pointer is released.</summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        // Restore visuals
        if (_sr)
        {
            _sr.color        = _defaultColor;
            _sr.sortingOrder -= 10;
        }

        DropZone nearest = FindNearestDropZone();

        if (nearest != null && nearest.TryAccept(this))
        {
            // Snap exactly to zone centre
            transform.position = nearest.transform.position;
        }
        else
        {
            // Return home
            transform.position = _startPosition;
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    /// <summary>Converts a screen-space point to world space at z = 0.</summary>
    private Vector3 ScreenToWorld(Vector2 screenPos)
    {
        Vector3 world = _cam.ScreenToWorldPoint(
            new Vector3(screenPos.x, screenPos.y, Mathf.Abs(_cam.transform.position.z)));
        world.z = 0f;
        return world;
    }

    /// <summary>Returns the nearest unoccupied DropZone within snapDistance, or null.</summary>
    private DropZone FindNearestDropZone()
    {
        DropZone[] zones    = FindObjectsByType<DropZone>(FindObjectsSortMode.None);
        DropZone   best     = null;
        float      bestDist = snapDistance;

        foreach (DropZone zone in zones)
        {
            if (zone.IsOccupied) continue;
            float d = Vector3.Distance(transform.position, zone.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                best     = zone;
            }
        }
        return best;
    }

    /// <summary>Called by DropZone.Clear() to return the item home.</summary>
    public void ReturnToStart() => transform.position = _startPosition;
}
