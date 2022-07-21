using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<int, Item> Items { get; } = new Dictionary<int, Item>();

    public void Add(Item item)
    {
        Items.Add(item.Slot, item);
    }

    public void Remove(int Slot)
    {
        Items.Remove(Slot);
    }

    public Item Get(int Slot)
    {
        Item item = null;
        Items.TryGetValue(Slot, out item);
        return item;
    }

    public void Set(Item newitem)
    {
        Items[newitem.Slot] = newitem;
    }

    public void SetFindItemSlot( int slot, int itemnum )
    {
        Items[slot].Count = itemnum;

        //foreach ( Item item in Items.Values)
        //{
        //    if( item.Info.Slot == slot )
        //    {
        //        if( itemnum <= 0 )
        //        {
        //            itemnum = 0;
        //        }

        //        {
        //            item.Count = itemnum;
        //        }

        //        break;
        //    }
        //}
    }

    public void SwapItemSlot( int orgSlot, int targetSlot )
    {
        Items[orgSlot].Slot = targetSlot;
        Items[targetSlot].Slot = orgSlot;
    }

    public Item Find(Func<Item, bool> condition)
    {
        foreach (Item item in Items.Values)
        {
            if (condition.Invoke(item))
                return item;
        }

        return null;
    }

    public void Clear()
    {
        Items.Clear();
    }
}
