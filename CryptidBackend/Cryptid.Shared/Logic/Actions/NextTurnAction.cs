using System;
using System.Collections.Generic;
using System.Text;

namespace Cryptid.Shared.Logic.Actions
{
    public class NextTurnAction : GameAction
    {
        public class Command : CommandBase
        {

        }

        public NextTurnAction(GameState gameState) : base(gameState)
        {
        }

        public override ActionsController.Result CanExecute(GameState state, CommandBase command)
        {
            if (IsServer && state.CurrentPlayerId != command.PlayerId)
            {
                return ActionsController.Result.Failure("False");
            }

            return base.CanExecute(state, command);
        }

        public override void Execute(GameState state, CommandBase command)
        {
            state.FinishTurn();
        }
    }
}
