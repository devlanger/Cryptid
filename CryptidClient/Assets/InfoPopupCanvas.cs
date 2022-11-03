using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InfoPopupCanvas : ViewUI
{
    [SerializeField] private GenericInfoPopup popupPrefab;
    [SerializeField] private Transform container;
    
    private PopupsController controller;

    [Inject]
    public void Construct(PopupsController controller)
    {
        this.controller = controller;

        controller.OnPopupShow += ShowPopup;
    }

    public void ShowPopup(PopupSettings settings)
    {
        var inst = Instantiate(popupPrefab, container.transform);
        inst.Fill(settings);
        Destroy(inst.gameObject, 1);
    }
}

public class PopupSettings
{
    public string text;
    public Color color;
}