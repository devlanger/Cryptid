using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.Asteroids;
using static UnityEngine.UI.CanvasScaler;

public class DefeatRewardAction : GameAction
{
    private UnitsController _unitsController;
    private string unitId;
    private string targetUnitId;
    private ActionsController actionsController;
    private GameController gameController;

    public DefeatRewardAction(GameState gameState, GameController gameController, ActionsController actionsController, UnitsController unitsController, 
        string playerId, string unitId, string targetUnitId) : base(gameState, playerId)
    {
        _unitsController = unitsController;
        this.gameController = gameController;
        this.actionsController = actionsController;
        this.unitId = unitId;
        this.targetUnitId = targetUnitId;
    }

    public override void Execute(GameState state)
    {
        if(_unitsController.GetUnit(unitId, out Unit unitAttacker) && _unitsController.GetUnit(targetUnitId, out Unit unitTarget))
        {
            var player = gameState.GetCurrentPlayer();
            player.Experience += 10;
            player.Gold += 10;

            if(player.Experience >= 100)
            {
                player.Experience = 0;
                player.Level++;
            }

            gameController.RaiseUpdateEvent();
        }
    }
}
