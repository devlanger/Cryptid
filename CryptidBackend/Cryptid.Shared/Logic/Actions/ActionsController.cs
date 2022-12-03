using System.Collections;
using System.Collections.Generic;

public class ActionsController
{
    public void Execute(GameState state, GameAction action, CommandBase command)
    {
        if(!action.CanExecute(state, command))
        {
            return;
        }

        action.Execute(state, command);
    }
}
