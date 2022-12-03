using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchmakingWaitUI : ViewUI
{
    [SerializeField] private MenuRouterUI router;
    [SerializeField] private Button cancelButton;

    private void Awake()
    {
        cancelButton.onClick.AddListener(Cancel);
    }

    public override void Activate()
    {
        base.Activate();
        ConnectionManager.Instance.AskToJoinMatchmaking();
    }

    private void Cancel()
    {
        ConnectionManager.Instance.AskToRemoveMatchmaking();
        router.GoToView("online_games");
    }
}
