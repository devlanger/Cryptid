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

public class ConnectionController : MonoBehaviour, IGameServer, IAsyncDisposable
{
    public static ConnectionController Instance { get; private set; }

    HubConnection connection;
    [SerializeField] private bool useRemote = false;
    [SerializeField] private string api = "https://localhost:7212/";
    [SerializeField] private string remoteApi = "https://localhost:7212/";
    [SerializeField] private string ip = "https://localhost:7212/gameHub";
    [SerializeField] private string remoteIp = "https://cryptid-backend.azurewebsites.net/gameHub";

    public event Action<byte[]> OnCommandHandle;

    public bool IsConnected => connection.State == HubConnectionState.Connected;

    public string Api { get => useRemote ? remoteApi : api; set => api = value; }

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

    private async void Start()
    {
        if(LoginUI.UserData != null)
        {
            await BuildConnection();
        }
    }

    private async void OnDestroy()
    {
        await Disconnect();
    }

    public async Task BuildConnection()
    {
        string token = LoginUI.UserData.token;
        connection = new HubConnectionBuilder()
            .WithUrl(useRemote ? remoteIp : ip, options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(token);
                options.Headers.Add("access_token", token);
                options.SkipNegotiation = false;
            })
            .Build();


        connection.Closed += async (error) =>
        {
            Debug.Log("Disconnected");
            FindObjectOfType<LoginUI>().GoToLogin();
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
        connection.On<string, string>(nameof(LoadGameState), LoadGameState);

        await Connect();
    }

    public async ValueTask DisposeAsync()
    {
        if (connection != null)
        {
            await connection.DisposeAsync();
        }
    }

    public async Task Connect()
    {
        await connection.StartAsync();
    }

    public async Task Disconnect()
    {
        await connection?.StopAsync();
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
        OnCommandHandle?.Invoke(bytes);
    }

    public void LoadGameState(string id, string gameStateJson)
    {
        var gameState = JsonConvert.DeserializeObject<GameState>(gameStateJson);
        Debug.Log($"Load game {id}: {gameStateJson}");
        GameController.InitialState = new Tuple<string, GameState>(id, gameState);
        SceneManager.LoadScene(1);
    }
    #endregion
}
