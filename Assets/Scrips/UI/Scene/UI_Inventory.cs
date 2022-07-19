using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Inventory : UI_Movable
{
    public List<UI_Inventory_Item> Items { get; } = new List<UI_Inventory_Item>();

    public override void Init()
    {
        base.Init();

        Items.Clear();

        GameObject grid = transform.Find("ItemGrid").gameObject;
        foreach (Transform child in grid.transform)
            Destroy(child.gameObject);

        for(int i = 0; i < 20; i++)
        {
            GameObject go = Managers.Resource.Instantiate("UI/Scene/UI_Inventory_Item", grid.transform);
            UI_Inventory_Item item = go.GetOrAddComponent<UI_Inventory_Item>();
            Items.Add(item);
        }

        RefreshUI();
    }

    public void RefreshUI()
    {
        if (Items.Count == 0)
            return;

        List<Item> items = Managers.Inven.Items.Values.ToList();
        items.Sort((left, right) => { return left.Slot - right.Slot; });

        foreach(Item item in items)
        {
            if(item.Slot < 0 || item.Slot >= 20)
            continue;

            Items[item.Slot].SetItem(item.Slot, item);
        }
    }

    public void ChangeSlotItem(int orgSlot, int destSlot)
    {
        List<Item> items = Managers.Inven.Items.Values.ToList();
        items.Sort((left, right) => { return left.Slot - right.Slot; });

        Item orgItem = null;
        Item destItem = null;
        foreach (Item item in items)
        {
            if (orgItem != null && destItem != null) break;

            if (item.Slot == orgSlot) orgItem = item;
            if (item.Slot == destSlot) destItem = item;
        }

        if (orgItem != null && destItem != null)
        {
            Debug.Log($"{orgSlot}, {orgItem.TemplateId} / {destSlot}, {destItem.TemplateId}");
            Managers.Inven.SetItemSlot(orgSlot, destItem);
            Managers.Inven.SetItemSlot(destSlot, orgItem);
            Debug.Log($"{orgSlot}, {orgItem.TemplateId} / {destSlot}, {destItem.TemplateId}");
        }
    }
}
