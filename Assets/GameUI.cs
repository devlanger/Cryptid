using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Button finishTurnButton;
    [SerializeField] private Text turnText;
    [SerializeField] private Text playerText;

    private void Start()
    {
        Instance_OnGameStateChanged(GameController.Instance.gameState);
        GameController.Instance.OnGameStateChanged += Instance_OnGameStateChanged;

        finishTurnButton.onClick.AddListener(FinishTurn_Click);
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
