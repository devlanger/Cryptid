using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchmakingWaitUI : ViewUI
{
    [SerializeField] private Button cancelButton;

    private void Awake()
    {
        cancelButton.onClick.AddListener(Cancel);
    }

    private void Cancel()
    {
        ConnectionManager.Instance.AskToRemoveMatchmaking();
        Deactivate();
    }
}
