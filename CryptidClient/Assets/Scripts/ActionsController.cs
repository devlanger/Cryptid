using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/*public class ActionsController
{
    //public Stack<GameAction> actions = new Stack<GameAction>();
    private GameController gameController;

    [Inject]
    public void Construct(GameController gameController)
    {
        this.gameController = gameController;
    }

    public void Execute(GameAction action)
    {
        if(!action.CanExecute(gameController.gameState))
        {
            return;
        }

        action.Execute(gameController.gameState);
        actions.Push(action);
    }
}
*/