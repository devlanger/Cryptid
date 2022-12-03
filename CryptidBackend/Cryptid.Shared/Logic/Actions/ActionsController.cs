using System.Collections;
using System.Collections.Generic;

public class ActionsController
{
    //public Stack<GameAction> actions = new Stack<GameAction>();

    public void Execute(GameState state, GameAction action)
    {
        if(!action.CanExecute(state))
        {
            return;
        }

        action.Execute(state);
        //actions.Push(action);
    }
}
