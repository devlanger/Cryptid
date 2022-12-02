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

[DefaultExecutionOrder(-1)]
public class ConnectionManager : MonoBehaviour, IGameServer, IAsyncDisposable
{
    public static ConnectionManager Instance { get; private set; }
    
    HubConnection connection;
    [SerializeField] private bool useRemote = false;
    [SerializeField] private string ip = "https://localhost:7006/gameHub";
    [SerializeField] private string remoteIp = "https://cryptid-backend.azurewebsites.net/gameHub";


    public bool IsConnected => connection.State == HubConnectionState.Connected;
    
    private GameController gameController;

    [Inject]
    public void Construct(GameController gameController)
    {
        this.gameController = gameController;
    }

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

        connection.On<byte>(nameof(ChangeMenuState), ChangeMenuState);
        connection.On<string>(nameof(LoadGameState), LoadGameState);
    }

    private async void Connect()
    {
        await connection.StartAsync();
    }

    #region Send
    public async Task AskToJoinMatchmaking()
    {
        await connection.SendAsync(nameof(AskToJoinMatchmaking));
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
    public void ChangeMenuState(byte state)
    {
        FindObjectOfType<MenuStateController>().ChangeMenuState(state);
    }

    public void LoadGameState(string gameJson)
    {
        Debug.Log($"Load json {gameJson}");
        GameState state = JsonConvert.DeserializeObject<GameState>(gameJson);
        GameController.InitialState = state;
        SceneManager.LoadScene(1);
    }
    #endregion
}
