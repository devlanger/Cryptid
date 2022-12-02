using System;
using System.Collections;
using System.Collections.Generic;
using Cryptid.Logic;
using UnityEngine;
using Zenject;

public class UnitsController
{
    private UnitObjectsScriptable _unitsScriptable;
    private GameController gameController;
    private DatabaseController databaseController;

    public Dictionary<UnitType, Unit> prefabs => _unitsScriptable.prefabs;
    public Dictionary<string, Unit> units = new Dictionary<string, Unit>();

    public event Action<Unit> OnUnitSpawn;
    public event Action<Unit> OnUnitDespawn;

    [Inject]
    public void Construct(DatabaseController databaseController, GameController gameController, UnitObjectsScriptable unitsScriptable)
    {
        this.gameController = gameController;
        this.databaseController = databaseController;
        this._unitsScriptable = unitsScriptable;
        gameController.OnFinishedTurn += RefreshOutlines;
        gameController.OnFinishedTurn += ResetUnits;
        gameController.OnGameBegun += RefreshOutlines;
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
            if (item.state.type == UnitType.PLAYER)
            {
                if (item.state.ownerId == obj.CurrentPlayerId)
                {
                    item.GetComponent<Outline>().enabled = true;
                }
                else
                {
                    item.GetComponent<Outline>().enabled = false;
                }
            }
        }
    }

    public void SpawnUnit(UnitState state)
    {
        var inst = Unit.Instantiate(GetPrefab(state.type));
        inst.Construct(gameController);
        inst.transform.position = new Vector3(state.posX, 1, state.posZ);
        inst.UnitId = state.unitId;
        
        units.Add(state.unitId, inst);
        OnUnitSpawn?.Invoke(inst);
    }

    private Unit GetPrefab(UnitType type)
    {
        if (prefabs.ContainsKey(type))
        {
            return prefabs[type];
        }

        throw new NullReferenceException("The object prefab is missing in the UnitController");
    }

    public bool GetUnit(string unitId, out Unit unit)
    {
        return units.TryGetValue(unitId, out unit);
    }

    public void RemoveUnit(string unitId)
    {
        OnUnitDespawn?.Invoke(units[unitId]);
        gameController.gameState.unitStates.Remove(unitId);
        GameObject.Destroy(units[unitId].gameObject);
        units.Remove(unitId);
    }
}
