using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.BuiltIn;
using UnityEngine;

public class MenuRouterUI : MonoBehaviour
{
    public List<RouterNode> list = new List<RouterNode>();

    public void GoToView(string routeName)
    {
        foreach (var item in list)
        {
            item.view?.Deactivate();
        }

        var v = list.Find(v => v.routeName == routeName);
        if(v != null)
        {
            v.view?.Activate();
        }
    }
}

[System.Serializable]
public class RouterNode
{
    public ViewUI view;
    public string routeName;
}
