using System;
using System.Collections;
using System.Collections.Generic;

public class ActionsController
{
    public bool Execute(GameState state, GameAction action, CommandBase command)
    {
        if(!action.CanExecute(state, command))
        {
            return false;
        }

        action.Execute(state, command);
        return true;
    }
}
