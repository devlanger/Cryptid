using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System;

public class ConnectionManager : MonoBehaviour
{
    HubConnection connection;
    [SerializeField] private string ip = "https://localhost:7006/gameHub";

    public void Start()
    {
        connection = new HubConnectionBuilder()
            .WithUrl(ip)
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
    }

    private async void Connect()
    {
        await connection.StartAsync();
    }
}
