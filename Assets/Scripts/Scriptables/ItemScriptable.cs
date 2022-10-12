using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemScriptable : SerializedScriptableObject
{
    public int baseId;
    public string name;
    public string description;
    public Sprite icon;
}
