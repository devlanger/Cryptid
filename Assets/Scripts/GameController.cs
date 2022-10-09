using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [OdinSerialize]
    [field:NonSerialized]
    public GameState gameState;

    public event Action<GameState> OnGameStateChanged;

    public void FinishTurn()
    {
        gameState.FinishTurn();
        OnGameStateChanged?.Invoke(gameState);
    }
}

[System.Serializable]
public class GameState
{
    public string CurrentPlayerId;
    public int CurrentPlayerIndex;
    public int TurnNumber = 1;

    public Dictionary<string, Player> players = new Dictionary<string, Player>();
    public Dictionary<string, UnitState> unitStates = new Dictionary<string, UnitState>();

    public void FinishTurn()
    {
        var list = players.Values.ToList();
        CurrentPlayerIndex++;
        if(CurrentPlayerIndex == list.Count)
        {
            TurnNumber++;
            CurrentPlayerIndex = 0;
        }

        foreach (var item in unitStates)
        {
            item.Value.moved = false;
            item.Value.attacked = false;
        }

        CurrentPlayerId = list[CurrentPlayerIndex].Id;
    }
}

public class Player
{
    public Player(string id)
    {
        this.Id = id;
    }

    public string Id;
    public string Name;
}

[System.Serializable]
public class UnitState
{
    public string ownerId;
    public string unitId;
    public UnitType type;
    public int posX, posZ;
    public float health;
    public bool moved;
    public bool attacked;
}