using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using Cryptid.Shared;
using System.Threading.Tasks;

[DefaultExecutionOrder(-1)]
public class ConnectionManager : MonoBehaviour, IGameServer, IAsyncDisposable
{
    public static ConnectionManager Instance { get; private set; }

    HubConnection connection;
    [SerializeField] private string ip = "https://localhost:7006/gameHub";
    public bool IsConnected => connection.State == HubConnectionState.Connected;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (connection != null)
        {
            await connection.DisposeAsync();
        }
    }

    public void Start()
    {
        connection = new HubConnectionBuilder()
            .WithUrl(ip)
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

        connection.On<byte>(nameof(ChangeMenuState), ChangeMenuState);
    }

    private async void Connect()
    {
        await connection.StartAsync();
    }

    #region Send
    public async void AskToJoinMatchmaking()
    {
        await connection.SendAsync(nameof(AskToJoinMatchmaking));
    }

    public async void AskToRemoveMatchmaking()
    {
        await connection.SendAsync(nameof(AskToRemoveMatchmaking));
    }

    public async void LoginWithAccessToken(string userId, string accessToken)
    {
        await connection.SendAsync(nameof(LoginWithAccessToken), userId, accessToken);
    }
    #endregion

    #region Receive
    public void ChangeMenuState(byte state)
    {
        FindObjectOfType<MenuStateController>().ChangeMenuState(state);
    }
    #endregion
}
