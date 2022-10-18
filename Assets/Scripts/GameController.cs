using JetBrains.Annotations;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class GameController : IInitializable
{
    public GameState gameState;

    public event Action<GameState> OnFinishedTurn;
    public event Action<GameState> OnGameBegun;
    public event Action<GameState> OnGameUpdated;

    private UnitsController _unitsController;
    private GameStartSettings gameStartSettings;
    
    [Inject]
    public void Construct(UnitsController _unitsController, GameStartSettings gameStartSettings)
    {
        this.gameStartSettings = gameStartSettings;
        this._unitsController = _unitsController;
    }

    public void Initialize()
    {
        StartNewGame(gameStartSettings);
        OnGameBegun?.Invoke(gameState);
    }

    public void StartNewGame(GameStartSettings settings)
    {
        GameState state = new GameState();
        gameState = state;

        for (int i = 0; i < settings.players; i++)
        {
            string playerId = System.Guid.NewGuid().ToString();

            _unitsController.SpawnUnit(new UnitSpawnSettings()
            {
                ownerId = playerId,
                type = UnitType.PLAYER,
                spawnPoint = new Vector2Int(i * 2 - (settings.players / 2), 0)
            });

            state.backpacks[playerId] = new ItemsContainer();
            state.players[playerId] = new Player(playerId);
        }

        for (int i = 0; i < 5; i++)
        {
            _unitsController.SpawnUnit(new UnitSpawnSettings()
            {
                baseId = 1,
                ownerId = "",
                type = UnitType.MONSTER,
                spawnPoint = new Vector2Int(UnityEngine.Random.Range(-9, 9), UnityEngine.Random.Range(7, 27))
            });

            _unitsController.SpawnUnit(new UnitSpawnSettings()
            {
                ownerId = "",
                type = UnitType.CHEST,
                spawnPoint = new Vector2Int(UnityEngine.Random.Range(-9, 9), UnityEngine.Random.Range(7, 27))
            });
        }

        state.CurrentPlayerIndex = 0;
        state.CurrentPlayerId = state.players.Values.ToList()[0].Id;
    }

    public void FinishTurn()
    {
        gameState.FinishTurn();
        OnFinishedTurn?.Invoke(gameState);
    }

    public void RaiseUpdateEvent()
    {
        OnGameUpdated?.Invoke(gameState);
    }
}

[System.Serializable]
public class GameState
{
    public string CurrentPlayerId;
    public int CurrentPlayerIndex;
    public int TurnNumber = 1;

    public Dictionary<string, ItemsContainer> backpacks = new Dictionary<string, ItemsContainer>();
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

public class Player
{
    public Player(string id)
    {
        this.Id = id;
    }

    public string Id;
    public string Name;
    public int Gold = 0;
    public int Level = 1;
    public int Experience = 0;

}

[System.Serializable]
public class UnitState
{
    public string ownerId;
    public string unitId;
    public UnitType type;
    public int posX, posZ;
    public int health;
    public bool moved;
    public bool attacked;
    public int maxDmg = 1;
    public int minDmg = 2;
}