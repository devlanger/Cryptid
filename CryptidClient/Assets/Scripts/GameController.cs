using System.Text;
using CryptidClient.Assets.Scripts.MapLoader;
using JetBrains.Annotations;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Zenject;

public class GameController : IInitializable
{
    public GameState gameState;
    public static GameState InitialState;

    public event Action<GameState> OnFinishedTurn;
    public event Action<GameState> OnGameBegun;
    public event Action<GameState> OnGameUpdated;

    private UnitsController _unitsController;
    
    [Inject]
    public void Construct(UnitsController _unitsController)
    {
        this._unitsController = _unitsController;
    }

    public void LoadGame(GameState state)
    {
        this.gameState = state;
        foreach (var unit in state.unitStates)
        {
            _unitsController.SpawnUnit(unit.Value);
        }
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

    public void Initialize()
    {
        if(InitialState != null)
        {
            LoadGame(InitialState);
        }
    }
}