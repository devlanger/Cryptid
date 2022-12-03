using System.Collections;
using System.Collections.Generic;

public abstract class GameAction
{
    protected GameState gameState;
    protected string playerId;

    protected GameAction(GameState gameState, string playerId)
    {
        this.gameState = gameState;
        this.playerId = playerId;
    }

    public virtual bool CanExecute(GameState state) => true;
    public abstract void Execute(GameState state);
}
