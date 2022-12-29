using System;
using Cryptid.Shared;
using Zenject;
using static UnityEngine.UI.CanvasScaler;
using Zenject.Asteroids;
using UnityEngine;

public class CommandExecutor
{
    private ActionsController actionsController;
    private UnitsController unitsController;
    private GameController gameController;

    public string CurrentGameId { get; set; }

    [Inject]
    public void Construct(
        GameController gameController,
        UnitsController _unitsController,
        ActionsController actionsController)
    {
        this.unitsController = _unitsController;
        this.actionsController = actionsController;
        this.gameController = gameController;
    }

	public void ExecuteCommand(GameState state, CommandBase command)
	{
		if(state.IsOnline)
		{
            ConnectionController.Instance.SendActionCommand(CommandReader.WriteCommandToBytes(command));
        }
		else
		{
            RunCommand(state, command);
		}
	}

	public void RunCommand(GameState state, CommandBase command)
	{
        var action = ActionFactory.CreateActionFromCommand(gameController.gameState, command);
        command.PlayerId = gameController.CurrentUserId;

        var result = actionsController.Execute(gameController.gameState, action, command);
        if (result.IsSuccess)
        {
            try
            {
                switch (command.id)
                {
                    case CommandType.MOVE:
                        new MoveCommandHandler(unitsController).Handle(gameController.gameState, command);
                        break;
                    case CommandType.NEXT_TURN:
                        new NextTurnCommandHandler(gameController).Handle(gameController.gameState, command);
                        break;
                    case CommandType.ATTACK_TARGET:
                        new AttackCommandHandler(unitsController).Handle(gameController.gameState, command);
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }
        else
        {
            Debug.LogError($"Command issue {command.id}: {result.Error}");
        }
    }
}

