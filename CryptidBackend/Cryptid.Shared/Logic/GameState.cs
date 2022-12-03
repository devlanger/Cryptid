using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cryptid.Logic;

[System.Serializable]
public class GameState
{
    public string CurrentPlayerId;
    public int CurrentPlayerIndex;
    public int TurnNumber = 1;

    public Dictionary<string, ItemsContainer> backpacks = new Dictionary<string, ItemsContainer>();
    public Dictionary<string, Player> players = new Dictionary<string, Player>();
    public Dictionary<string, UnitState> unitStates = new Dictionary<string, UnitState>();

    public bool GetUnit(string id, out UnitState state)
    {
        return unitStates.TryGetValue(id, out state);
    }

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

    public ItemsContainer GetCurrentPlayerBackpack()
    {
        return GetPlayerBackpack(CurrentPlayerId);
    }

    public ItemsContainer GetPlayerBackpack(string playerId)
    {
        return backpacks[CurrentPlayerId];
    }

    public Player GetCurrentPlayer()
    {
        return players[CurrentPlayerId];
    }
}