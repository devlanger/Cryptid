using Cryptid.Shared.Logic.Actions;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

public class NextTurnCommandHandler : CommandHandlerBase
{
    [Inject]
    public NextTurnCommandHandler()
    {

    }

    public override void Handle(GameState gameState, CommandBase commandBase)
    {
        var command = commandBase as NextTurnAction.Command;
        if (commandBase == null)
        {
            return;
        }

        gameState.FinishTurn();
    }
}