using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotateItem : MonoBehaviour, IPointerClickHandler
{
    bool isRotating;
    public RotateMode rotateMode = RotateMode.LocalAxisAdd;
    public int rotationDirection =-1;
    private Vector3 rotationAmount;
    
    private void Awake()
    {
        rotationAmount = new Vector3(0, 0, rotationDirection * 90);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isRotating) return;
        isRotating = true;
        print("OnPointerClick");
        transform.DOLocalRotate(rotationAmount, 0.3f, rotateMode).OnComplete(() => { isRotating = false; });
    }
}
