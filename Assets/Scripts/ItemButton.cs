using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public Image icon;

    private void Awake()
    {
        Reset();
    }

    public void Fill(ItemScriptable itemData)
    {
        if(itemData == null)
        {
            Reset();
            return;
        }

        icon.enabled = true;
        icon.sprite = itemData.icon;
    }

    public void Reset()
    {
        icon.enabled = false;
    }
}
