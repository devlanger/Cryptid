using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuStateController : MonoBehaviour
{
    public byte state;

    public event Action<byte> OnMenuStateChanged;

    public void ChangeMenuState(byte state)
    {
        this.state = state;
        OnMenuStateChanged?.Invoke(state);
    }
}
