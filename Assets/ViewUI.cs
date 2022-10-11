using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewUI : MonoBehaviour
{
    public Canvas canvas;

    public bool IsActive => canvas != null && canvas.enabled;

    public void Toggle() { if (IsActive) { Deactivate(); } else { Activate(); } }
    public void Activate() { canvas.enabled = true; }
    public void Deactivate() { canvas.enabled = false; }
}
