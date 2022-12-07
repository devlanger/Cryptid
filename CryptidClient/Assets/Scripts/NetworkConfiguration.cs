using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkConfiguration : MonoBehaviour
{
    public static string API_URL => ConnectionController.Instance.Api;
}
