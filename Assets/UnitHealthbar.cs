using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHealthbar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private Unit unit;

    public void Fill(Unit obj)
    {
        this.unit = obj;
        slider.maxValue = unit.state.health;
        Refresh();
    }

    public void Refresh()
    {
        if(unit == null)
        {
            return;
        }

        slider.value = unit.state.health;
        transform.position = Camera.main.WorldToScreenPoint(unit.transform.position + Vector3.up);
    }
}
