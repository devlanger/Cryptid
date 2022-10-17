using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MoveAction : GameAction
{
    private UnitsController _unitsController;

    public override bool CanExecute(GameState state)
    {
        if (_unitsController.GetUnit(unitId, out Unit unit))
        {
            if(state.CurrentPlayerId != unit.state.ownerId)
            {
                return false;
            }

            if(Vector3.Distance(new Vector3(pos.x, 0, pos.y), unit.transform.position) > 6)
            {
                return false;
            }

            if(unit.state.moved)
            {
                return false;
            }
            
            //if(unit.state.moved)
            //{
            //    return false;
            //}
        }


        return base.CanExecute(state);
    }

    private Vector2Int pos;
    private string unitId;

    [Inject]
    public void Initialize(UnitsController _unitsController)
    {
        this._unitsController = _unitsController;
    }

    public MoveAction(GameState gameState, UnitsController unitsController, string playerId, string unitId, Vector2Int pos) : base(gameState, playerId)
    {
        _unitsController = unitsController;
        this.pos = pos;
        this.unitId = unitId;
    }

    public override void Execute(GameState state)
    {
        if(_unitsController.GetUnit(unitId, out Unit unit))
        {
            unit.state.moved = true;
            unit.state.posX = pos.x;
            unit.state.posZ = pos.y;
            unit.transform.DOMove(new Vector3(pos.x, 1, pos.y), 0.25f);
            if(UnityEngine.Random.Range(0, 2) == 0)
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
