using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class MenuUI : ViewUI
{
    [SerializeField] private TextMeshProUGUI nicknameText;
    [SerializeField] private MenuRouterUI router;
    [SerializeField] private LoginUI loginScreen;
    [SerializeField] private Button startNewButton;
    [SerializeField] private Button logoutButton;
    [SerializeField] private Button multiplayerButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Text versionText;

    private ConnectionController connectionController;

    public static string Nickname { get; set; }

    [Inject]
    public void Construct(ConnectionController connectionController)
    {
        this.connectionController = connectionController;
    }

    private void Awake()
    {
        startNewButton.onClick.AddListener(StartNew);
        multiplayerButton.onClick.AddListener(OnlineGame);
        exitButton.onClick.AddListener(Exit);
        logoutButton.onClick.AddListener(Logout);

        versionText.text = $"Ver {Application.version}";
    }

    public override void Activate()
    {
        base.Activate();
        nicknameText.text = LoginUI.UserData.nickname;
    }

    private void Logout()
    {
        //TODO: Erase token
        PlayerPrefs.DeleteKey("userdata");
        loginScreen.GoToLogin();
        connectionController.Disconnect();
    }

    private void OnlineGame()
    {
        router.GoToView("online_games");
    }

    private void Exit()
    {
        Application.Quit();
    }

    private void StartNew()
    {
        SceneManager.LoadScene(1);
    }
}
