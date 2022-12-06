using System.Collections;
using System.Collections.Generic;

public abstract class CommandHandlerBase
{
    public abstract void Handle(GameState gameState, CommandBase commandBase);
}
