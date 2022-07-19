using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler
{    
    public Action<PointerEventData> OnLeftClickHandler = null;
    public Action<PointerEventData> OnLDbClickHandler = null;
    public Action<PointerEventData> OnRightClickHandler = null;
    public Action<PointerEventData> OnRDbClickHandler = null;
    public Action<PointerEventData> OnDragHandler = null;
    public Action<PointerEventData> OnEndDragHandler = null;

    long time = Environment.TickCount;

	public void OnPointerClick(PointerEventData eventData)
	{      
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (Environment.TickCount - time <= 200)
            {
                if (OnRDbClickHandler != null)
                    OnRDbClickHandler.Invoke(eventData);
            }
            else
            {
                if (OnRightClickHandler != null)
                    OnRightClickHandler.Invoke(eventData);
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (Environment.TickCount - time <= 200)
            {
                if (OnLDbClickHandler != null)
                    OnLDbClickHandler.Invoke(eventData);
            }
            else
            {
                if (OnLeftClickHandler != null)
                    OnLeftClickHandler.Invoke(eventData);
            }
        }

        time = Environment.TickCount;
    }

	public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag");
		if (OnDragHandler != null)
            OnDragHandler.Invoke(eventData);
	}

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("EndDrag");
        if (OnEndDragHandler != null)
            OnEndDragHandler.Invoke(eventData);        
    }
}
