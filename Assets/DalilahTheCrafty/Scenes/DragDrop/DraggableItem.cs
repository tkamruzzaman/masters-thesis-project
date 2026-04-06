using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] int itemIndex;
    [SerializeField] float snapDistance = 1.0f;
    [SerializeField] float dragSpeed = 20f;

    private Vector3 _startPosition;
    private Vector3 _dragOffset;
    private Camera _cam;
    private SpriteRenderer _sr;

    DropZone[] dropZones;

    private bool _isDone;

    void Awake()
    {
        _cam = Camera.main;
        _sr = GetComponent<SpriteRenderer>();
        _startPosition = transform.position;
    }

    private void Start()
    {
        dropZones = FindObjectsByType<DropZone>(FindObjectsSortMode.None);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isDone) return;
        _dragOffset = transform.position - ScreenToWorld(eventData.position);
        _sr.sortingOrder += 10;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isDone) return;
        transform.position = Vector3.Lerp(transform.position, ScreenToWorld(eventData.position) + _dragOffset, Time.deltaTime * dragSpeed);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isDone) return;
        _sr.sortingOrder -= 10;

        DropZone target = FindMatchingDropZone();
        //print("target:  " + target );

        if (target != null && target.TryAccept(this, itemIndex))
        {
            transform.position = target.transform.position;
            _isDone = true;
        }
        else
        {
            transform.position = _startPosition;
        }
    }
    
    private DropZone FindMatchingDropZone()
    {
        DropZone best = null;
        float bestDist = snapDistance;

        foreach (DropZone zone in dropZones)
        {
            if (zone.zoneIndex != itemIndex)
            {
                continue;
            }
            if (zone.IsOccupied)
            {
                continue;
            }

            float d = Vector3.Distance(transform.position, zone.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                best = zone;
            }
            //print("ZONE:::: " + zone);
        }

        return best;
    }

    private Vector3 ScreenToWorld(Vector2 screenPos)
    {
        Vector3 world = _cam.ScreenToWorldPoint(
            new Vector3(screenPos.x, screenPos.y, Mathf.Abs(_cam.transform.position.z)));
        world.z = 0f;
        return world;
    }

    public void ReturnToStart() => transform.position = _startPosition;
}
