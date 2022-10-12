using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsController : Singleton<UnitsController>
{
    public Unit prefab;

    public Dictionary<string, Unit> units = new Dictionary<string, Unit>();

    private void Awake()
    {
        GameController.Instance.OnFinishedTurn += RefreshOutlines;
        GameController.Instance.OnFinishedTurn += ResetUnits;
        GameController.Instance.OnGameBegun += RefreshOutlines;
    }

    private void ResetUnits(GameState obj)
    {
        foreach (var item in obj.unitStates.Values)
        {
            item.moved = false;
            item.attacked = false;
        }
    }

    private void RefreshOutlines(GameState obj)
    {
        foreach (var item in units.Values)
        {
            if(item.state.ownerId == obj.CurrentPlayerId)
            {
                item.GetComponent<Outline>().enabled = true;
            }
            else
            {
                item.GetComponent<Outline>().enabled = false;
            }
        }
    }

    public void SpawnUnit(UnitSpawnSettings settings)
    {
        var inst = Instantiate(prefab);
        inst.transform.position = new Vector3(settings.spawnPoint.x, 1, settings.spawnPoint.y);
        
        var state = new UnitState();
        state.unitId = System.Guid.NewGuid().ToString();
        state.ownerId = settings.ownerId;
        state.type = settings.type;
        state.posX = settings.spawnPoint.x;
        state.posZ = settings.spawnPoint.y;

        inst.unitId = state.unitId;
        GameController.Instance.gameState.unitStates.Add(state.unitId, state);
        units.Add(state.unitId, inst);
    }

    public bool GetUnit(string unitId, out Unit unit)
    {
        return units.TryGetValue(unitId, out unit);
    }
}

public enum UnitType
{
    PLAYER = 1,
    MONSTER = 2,
    DROP = 3,
    DOORS = 4
}

public class UnitSpawnSettings
{
    public string ownerId;
    public UnitType type;
    public Vector2Int spawnPoint;
}