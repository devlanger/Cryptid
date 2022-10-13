using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthbarsUI : MonoBehaviour
{
    [SerializeField] private UnitHealthbar healthbarPrefab;

    private Dictionary<string, UnitHealthbar> healthbars = new Dictionary<string, UnitHealthbar>();

    private void Awake()
    {
        UnitsController.Instance.OnUnitSpawn += Instance_OnUnitSpawn;
        UnitsController.Instance.OnUnitDespawn += Instance_OnUnitDespawn;
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
