using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using Cryptid.Shared;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Zenject;
using UnityEngine.SceneManagement;
using UnityEditor.Rendering.LookDev;

public class ConnectionController : IInitializable, IGameServer, IAsyncDisposable
{
    HubConnection connection;
    [SerializeField] private bool useRemote = false;
    [SerializeField] private string ip = "http://localhost:5212/gameHub";
    [SerializeField] private string remoteIp = "https://cryptid-backend.azurewebsites.net/gameHub";

    public bool IsConnected => connection.State == HubConnectionState.Connected;
    
    private GameController gameController;
    private ActionsController actionsController;
    private UnitsController unitsController;

    [Inject]
    public void Construct(
        GameController gameController,
        ActionsController actionsController,
        UnitsController unitsController)
    {
        this.gameController = gameController;
        this.actionsController = actionsController;
        this.unitsController = unitsController;
    }

    public async ValueTask DisposeAsync()
    {
        if (connection != null)
        {
            await connection.DisposeAsync();
        }
    }

    public async void Connect()
    {
        await connection.StartAsync();
    }

    public async void Disconnect()
    {
        await connection.StopAsync();
        await DisposeAsync();
    }

    #region Send
    public async Task AskToJoinMatchmaking()
    {
        await connection.SendAsync(nameof(AskToJoinMatchmaking));
    }

    public async Task SendActionCommand(byte[] bytes)
    {
        await connection.SendAsync(nameof(SendActionCommand), bytes);
    }

    public async Task AskToRemoveMatchmaking()
    {
        await connection.SendAsync(nameof(AskToRemoveMatchmaking));
    }

    public async Task LoginWithAccessToken(string userId, string accessToken)
    {
        await connection.SendAsync(nameof(LoginWithAccessToken), userId, accessToken);
    }
    #endregion

    #region Receive

    public void HandleActionCommand(byte[] bytes)
    {
        var command = CommandReader.ReadCommandFromBytes(bytes);
        var action = ActionFactory.CreateActionFromCommand(gameController.gameState, command);

        if (actionsController.Execute(gameController.gameState, action, command))
        {
            switch (command.id)
            {
                case CommandType.MOVE:
                    new MoveCommandHandler(unitsController).Handle(command);
                    break;
                default:
                    break;
            }
        }
    }

    public void Initialize()
    {
        connection = new HubConnectionBuilder()
            .WithUrl(useRemote ? remoteIp : ip)
            .WithAutomaticReconnect()
            .Build();

        Connect();

        connection.Closed += async (error) =>
        {
            Debug.Log("Disconnected");
            //await connection.StartAsync();
        };

        connection.On<string>("ReceiveGame", (gameIp) =>
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                Debug.Log($"Received game: {gameIp}");
            });
        });

        connection.On<byte[]>(nameof(HandleActionCommand), HandleActionCommand);
    }
    #endregion
}
