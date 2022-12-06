using Cryptid.Logic;
using Cryptid.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public enum CommandType : byte
{
    MOVE = 1,
    NEXT_TURN = 2,
    ATTACK_TARGET = 3,
}

public class CommandBase
{
    public CommandType id;
    public string gameId;
    public string PlayerId { get; set; }
}

public class MoveAction : GameAction
{
    public class Command : CommandBase
    {
        public string unitId;
        public int posX;
        public int posZ;
    }

    public override ActionsController.Result CanExecute(GameState state, CommandBase command)
    {
        var c = command as MoveAction.Command;

        if(state.CurrentPlayerId != command.PlayerId)
        {
            return ActionsController.Result.Failure("Not my turn");
        }

        if (state.GetUnit(c.unitId, out UnitState unit))
        {
            if(state.CurrentPlayerId != unit.ownerId)
            {
                return ActionsController.Result.Failure("Not my unit");
            }

            //if(Vector3.Distance(new Vector3(pos.x, 0, pos.y), unit.transform.position) > 6)
            //{
            //    return false;
            //}

            if(unit.moved)
            {
                return ActionsController.Result.Failure("Unit already moved");
            }
        }
        else
        {
            return ActionsController.Result.Failure("Unit is missing");
        }


        return base.CanExecute(state, command);
    }

    public MoveAction(GameState gameState, string playerId) : base(gameState)
    {
        this.gameState = gameState;
    }

    public override void Execute(GameState state, CommandBase command)
    {
        Command c = command as MoveAction.Command;
        if(state.GetUnit(c.unitId, out UnitState unit))
        {
            unit.moved = true;
            unit.posX = c.posX;
            unit.posZ = c.posZ;
        }
    }
}
