using Cryptid.Shared;
using Cryptid.Shared.Logic.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI experienceText;
    [SerializeField] private Slider experienceSlider;

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
        gameController.OnGameUpdated += Instance_OnGameStateChanged;

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
        ConnectionController.Instance.SendActionCommand(CommandReader.WriteCommandToBytes(new NextTurnAction.Command
        {
            id = CommandType.NEXT_TURN,
            gameId = gameController.CurrentGameId,
        }));
    }

    private void Instance_OnGameStateChanged(GameState obj)
    {
        if(obj == null)
        {
            return;
        }
        Player player = obj.GetCurrentPlayer();
        
        turnText.SetText($"Turn {Environment.NewLine}{obj.TurnNumber}");
        levelText.SetText($"Level {Environment.NewLine}{player.Level}");
        experienceText.SetText($"{player.Experience}/100");
        
        experienceSlider.value = player.Experience;
    }
}
