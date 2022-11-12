using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : ViewUI
{
    [SerializeField] private MenuRouterUI router;
    [SerializeField] private Button startNewButton;
    [SerializeField] private Button multiplayerButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Text versionText;

    [SerializeField] MatchmakingWaitUI matchmakingWaitUi;

    private void Awake()
    {
        startNewButton.onClick.AddListener(StartNew);
        multiplayerButton.onClick.AddListener(OnlineGame);
        exitButton.onClick.AddListener(Exit);

        versionText.text = $"Ver {Application.version}";

        FindObjectOfType<MenuStateController>().OnMenuStateChanged += MenuUI_OnMenuStateChanged;
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
            case (byte)2:
                SceneManager.LoadScene(1);
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
