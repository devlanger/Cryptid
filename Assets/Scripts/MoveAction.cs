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

            if(Vector3.Distance(new Vector3(pos.x, 0, pos.y), unit.transform.position) > 5)
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
