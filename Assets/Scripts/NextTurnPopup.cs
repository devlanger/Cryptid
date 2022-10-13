using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextTurnPopup : ViewUI
{
    [SerializeField] private Transform container;

    private void Awake()
    {
        GameController.Instance.OnFinishedTurn += Instance_OnFinishedTurn;
        GameController.Instance.OnGameBegun += Instance_OnGameBegun;
    }

    private void Instance_OnGameBegun(GameState obj)
    {
        Activate();
    }

    private void Instance_OnFinishedTurn(GameState obj)
    {
        Activate();
    }

    public override void Activate()
    {
        base.Activate();

        container.transform.localScale = Vector3.one;

        var s = DOTween.Sequence()
            .Append(container.DOPunchScale(Vector3.one * 2, 0.25f))
            .AppendInterval(1f)
            .Append(container.DOScale(0, 0.25f))
            .OnStepComplete(() => { Deactivate(); });
        
    }
}
