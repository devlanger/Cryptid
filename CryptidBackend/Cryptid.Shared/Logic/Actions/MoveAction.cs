using Cryptid.Logic;
using Cryptid.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public enum CommandType : byte
{
    MOVE = 1,
}

public class CommandBase
{
    public CommandType id;
    public string gameId;
}

public class MoveAction : GameAction
{
    public class Command : CommandBase
    {
        public string unitId;
        public int posX;
        public int posZ;
    }

    public override bool CanExecute(GameState state, CommandBase command)
    {
        var c = command as MoveAction.Command;

        if (state.GetUnit(c.unitId, out UnitState unit))
        {
            if(state.CurrentPlayerId != unit.ownerId)
            {
                return false;
            }

            //if(Vector3.Distance(new Vector3(pos.x, 0, pos.y), unit.transform.position) > 6)
            //{
            //    return false;
            //}

            if(unit.moved)
            {
                return false;
            }
        }


        return base.CanExecute(state, command);
    }

    public MoveAction(GameState gameState, string playerId) : base(gameState, playerId)
    {
        this.gameState = gameState;
    }

    public override void Execute(GameState state, CommandBase command)
    {
        Command c = command as MoveAction.Command;
        if(state.GetUnit(c.unitId, out UnitState unit))
        {
            //unit.moved = true;
            unit.posX = c.posX;
            unit.posZ = c.posZ;
        }
    }
}
