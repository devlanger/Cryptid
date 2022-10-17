using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UnitObjectsScriptable : SerializedScriptableObject
{
    public Dictionary<UnitType, Unit> prefabs = new Dictionary<UnitType, Unit>();
}
