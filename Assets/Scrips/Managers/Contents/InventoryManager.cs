using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<int, Item> Items { get; } = new Dictionary<int, Item>();

    public void Add(Item item)
    {
        Items.Add(item.ItemDbId, item);
    }

    public void Remove(int itemDbId)
    {
        Items.Remove(itemDbId);
    }

    public Item Get(int itemDbId)
    {
        Item item = null;
        Items.TryGetValue(itemDbId, out item);
        return item;
    }

    public void SetFindItemSlot( int slot, int itemnum )
    {
        foreach( Item item in Items.Values)
        {
            if( item.Info.Slot == slot )
            {
                if( itemnum <= 0 )
                {
                    itemnum = 0;
                }

                {
                    item.Count = itemnum;
                }

                break;
            }
        }
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
