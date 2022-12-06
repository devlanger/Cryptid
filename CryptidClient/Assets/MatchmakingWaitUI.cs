using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MatchmakingWaitUI : ViewUI
{
    [SerializeField] private MenuRouterUI router;
    [SerializeField] private Button cancelButton;

    private ConnectionController connectionController => ConnectionController.Instance;

    private void Awake()
    {
        cancelButton.onClick.AddListener(Cancel);
    }

    public override void Activate()
    {
        base.Activate();
        connectionController.AskToJoinMatchmaking();
    }

    private void Cancel()
    {
        connectionController.AskToRemoveMatchmaking();
        router.GoToView("online_games");
    }
}
