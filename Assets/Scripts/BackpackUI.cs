using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BackpackUI : ViewUI
{
    private ItemButton[] buttons;

    private void Awake()
    {
        buttons = transform.GetComponentsInChildren<ItemButton>(true);
        foreach (var item in buttons)
        {
            item.Reset();
        }

        GameController.Instance.OnGameBegun += Instance_OnGameBegun;
    }

    private void Instance_OnGameBegun(GameState obj)
    {
        GameController.Instance.OnFinishedTurn += Instance_OnFinishedTurn;
    }

    private void Instance_OnFinishedTurn(GameState obj)
    {
        foreach (var item in buttons)
        {
            item.Reset();
        }

        foreach (var item in obj.GetCurrentPlayerBackpack().GetItems())
        {
            if(DatabaseController.Instance.Manager.GetItem(item.Value.itemBaseId, out ItemScriptable itemData))
            {
                buttons[item.Key].Fill(itemData);
            }
        }
    }
}
