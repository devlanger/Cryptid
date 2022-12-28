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
    public static Tuple<string, GameState> InitialState;

    public event Action<GameState> OnFinishedTurn;
    public event Action<GameState> OnGameBegun;
    public event Action<GameState> OnGameUpdated;

    private UnitsController _unitsController;
    private ActionsController actionsController;

    public string CurrentGameId { get; set; }

    public bool IsMyTurn(GameState state) => state.CurrentPlayerId == LoginUI.UserData.id;

    [Inject]
    public void Construct(UnitsController _unitsController, ActionsController actionsController)
    {
        this._unitsController = _unitsController;
        this.actionsController = actionsController;
    }

    public void LoadGame(GameState state)
    {
        this.gameState = state;
        foreach (var unit in state.unitStates)
        {
            _unitsController.SpawnUnit(unit.Value);
        }

        FinishTurn();
    }

    public void FinishTurn()
    {
        //gameState.FinishTurn();
        OnFinishedTurn?.Invoke(gameState);
        OnGameUpdated?.Invoke(gameState);
    }

    public void RaiseUpdateEvent()
    {
        OnGameUpdated?.Invoke(gameState);
    }

    public void Initialize()
    {
        if(InitialState != null)
        {
            CurrentGameId = InitialState.Item1;
            LoadGame(InitialState.Item2);
        }
    }
}