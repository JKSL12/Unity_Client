using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameScene : UI_Scene
{
    public UI_Stat statUI { get; private set; }
    public UI_Equip equipUI { get; private set; }
    public UI_Inventory InvenUI { get; private set; }
    public UI_Chat chatUI { get; private set; }

    public override void Init()
    {
        base.Init();

        statUI = GetComponentInChildren<UI_Stat>();
        equipUI = GetComponentInChildren<UI_Equip>();
        InvenUI = GetComponentInChildren<UI_Inventory>();
        chatUI = GetComponentInChildren<UI_Chat>();

        statUI.gameObject.SetActive(false);
        equipUI.gameObject.SetActive(false);
        InvenUI.gameObject.SetActive(false);
    }
}
