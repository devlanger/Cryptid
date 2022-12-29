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
    private CommandExecutor commandExecutor;
    private ActionsController actionsController;
    private GameController gameController;

    public string CurrentGameId { get; set; }

    [Inject]
    public void Construct(
        CommandExecutor commandExecutor,
        GameController gameController)
    {
        this.commandExecutor = commandExecutor;
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
        commandExecutor.RunCommand(gameController.gameState, command);
    }
}