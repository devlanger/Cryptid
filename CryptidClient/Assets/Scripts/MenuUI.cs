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

    [SerializeField] MatchmakingWaitUI matchmakingWaitUi;

    public static string Nickname { get; set; }

    private void Awake()
    {
        startNewButton.onClick.AddListener(StartNew);
        multiplayerButton.onClick.AddListener(OnlineGame);
        exitButton.onClick.AddListener(Exit);
        logoutButton.onClick.AddListener(Logout);

        versionText.text = $"Ver {Application.version}";

        FindObjectOfType<MenuStateController>().OnMenuStateChanged += MenuUI_OnMenuStateChanged;
    }

    public override void Activate()
    {
        base.Activate();
        nicknameText.text = Nickname;
    }

    private void Logout()
    {
        //TODO: Erase token

        loginScreen.GoToLogin();
    }

    private void MenuUI_OnMenuStateChanged(byte obj)
    {
        switch (obj)
        {
            case (byte)0:
                //matchmakingWaitUi.Deactivate();
                break;
            case (byte)1:
                //matchmakingWaitUi.Activate();
                break;
        }
    }

    private void OnlineGame()
    {
        router.GoToView("multiplayer");
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
