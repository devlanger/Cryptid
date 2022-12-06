using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;
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

    private ConnectionController connectionController => ConnectionController.Instance;
    public static string PrefsPath = "userdata";

    public static string Nickname { get; set; }

    private void Awake()
    {
        string path = Path.Combine(Application.dataPath.Replace("/Assets", ""), "prefs.txt");
        if (System.IO.File.Exists(path))
        {
            PrefsPath = System.IO.File.ReadAllText(path);
        }

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
        PlayerPrefs.DeleteKey(MenuUI.PrefsPath);
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
