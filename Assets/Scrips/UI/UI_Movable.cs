using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Movable : UI_Base
{
    protected RectTransform rt = null;

    public override void Init()
    {
        rt = GetComponent<RectTransform>();

        this.gameObject.BindEvent((e) =>
        {
            Vector3 mousePos = Input.mousePosition;
            this.transform.position = mousePos - new Vector3(0, rt.sizeDelta.y / 2, 0);
        }, Define.UIEvent.Drag);
    }
}
