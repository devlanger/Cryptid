using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsController : Singleton<ActionsController>
{
    public Stack<GameAction> actions;

    public void Execute(GameAction action)
    {
        if(!action.CanExecute(GameController.Instance.gameState))
        {
            return;
        }

        action.Execute(GameController.Instance.gameState);
        actions.Push(action);
    }
}
