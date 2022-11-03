using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : SerializedMonoBehaviour where T : MonoBehaviour
{
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<T>();
            }

            return instance;
        }
    }

    private static T instance;
}
