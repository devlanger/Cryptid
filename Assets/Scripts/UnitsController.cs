using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsController : Singleton<UnitsController>
{
    public Unit prefab;

    public Dictionary<string, Unit> units = new Dictionary<string, Unit>();

    public void SpawnUnit(UnitSpawnSettings settings)
    {
        var inst = Instantiate(prefab);
        inst.transform.position = new Vector3(settings.spawnPoint.x, 1, settings.spawnPoint.y);
        inst.state.unitId = System.Guid.NewGuid().ToString();
        inst.state.ownerId = settings.ownerId;
        inst.state.type = settings.type;
        inst.state.posX = settings.spawnPoint.x;
        inst.state.posZ = settings.spawnPoint.y;

        units.Add(inst.state.unitId, inst);
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