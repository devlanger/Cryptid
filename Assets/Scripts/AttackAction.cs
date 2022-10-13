using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class AttackAction : GameAction
{
    public override bool CanExecute(GameState state)
    {
        if (UnitsController.Instance.GetUnit(unitId, out Unit unit) && UnitsController.Instance.GetUnit(targetUnitId, out Unit unitTarget))
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
                return false;
            }

            if(unit.state.attacked)
            {
                return false;
            }
        }


        return base.CanExecute(state);
    }

    private string unitId;
    private string targetUnitId;

    public AttackAction(GameState gameState, string playerId, string unitId, string targetUnitId) : base(gameState, playerId)
    {
        this.unitId = unitId;
        this.targetUnitId = targetUnitId;
    }

    public override void Execute(GameState state)
    {
        if(UnitsController.Instance.GetUnit(unitId, out Unit unitAttacker) && UnitsController.Instance.GetUnit(targetUnitId, out Unit unitTarget))
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
                            UnitsController.Instance.StartCoroutine(x(unitTarget.UnitId)); 
                        };
                    }
                    else
                    {
                        s.onComplete += () =>
                        {
                            ActionsController.Instance.Execute(new AttackAction(state, "", unitTarget.UnitId, unitAttacker.UnitId));
                        };
                    }
                    break;
                case UnitType.DROP:
                    break;
                case UnitType.DOORS:
                    break;
                case UnitType.CHEST:
                    SoundsController.Instance.PlaySound(SoundId.LOOT_PICKUP);
                    string playerId = unitAttacker.state.ownerId;
                    var backpack = GameController.Instance.gameState.GetCurrentPlayerBackpack();
                    backpack.AddItem(new ItemState()
                    {
                        itemBaseId = UnityEngine.Random.Range(1, 4)
                    });
                    unitTarget.Die();
                    break;
                default:
                    break;
            }

        }
    }

    private IEnumerator x(string unitId)
    {
        if (UnitsController.Instance.GetUnit(targetUnitId, out Unit unitTarget))
        {
            unitTarget.Die();
            yield return new WaitForSeconds(1);
            UnitsController.Instance.RemoveUnit(unitId);
        }
    }
}
