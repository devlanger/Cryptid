using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UnitScriptable : SerializedScriptableObject
{
    public string name;
    public int level;
    [TextArea]
    public string description;
    public int health = 3;
    public int minDamage = 1;
    public int maxDamage = 2;
    public Sprite icon;
}
