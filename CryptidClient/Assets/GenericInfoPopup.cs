using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenericInfoPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void Fill(PopupSettings settings)
    {
        text.text = settings.text;
        text.color = settings.color;
    }
}
