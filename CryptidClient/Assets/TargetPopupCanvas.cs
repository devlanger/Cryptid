using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TargetPopupCanvas : ViewUI
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI lvlText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider slider;

    private InputController inputController;
    private DatabaseController databaseController;

    [Inject]
    public void Construct(InputController inputController, DatabaseController databaseController)
    {
        this.inputController = inputController;
        this.databaseController = databaseController;

        inputController.UnitSelected += InputController_UnitSelected;
    }

    private void InputController_UnitSelected(Unit unit)
    {
        if(unit == null)
        {
            Deactivate();
            return;
        }

        if(databaseController.Manager.GetUnit(1, out var unitScriptable))
        {
            Fill(unit, unitScriptable);
            Activate();
        }
    }

    private void Fill(Unit unit, UnitScriptable unitScriptable)
    {
        nameText.SetText($"{unitScriptable.name}");
        lvlText.SetText($"Lv. {unitScriptable.level}");
        healthText.SetText($"{unit.state.health}/{unitScriptable.health}");
        slider.maxValue = unitScriptable.health;
        slider.value = unit.state.health;
    }
}
