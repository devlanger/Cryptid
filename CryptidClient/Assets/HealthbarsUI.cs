using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class HealthbarsUI : MonoBehaviour
{
    [SerializeField] private UnitHealthbar healthbarPrefab;

    private Dictionary<string, UnitHealthbar> healthbars = new Dictionary<string, UnitHealthbar>();

    private UnitsController _unitsController;

    [Inject]
    public void Construct(UnitsController _unitsController) 
    {
        this._unitsController = _unitsController;
    }

    private void Awake()
    {
        _unitsController.OnUnitSpawn += Instance_OnUnitSpawn;
        _unitsController.OnUnitDespawn += Instance_OnUnitDespawn;
    }

    private void Update()
    {
        foreach (var item in healthbars.ToList())
        {
            item.Value?.Refresh();
        }
    }

    private void Instance_OnUnitSpawn(Unit obj)
    {
        if(obj.state == null)
        {
            return;
        }

        switch (obj.state.type)
        {
            case UnitType.PLAYER:
            case UnitType.MONSTER:
                var inst = Instantiate(healthbarPrefab, transform);
                healthbars.Add(obj.UnitId, inst);
                inst.Fill(obj);
                break;
        }
    }

    private void Instance_OnUnitDespawn(Unit unit)
    {
        if(unit == null)
        {
            return;
        }

        if (healthbars.ContainsKey(unit.UnitId))
        {
            Destroy(healthbars[unit.UnitId].gameObject);
            healthbars.Remove(unit.UnitId);
        }
    }
}
