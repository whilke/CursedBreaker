using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragClickHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public UnityEvent OnClick;
    public UnityEvent OnClickEnd;

    private Button button;

    private void Awake()
    {
        this.button = this.GetComponent<Button>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!this.button.interactable) return;
        this.OnClick?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!this.button.interactable) return;
        this.OnClickEnd?.Invoke();
    }
}
