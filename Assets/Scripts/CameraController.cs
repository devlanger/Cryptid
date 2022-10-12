using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 lastPanPosition;
    private int panFingerId;
    public Camera cam;
    public float groundZ = 0;

    private int currentIndex;
    private bool wasZoomingLastFrame;

    private Vector3 camPos;

    Plane ground;

    private void Awake()
    {
        ground = new Plane(Vector3.up, new Vector3(0, 0, groundZ));
        camPos = transform.position;
    }

    private void Update()
    {
        transform.position = camPos;
        //transform.position = Vector3.Lerp(transform.position, camPos, 5 * Time.deltaTime);
    }

    public void UpdateCamera()
    {
        if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
        {
            HandleTouch();
        }
        else
        {
            HandleMouse();
        }
    }

    public void PanCamera(Vector3 newPanPosition)
    {
        Vector3 direction = lastPanPosition - GetWorldPosition(newPanPosition);
        camPos += direction;
    }

    void HandleMouse()
    {
        // On mouse down, capture it's position.
        // Otherwise, if the mouse is still down, pan the camera.
        if (Input.GetMouseButtonDown(0))
        {
            lastPanPosition = GetWorldPosition(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            PanCamera(Input.mousePosition);
        }

        // Check for scrolling to zoom the camera
        float scroll = Input.GetAxis("Mouse ScrollWheel") * 10;
        Zoom(scroll);
    }

    void HandleTouch()
    {
        switch (Input.touchCount)
        {

            case 1: // Panning
                wasZoomingLastFrame = false;

                // If the touch began, capture its position and its finger ID.
                // Otherwise, if the finger ID of the touch doesn't match, skip it.
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    lastPanPosition = GetWorldPosition(touch.position);
                    panFingerId = touch.fingerId;
                }
                else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved)
                {
                    PanCamera(touch.position);
                }
                break;

            case 2: // Zooming
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                Zoom(difference * 0.02f);
                touch = Input.GetTouch(0);
                lastPanPosition = GetWorldPosition(touch.position);
                panFingerId = touch.fingerId;
                break;

            default:
                wasZoomingLastFrame = false;
                break;
        }
    }

    private void Zoom(float val)
    {
        camPos += cam.transform.forward * val;
        //cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - val, 25, 80);
    }

    private Vector3 GetWorldPosition(Vector2 position)
    {
        Ray mousePos = cam.ScreenPointToRay(position);
        float distance;
        ground.Raycast(mousePos, out distance);
        return mousePos.GetPoint(distance);
    }
}