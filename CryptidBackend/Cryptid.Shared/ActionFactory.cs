using Cryptid.Shared.Logic.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cryptid.Shared
{
    public class ActionFactory
    {
        public static GameAction CreateActionFromCommand(GameState state, CommandBase command)
        {
            GameAction action = null;

            switch (command.id)
            {
                case CommandType.MOVE:
                    action = new MoveAction(state, "");
                    break;
                case CommandType.ATTACK_TARGET:
                    action = new AttackAction(state);
                    break;
                case CommandType.NEXT_TURN:
                    action = new NextTurnAction(state);
                    break;
            }


            return action;
        }
    }
}
