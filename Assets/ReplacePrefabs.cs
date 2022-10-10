using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplacePrefabs : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private List<GameObject> items;

#if UNITY_EDITOR
    [Button]
    public void Replace()
    {
        foreach (var item in items)
        {
            GameObject inst = PrefabUtility.InstantiatePrefab(prefab, SceneManager.GetActiveScene()) as GameObject;
            inst.transform.position = item.transform.position;
            inst.transform.rotation = item.transform.rotation;
            DestroyImmediate(item.gameObject);
        }

        items.Clear();
    }
#endif
}
