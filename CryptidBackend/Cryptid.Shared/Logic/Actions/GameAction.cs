using System.Collections;
using System.Collections.Generic;

public abstract class GameAction
{
    protected GameState gameState;

    protected GameAction(GameState gameState)
    {
        this.gameState = gameState;
    }

    public virtual ActionsController.Result CanExecute(GameState state, CommandBase command) => ActionsController.Result.Success();
    public abstract void Execute(GameState state, CommandBase command);
}
