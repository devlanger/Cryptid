using Cryptid.Logic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.Asteroids;

public class Unit : SerializedMonoBehaviour
{
    public string UnitId = "";
    private GameController gameController;

    public UnitState state
    {
        get
        {
            if(gameController.gameState.unitStates.TryGetValue(UnitId, out var state))
            {
                return state;
            }

            return null;
        }
    }

    public bool IsMine => state.ownerId == gameController.gameState.CurrentPlayerId;

    public void Construct(GameController gameController)
    {
        this.gameController = gameController;
    }

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
