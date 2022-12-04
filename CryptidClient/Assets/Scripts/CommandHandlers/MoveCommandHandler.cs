using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

public class MoveCommandHandler : CommandHandlerBase
{
    private readonly UnitsController unitsController;

    [Inject]
    public MoveCommandHandler(UnitsController unitsController)
    {
        this.unitsController = unitsController;
    }

    public override void Handle(CommandBase commandBase)
    {
        var command = commandBase as MoveAction.Command;
        if(command == null)
        {
            return;
        }

        if(unitsController.GetUnit(command.unitId, out var unit))
        {
            unit.transform.DOMove(new UnityEngine.Vector3(command.posX, 1, command.posZ), 0.25f);
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                SoundsController.Instance.PlaySound(SoundId.MOVE_2);
            }
            else
            {
                SoundsController.Instance.PlaySound(SoundId.MOVE_1);
            }
        }
    }
}