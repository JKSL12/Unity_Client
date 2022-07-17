using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory_Item : UI_Base
{
    [SerializeField]
    Image _icon;

    [SerializeField]
    Image _frame = null;

    [SerializeField]
    Text _text = null;

    public int ItemDbId { get; private set; }
    public int TemplateId { get; private set; }
    public int Count { get; private set; }
    public bool Equipped { get; private set; }
    public int Slot { get; private set; }

    public override void Init()
    {
        _icon.gameObject.BindEvent((e) =>
        {
            if (TemplateId == 0) return;

            if (Managers.Object.MyPlayer == null) return;

            if (Managers.Object.MyPlayer.clickItem != null)
            {
                UI_GameScene gameSceneUI = Managers.UI.SceneUI as UI_GameScene;

                if (Managers.Object.MyPlayer.clickItem.slot == -1)
                {                  
                    Managers.Object.MyPlayer.clickItem.itemDbId = ItemDbId;
                    Managers.Object.MyPlayer.clickItem.templateId = TemplateId;
                    Managers.Object.MyPlayer.clickItem.slot = Slot;

                    gameSceneUI.IC.gameObject.SetActive(true);

                    SetIconA(0.5f);
                }
                else
                {                    
                    gameSceneUI.InvenUI.Items[Managers.Object.MyPlayer.clickItem.slot].SetIconA(1.0f);

                    if (Managers.Object.MyPlayer.clickItem.slot != Slot)
                    {
                        // 아이템 자리 변경
                    }

                    gameSceneUI.IC.gameObject.SetActive(false);

                    Managers.Object.MyPlayer.clickItem.Init();
                    SetIconA(1.0f);
                }
            }
            

        }, Define.UIEvent.LClick);

        _icon.gameObject.BindEvent((e) =>
        {
            Data.ItemData itemData = null;
            Managers.Data.ItemDict.TryGetValue(TemplateId, out itemData);

            if (itemData == null) return;

            if (itemData.itemType == ItemType.Consumable)
                return;

            C_EquipItem equipPacket = new C_EquipItem();
            equipPacket.ItemDbId = ItemDbId;
            equipPacket.Equipped = !Equipped;

            Managers.Network.Send(equipPacket);
        }, Define.UIEvent.LDbClick);

        _icon.gameObject.BindEvent((e) =>
        {
            Data.ItemData itemData = null;
            Managers.Data.ItemDict.TryGetValue(TemplateId, out itemData);

            if (itemData == null) return;

            if (itemData.itemType != ItemType.Consumable)
                return;

            if (Count <= 0) return;

            C_UseItem usePacket = new C_UseItem();
            usePacket.ItemDbId = ItemDbId;
            usePacket.TemplateId = TemplateId;
            usePacket.UseNum = 1;

            Managers.Network.Send(usePacket);

        }, Define.UIEvent.RClick);

        SetItem(-1, null);
    }
    
    public void SetItem(int slot, Item item)
    {
        if (item == null)
        {
            Slot = slot;
            ItemDbId = 0;
            TemplateId = 0;
            Count = 0;
            Equipped = false;

            _icon.gameObject.SetActive(false);
            _frame.gameObject.SetActive(false);
            _text.gameObject.SetActive(false);
        }
        else
        {
            Slot = slot;
            ItemDbId = item.ItemDbId;
            TemplateId = item.TemplateId;
            Count = item.Count;
            Equipped = item.Equipped;

            Data.ItemData itemData = null;
            Managers.Data.ItemDict.TryGetValue(TemplateId, out itemData);

            Sprite icon = Managers.Resource.Load<Sprite>(itemData.iconPath);
            _icon.sprite = icon;

            _text.text = $"{Count}";

            

            if (Count > 0)
            {
                _icon.gameObject.SetActive(true);
                _frame.gameObject.SetActive(Equipped);
                _text.gameObject.SetActive(true);
            }                
            else
            {
                _icon.gameObject.SetActive(false);
                _frame.gameObject.SetActive(false);
                _text.gameObject.SetActive(false);
            }
        }
    }

    public void SetIconA(float value)
    {
        _icon.color = new Color(_icon.color.r, _icon.color.g, _icon.color.b, value);
    }
}
