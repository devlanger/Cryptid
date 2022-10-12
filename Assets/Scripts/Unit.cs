using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : SerializedMonoBehaviour
{
    public string unitId = "";
    public UnitState state => GameController.Instance.gameState.unitStates[unitId];
    public bool IsMine => state.ownerId == GameController.Instance.gameState.CurrentPlayerId;
}
