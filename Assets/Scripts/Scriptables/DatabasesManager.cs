using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DatabasesManager : SerializedScriptableObject
{
    public Dictionary<int, ItemScriptable> items = new Dictionary<int, ItemScriptable>();
    public List<UnitScriptable> units = new List<UnitScriptable>();

    public bool GetItem(int itemBaseId, out ItemScriptable itemData)
    {
        return items.TryGetValue(itemBaseId, out itemData);
    }
}