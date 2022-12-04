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
            }


            return action;
        }
    }
}
