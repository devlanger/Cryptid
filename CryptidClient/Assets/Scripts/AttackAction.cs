using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.Asteroids;
using static UnityEngine.UI.CanvasScaler;

public class AttackAction : GameAction
{
    private UnitsController _unitsController;
    private PopupsController popupsController;

    public override bool CanExecute(GameState state)    
    {
        if (_unitsController.GetUnit(unitId, out Unit unit) && _unitsController.GetUnit(targetUnitId, out Unit unitTarget))
        {
            if(unit == unitTarget)
            {
                return false;
            }

            switch (unit.state.type)
            {
                case UnitType.PLAYER:
                case UnitType.MONSTER:
                    break;
                case UnitType.DROP:
                case UnitType.DOORS:
                case UnitType.CHEST:
                    return false;
                default:
                    break;
            }

            if (unit.state.type == UnitType.PLAYER)
            {
                if (state.CurrentPlayerId != unit.state.ownerId)
                {
                    return false;
                }
            }

            if(Vector3.Distance(unit.transform.position, unitTarget.transform.position) > 3)
            {
                popupsController.ShowPopup(new PopupSettings()
                {
                    text = "You are too far away!",
                    color = Color.red,
                });
                return false;
            }

            if(unit.state.attacked)
            {
                popupsController.ShowPopup(new PopupSettings()
                {
                    text = "You cannot attack anymore.",
                    color = Color.red,
                });
                return false;
            }
        }


        return base.CanExecute(state);
    }

    private string unitId;
    private string targetUnitId;
    private ActionsController actionsController;
    private GameController gameController;

    public AttackAction(GameState gameState, PopupsController popupsController, GameController gameController, ActionsController actionsController, UnitsController unitsController, 
        string playerId, string unitId, string targetUnitId) : base(gameState, playerId)
    {
        _unitsController = unitsController;
        this.popupsController = popupsController;
        this.gameController = gameController;
        this.actionsController = actionsController;
        this.unitId = unitId;
        this.targetUnitId = targetUnitId;
    }

    public override void Execute(GameState state)
    {
        if(_unitsController.GetUnit(unitId, out Unit unitAttacker) && _unitsController.GetUnit(targetUnitId, out Unit unitTarget))
        {
            unitAttacker.state.moved = true;
            unitAttacker.state.attacked = true;

            var s = DOTween.Sequence()
                .Append(unitAttacker.transform.DOPunchPosition(unitTarget.transform.position - unitAttacker.transform.position, 0.25f))
                .Join(unitTarget.transform.DOShakeScale(0.25f).SetDelay(0.125f));

            switch (unitTarget.state.type)
            {
                case UnitType.PLAYER:
                case UnitType.MONSTER:
                    SoundsController.Instance.PlaySound(SoundId.CLASH_1);
                    unitTarget.state.health -= UnityEngine.Random.Range(unitAttacker.state.minDmg, unitAttacker.state.maxDmg + 1);
                    if (unitTarget.state.health <= 0)
                    {
                        s.onComplete += () => 
                        {
                            unitAttacker.StartCoroutine(x(unitTarget.UnitId)); 
                        };

                        //actionsController.Execute(new DefeatRewardAction(gameState, gameController, actionsController, _unitsController, playerId, unitId, targetUnitId));
                    }
                    else
                    {
                        s.onComplete += () =>
                        {
                            //actionsController.Execute(new AttackAction(state, popupsController, gameController, actionsController, _unitsController, "", unitTarget.UnitId, unitAttacker.UnitId));
                        };
                    }
                    break;
                case UnitType.DROP:
                    break;
                case UnitType.DOORS:
                    break;
                case UnitType.CHEST:
                    SoundsController.Instance.PlaySound(SoundId.LOOT_PICKUP);
                    var backpack = gameController.gameState.GetCurrentPlayerBackpack();
                    backpack.AddItem(new ItemState()
                    {
                        itemBaseId = UnityEngine.Random.Range(1, 4)
                    });
                    gameController.RaiseUpdateEvent();
                    unitTarget.Die();
                    
                    break;
                default:
                    break;
            }

        }
    }

    private IEnumerator x(string unitId)
    {
        if (_unitsController.GetUnit(targetUnitId, out Unit unitTarget))
        {
            unitTarget.Die();
            yield return new WaitForSeconds(1);
            _unitsController.RemoveUnit(unitId);
        }
    }
}
