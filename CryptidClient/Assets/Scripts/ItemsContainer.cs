using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsContainer
{
    private Dictionary<int, ItemState> items = new Dictionary<int, ItemState>();

    public int Capacity = 15;

    public Dictionary<int, ItemState> GetItems() => items;

    public bool AddItem(ItemState state, int slot = -1)
    {
        if (slot == -1)
        {
            for (int i = 0; i < Capacity; i++)
            {
                if (!items.ContainsKey(i))
                {
                    items[i] = state;
                    return true;
                }
            }
        }
        else
        {
            if (!items.ContainsKey(slot))
            {
                items[slot] = state;
                return true;
            }
        }

        return false;
    }
    
    public bool RemoveItem(int slot)
    {
        return items.Remove(slot);
    }

    public bool RetrieveItem(int slot, out ItemState item)
    {
        if(items.TryGetValue(slot, out item))
        {
            items.Remove(slot);
            return true;
        }

        return false;
    }

    public bool GetItem(int slot, out ItemState item)
    {
        if(items.TryGetValue(slot, out item))
        {
            return true;
        }

        return false;
    }

    public void AddItemRandomSlot(ItemState itemState)
    {
        var slots = GetFreeSlots();
        if(slots.Count > 0)
        {
            int slot = slots[UnityEngine.Random.Range(0, slots.Count)];
            AddItem(itemState, slot);
        }
    }

    private List<int> GetFreeSlots()
    {
        List<int> result = new List<int>();
        for (int i = 0; i < Capacity; i++)
        {
            if(!items.ContainsKey(i))
            {
                result.Add(i);
            }
        }

        return result;
    }
}

public class ItemState
{
    public string itemId;
    public int itemBaseId;
    public int inventoryId;
    public int slot = -1;

    public ItemState()
    {
        itemId = System.Guid.NewGuid().ToString();
    }
}
