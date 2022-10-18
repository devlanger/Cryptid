using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupsController
{
    public event Action<PopupSettings> OnPopupShow;
         
    public void ShowPopup(PopupSettings settings)
    {
        OnPopupShow?.Invoke(settings);
    }
}
