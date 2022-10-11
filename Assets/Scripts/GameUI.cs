using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private OptionsUI options;

    [SerializeField] private Button exitButton;
    [SerializeField] private Button finishTurnButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Text turnText;
    [SerializeField] private Text playerText;

    private void Start()
    {
        Instance_OnGameStateChanged(GameController.Instance.gameState);
        GameController.Instance.OnGameStateChanged += Instance_OnGameStateChanged;

        optionsButton.onClick.AddListener(OptionsClick);
        exitButton.onClick.AddListener(ExitToMenu);
        finishTurnButton.onClick.AddListener(FinishTurn_Click);
    }

    private void OptionsClick()
    {
        options.Toggle();
    }

    private void ExitToMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void FinishTurn_Click()
    {
        GameController.Instance.FinishTurn();
    }

    private void Instance_OnGameStateChanged(GameState obj)
    {
        turnText.text = $"Turn {obj.TurnNumber}";
        playerText.text = $"Player {obj.CurrentPlayerId}";
    }
}
