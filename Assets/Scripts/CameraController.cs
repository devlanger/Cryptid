using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 touchStart;
    public Camera cam;
    public float groundZ = 0;

    private int currentIndex;

    public void UpdateCamera()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            currentIndex = 0;
            touchStart = GetWorldPosition(Input.GetTouch(0).position, groundZ);
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

            if(touchOne.phase == TouchPhase.Ended || touchZero.phase == TouchPhase.Ended)
            {
                currentIndex = 0;
                touchStart = GetWorldPosition(Input.GetTouch(0).position, groundZ);
            }
            Zoom(difference * 0.02f);
        }

        if(Input.touchCount != 0)
        {
            Vector3 direction = touchStart - GetWorldPosition(Input.GetTouch(0).position, groundZ);
            cam.transform.position += direction;
        }

        Zoom(Input.GetAxis("Mouse ScrollWheel") * 5);
    }

    private void Zoom(float val)
    {
        cam.transform.position += cam.transform.forward * val;
    }

    private Vector3 GetWorldPosition(Vector2 position, float z)
    {
        Ray mousePos = cam.ScreenPointToRay(position);
        Plane ground = new Plane(Vector3.up, new Vector3(0, 0, z));
        float distance;
        ground.Raycast(mousePos, out distance);
        return mousePos.GetPoint(distance);
    }
}