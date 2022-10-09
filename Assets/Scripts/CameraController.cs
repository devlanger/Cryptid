using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private float speed = 50;

    private void LateUpdate()
    {
        if(!Input.GetMouseButton(0))
        {
            return;
        }

        Vector3 dir = Vector3.zero;

#if UNITY_EDITOR || UNITY_STANDALONE
        dir = camera.transform.rotation * new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"));
        dir.y = 0;
        camera.transform.position -= dir.normalized * speed * Time.deltaTime;
#elif !UNITY_EDITOR || UNITY_ANDROID
        Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
        dir = camera.transform.rotation * new Vector3(touchDeltaPosition.x, 0, touchDeltaPosition.y);
        dir.y = 0;
        camera.transform.position -= dir.normalized * speed * Time.deltaTime;
#else

#endif
    }
}
