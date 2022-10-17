using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
using Zenject.Asteroids;

public class GameUI : MonoBehaviour
{
    [SerializeField] private OptionsUI options;

    [SerializeField] private Button exitButton;
    [SerializeField] private Button finishTurnButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Text turnText;
    private GameController gameController;

    [Inject]
    public void Construct(GameController gameController)
    {
        this.gameController = gameController;
    }

    private void Start()
    {
        Instance_OnGameStateChanged(gameController.gameState);
        gameController.OnFinishedTurn += Instance_OnGameStateChanged;

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
        gameController.FinishTurn();
    }

    private void Instance_OnGameStateChanged(GameState obj)
    {
        turnText.text = $"Turn {Environment.NewLine}{obj.TurnNumber}";
    }
}
