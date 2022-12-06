using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using static UnityEngine.UI.CanvasScaler;
using Zenject.Asteroids;

public class AttackCommandHandler : CommandHandlerBase
{
    private readonly UnitsController unitsController;

    [Inject]
    public AttackCommandHandler(UnitsController unitsController)
    {
        this.unitsController = unitsController;
    }

    public override void Handle(GameState gameState, CommandBase commandBase)
    {
        var command = commandBase as AttackAction.Command;
        if(command == null)
        {
            return;
        }

        if(unitsController.GetUnit(command.UnitId, out var attacker) && unitsController.GetUnit(command.TargetId, out var target))
        {
            var s = DOTween.Sequence()
                .Append(attacker.transform.DOPunchPosition(target.transform.position - attacker.transform.position, 0.25f))
                .Join(target.transform.DOShakeScale(0.25f).SetDelay(0.125f));

            switch (target.state.type)
            {
                case UnitType.PLAYER:
                case UnitType.MONSTER:
                    SoundsController.Instance.PlaySound(SoundId.CLASH_1);
                    /*if (unitTarget.state.health <= 0)
                    {
                        s.onComplete += () =>
                        {
                            unitAttacker.StartCoroutine(x(unitTarget.UnitId));
                        };
                    }*/
                    break;
                case UnitType.DROP:
                    break;
                case UnitType.DOORS:
                    break;
                case UnitType.CHEST:
                    SoundsController.Instance.PlaySound(SoundId.LOOT_PICKUP);
                    /*var backpack = gameController.gameState.GetCurrentPlayerBackpack();
                    backpack.AddItem(new ItemState()
                    {
                        itemBaseId = UnityEngine.Random.Range(1, 4)
                    });
                    gameController.RaiseUpdateEvent();
                    unitTarget.Die();*/

                    break;
            }
        }
    }
}