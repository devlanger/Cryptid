using Cryptid.Logic;
using System;
using System.Collections;
using System.Collections.Generic;

public class AttackAction : GameAction
{
    public class Command : CommandBase
    {
        public string TargetId { get; internal set; }
        public string UnitId { get; internal set; }
    }

    public AttackAction(GameState gameState) : base(gameState)
    {

    }

    public override ActionsController.Result CanExecute(GameState state, CommandBase command)    
    {
        var c = command as Command;
        string unitId = c.UnitId;
        string targetUnitId = c.TargetId;


        if (state.GetUnit(unitId, out UnitState unit) && state.GetUnit(targetUnitId, out UnitState unitTarget))
        {
            if(unit == unitTarget)
            {
                return ActionsController.Result.Failure("Unit cant attack itself");
            }

            switch (unit.type)
            {
                case UnitType.PLAYER:
                case UnitType.MONSTER:
                    break;
                case UnitType.DROP:
                case UnitType.DOORS:
                case UnitType.CHEST:
                    return ActionsController.Result.Failure("Wrong unit type");
                default:
                    break;
            }

            if (unit.type == UnitType.PLAYER)
            {
                if (state.CurrentPlayerId != unit.ownerId)
                {
                    return ActionsController.Result.Failure("Not my turn");
                }
            }

            // if(Vector3.Distance(unit.transform.position, unitTarget.transform.position) > 3)
            // {
            //     return false;
            // }

            if(unit.attacked)
            {
                return ActionsController.Result.Failure("Unit already attacked");
            }
        }


        return base.CanExecute(state, command);
    }

    public override void Execute(GameState state, CommandBase command)
    {
        var c = command as Command;
        string unitId = c.UnitId;
        string targetUnitId = c.TargetId;

        if (state.GetUnit(unitId, out UnitState unitAttacker) && state.GetUnit(targetUnitId, out UnitState unitTarget))
        {
            unitAttacker.moved = true;
            unitAttacker.attacked = true;

            switch (unitTarget.type)
            {
                case UnitType.PLAYER:
                case UnitType.MONSTER:
                    unitTarget.health -= new Random().Next(unitAttacker.minDmg, unitAttacker.maxDmg + 1);
                    if (unitTarget.health <= 0)
                    {
                        state.unitStates.Remove(unitId);
                        //actionsController.Execute(new DefeatRewardAction(gameState, gameController, actionsController, _unitsController, playerId, unitId, targetUnitId));
                    }
                    else
                    {
                        //actionsController.Execute(new AttackAction(state, popupsController, gameController, actionsController, _unitsController, "", unitTarget.UnitId, unitAttacker.UnitId));
                    }
                    break;
                case UnitType.DROP:
                    break;
                case UnitType.DOORS:
                    break;
                case UnitType.CHEST:
                    var backpack = state.GetCurrentPlayerBackpack();
                    backpack.AddItem(new ItemState()
                    {
                        itemBaseId = new Random().Next(1, 4)
                    });
                    break;
                default:
                    break;
            }

        }
    }
}
