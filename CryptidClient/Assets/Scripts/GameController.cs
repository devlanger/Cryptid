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
    private GameController gameController;

    public string CurrentGameId { get; set; }
    public string CurrentUserId => gameState.IsOnline ? LoginUI.UserData.id : "local";

    public bool IsMyTurn(GameState state) => state.CurrentPlayerId == gameController.CurrentUserId;

    [Inject]
    public void Construct(
        UnitsController _unitsController,
        ActionsController actionsController,
        GameController gameController)
    {
        this._unitsController = _unitsController;
        this.actionsController = actionsController;
        this.gameController = gameController;
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