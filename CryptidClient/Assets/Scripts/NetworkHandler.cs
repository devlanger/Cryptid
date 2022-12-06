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
using Cryptid.Shared;
using UnityEngine.XR;
using Zenject.Asteroids;

public class NetworkHandler : IInitializable, IDisposable
{
    private UnitsController unitsController;
    private ActionsController actionsController;
    private GameController gameController;

    public string CurrentGameId { get; set; }

    [Inject]
    public void Construct(
        GameController gameController, 
        UnitsController _unitsController, 
        ActionsController actionsController)
    {
        this.unitsController = _unitsController;
        this.actionsController = actionsController;
        this.gameController = gameController;
    }

    public void Dispose()
    {
        if (ConnectionController.Instance)
        {
            ConnectionController.Instance.OnCommandHandle -= Instance_OnCommandHandle;
        }
    }

    public void Initialize()
    {
        ConnectionController.Instance.OnCommandHandle += Instance_OnCommandHandle;
    }

    private void Instance_OnCommandHandle(byte[] bytes)
    {
        var command = CommandReader.ReadCommandFromBytes(bytes);
        var action = ActionFactory.CreateActionFromCommand(gameController.gameState, command);
        command.PlayerId = LoginUI.UserData.id;

        var result = actionsController.Execute(gameController.gameState, action, command);
        if (result.IsSuccess)
        {
            try
            {
                switch (command.id)
                {
                    case CommandType.MOVE:
                        new MoveCommandHandler(unitsController).Handle(gameController.gameState, command);
                        break;
                    case CommandType.NEXT_TURN:
                        new NextTurnCommandHandler().Handle(gameController.gameState, command);
                        break;
                    case CommandType.ATTACK_TARGET:
                        new AttackCommandHandler(unitsController).Handle(gameController.gameState, command);
                        break;
                }
            }
            catch(Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }
        else
        {
            Debug.LogError($"Command issue {command.id}: {result.Error}");
        }
    }
}