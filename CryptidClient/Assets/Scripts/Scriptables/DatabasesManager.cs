using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DatabasesManager : SerializedScriptableObject
{
    public Dictionary<int, ItemScriptable> items = new Dictionary<int, ItemScriptable>();
    public Dictionary<int, UnitScriptable> units = new Dictionary<int, UnitScriptable>();

    public bool GetUnit(int itemBaseId, out UnitScriptable unit)
    {
        return units.TryGetValue(itemBaseId, out unit);
    }

    public bool GetItem(int itemBaseId, out ItemScriptable itemData)
    {
        return items.TryGetValue(itemBaseId, out itemData);
    }
}