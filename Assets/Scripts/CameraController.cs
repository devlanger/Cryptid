using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 touchStart;
    public Camera cam;
    public float groundZ = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = GetWorldPosition(groundZ);
        }

        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            Zoom(difference * 0.02f);
        }
        else if(Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - GetWorldPosition(groundZ);
            cam.transform.position += direction;
        }

        Zoom(Input.GetAxis("Mouse ScrollWheel") * 5);
    }

    private void Zoom(float val)
    {
        cam.transform.position += cam.transform.forward * val;
    }

    private Vector3 GetWorldPosition(float z)
    {
        Ray mousePos = cam.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, new Vector3(0, 0, z));
        float distance;
        ground.Raycast(mousePos, out distance);
        return mousePos.GetPoint(distance);
    }
}