using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : SerializedMonoBehaviour
{
    public string UnitId = "";
    public UnitState state => GameController.Instance.gameState.unitStates[UnitId];
    public bool IsMine => state.ownerId == GameController.Instance.gameState.CurrentPlayerId;

    public void Die()
    {
        var c = GetComponent<Collider>();
        if (c != null)
        {
            c.enabled = false;
        }

        var outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false;
        }

        GetComponentInChildren<Animator>()?.SetBool("dead", true);
    }
}
