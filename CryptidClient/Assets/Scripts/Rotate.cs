using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public bool local;
    public Vector3 speed;

    private void Update()
    {
        if(local)
        {
            transform.localEulerAngles += speed * Time.deltaTime;
        }
        else
        {
            transform.eulerAngles += speed * Time.deltaTime;
        }
    }
}
