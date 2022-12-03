using Cryptid.Logic;
using System;
using System.Collections;
using System.Collections.Generic;

public class CommandBase
{
    public byte id;
}

public class MoveAction : GameAction
{
    public class Command : CommandBase
    {
        public string unitId;
        public int posX;
        public int posZ;
    }

    public Command command;

    public override bool CanExecute(GameState state, CommandBase command)
    {
        if (state.GetUnit(unitId, out UnitState unit))
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

    private string unitId;

    public MoveAction(GameState gameState, Command command, string playerId) : base(gameState, playerId)
    {
        this.gameState = gameState;
        this.command = command;
    }

    public override void Execute(GameState state, CommandBase command)
    {
        Command c = command as Command;
        if(state.GetUnit(unitId, out UnitState unit))
        {
            unit.moved = true;
            unit.posX = c.posX;
            unit.posZ = c.posZ;
        }
    }
}
