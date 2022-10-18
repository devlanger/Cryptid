using System;
using System.Collections;
using System.Collections.Generic;
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

    public void SpawnUnit(UnitSpawnSettings settings)
    {
        var inst = Unit.Instantiate(GetPrefab(settings));
        inst.Construct(gameController);
        inst.transform.position = new Vector3(settings.spawnPoint.x, 1, settings.spawnPoint.y);
        
        var state = new UnitState();
        state.unitId = System.Guid.NewGuid().ToString();
        state.ownerId = settings.ownerId;
        state.type = settings.type;
        state.posX = settings.spawnPoint.x;
        state.posZ = settings.spawnPoint.y;
        state.health = 10;
        state.minDmg = 1;
        state.maxDmg = 2;

        if (state.type == UnitType.MONSTER)
        {
            if(databaseController.Manager.GetUnit(settings.baseId, out var unitData))
            {
                state.health = unitData.health;
                state.minDmg = unitData.minDamage;
                state.maxDmg = unitData.maxDamage;
            }
        }

        inst.UnitId = state.unitId;
        gameController.gameState.unitStates.Add(state.unitId, state);
        units.Add(state.unitId, inst);

        OnUnitSpawn?.Invoke(inst);
    }

    private Unit GetPrefab(UnitSpawnSettings settings)
    {
        if (prefabs.ContainsKey(settings.type))
        {
            return prefabs[settings.type];
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

public enum UnitType
{
    PLAYER = 1,
    MONSTER = 2,
    DROP = 3,
    DOORS = 4,
    CHEST = 5,
}

public class UnitSpawnSettings
{
    public string ownerId;
    public int baseId;
    public UnitType type;
    public Vector2Int spawnPoint;
}