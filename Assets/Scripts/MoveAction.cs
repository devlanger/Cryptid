using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : GameAction
{
    public override bool CanExecute(GameState state)
    {
        if (UnitsController.Instance.GetUnit(unitId, out Unit unit))
        {
            if(state.CurrentPlayerId != unit.state.ownerId)
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

    public MoveAction(GameState gameState, string playerId, string unitId, Vector2Int pos) : base(gameState, playerId)
    {
        this.pos = pos;
        this.unitId = unitId;
    }

    public override void Execute(GameState state)
    {
        if(UnitsController.Instance.GetUnit(unitId, out Unit unit))
        {
            unit.transform.position = new Vector3(pos.x, 1, pos.y);
            unit.state.moved = true;
        }
    }
}
