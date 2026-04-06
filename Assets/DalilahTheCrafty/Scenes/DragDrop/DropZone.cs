using System;
using UnityEngine;

public class DropZone : MonoBehaviour
{
    public int zoneIndex;
    
    public Action<DraggableItem> OnItemDropped;
    public Action<DraggableItem> OnItemRemoved;

    public bool IsOccupied => _heldItem != null;
    private DraggableItem _heldItem;

    public bool TryAccept(DraggableItem item, int itemIndex)
    {
        if (itemIndex != zoneIndex || IsOccupied)
        {
            ShowWrongFeedback();
            return false;
        }

        _heldItem = item;
        ShowCorrectFeedback();
        OnItemDropped?.Invoke(item);
        //Debug.Log($"[DropZone] Zone {zoneIndex} ('{name}') accepted item {itemIndex} ('{item.name}')");
        return true;
    }

    public void Clear()
    {
        if (_heldItem == null) return;
        _heldItem.ReturnToStart();
        OnItemRemoved?.Invoke(_heldItem);
        _heldItem = null;
    }

    private void ShowWrongFeedback()
    {
       // TODO: show wrong placement feedback
    }
    
    void ShowCorrectFeedback()
    {
        // TODO: show correct placement feedback
    }
}
