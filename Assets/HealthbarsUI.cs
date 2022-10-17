using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class HealthbarsUI : MonoBehaviour
{
    [SerializeField] private UnitHealthbar healthbarPrefab;

    private Dictionary<string, UnitHealthbar> healthbars = new Dictionary<string, UnitHealthbar>();

    [Inject] private UnitsController _unitsController;

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

    private void Instance_OnUnitDespawn(Unit obj)
    {
        switch (obj.state.type)
        {
            case UnitType.PLAYER:
            case UnitType.MONSTER:
                Destroy(healthbars[obj.UnitId].gameObject);
                healthbars.Remove(obj.UnitId);
                break;
        }
    }
}
