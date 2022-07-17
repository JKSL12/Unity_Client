using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageClick : UI_Base
{
    Image clickItemImage = null;

    public override void Init()
    {
        clickItemImage = GetComponent<Image>();
    }

    public void Show()
    {
        Data.ItemData itemData = null;
        Managers.Data.ItemDict.TryGetValue(Managers.Object.MyPlayer.clickItem.templateId, out itemData);

        if (itemData == null)
        {
            clickItemImage.gameObject.SetActive(false);
            clickItemImage.sprite = null;
            return;
        }

        Sprite icon = Managers.Resource.Load<Sprite>(itemData.iconPath);
        clickItemImage.sprite = icon;

        Vector3 mousePos = Input.mousePosition;
        //mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        clickItemImage.transform.position = mousePos;
    }
}
