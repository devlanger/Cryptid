using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;
using Zenject.Asteroids;

public class BackpackUI : ViewUI
{
    private ItemButton[] buttons;
    private GameController gameController;
    private DatabaseController databaseController;

    [Inject]
    public void Construct(GameController gameController, DatabaseController databaseController)
    {
        this.gameController = gameController;
        this.databaseController = databaseController;
    }

    private void Awake()
    {
        buttons = transform.GetComponentsInChildren<ItemButton>(true);
        foreach (var item in buttons)
        {
            item.Reset();
        }

        gameController.OnGameBegun += Instance_OnGameBegun;
    }

    private void Instance_OnGameBegun(GameState obj)
    {
        gameController.OnFinishedTurn += Instance_OnFinishedTurn;
    }

    private void Instance_OnFinishedTurn(GameState obj)
    {
        foreach (var item in buttons)
        {
            item.Reset();
        }

        foreach (var item in obj.GetCurrentPlayerBackpack().GetItems())
        {
            if(databaseController.Manager.GetItem(item.Value.itemBaseId, out ItemScriptable itemData))
            {
                buttons[item.Key].Fill(itemData);
            }
        }
    }
}
