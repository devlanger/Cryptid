using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentList : MonoBehaviour
{
    public Transform parent;
    public GameObject prefab;

    private List<GameObject> items = new List<GameObject>();

    private void Awake()
    {
        foreach (Transform item in parent)
        {
            if (item != parent)
            {
                Destroy(item.gameObject);
            }
        }
    }

    public T AddToList<T>() where T : MonoBehaviour
    {
        var inst = Instantiate(prefab.GetComponent<T>(), parent);
        items.Add(inst.gameObject);
        return inst; 
    }

    public void ClearList()
    {
        foreach (var item in items)
        {
            Destroy(item.gameObject);
        }

        items.Clear();
    }
}
