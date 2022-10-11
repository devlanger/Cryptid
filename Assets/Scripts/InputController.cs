using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : Singleton<InputController>
{
    private Unit selectionUnit;

    [SerializeField] private GameObject selectionIndicator;
    [SerializeField] private CameraController cameraController;

    void Update()
    {
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        cameraController.UpdateCamera();

        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(r, out var hit))
        {
            if(hit.collider == null)
            {
                return;
            }

            var unit = hit.collider.GetComponent<Unit>();
            if(unit != null)
            {
                selectionUnit = unit;
                selectionIndicator.SetActive(true);
                Vector3 pos = selectionUnit.transform.position;
                pos.y = selectionIndicator.transform.position.y;

                selectionIndicator.transform.position = pos;
            }
            else
            {
                if (selectionUnit != null)
                {
                    selectionIndicator.SetActive(false);

                    Vector2Int pos = new Vector2Int((int)hit.point.x, (int)hit.point.z);
                    var gState = GameController.Instance.gameState;
                    ActionsController.Instance.Execute(new MoveAction(gState, gState.CurrentPlayerId, selectionUnit.state.unitId, pos));
                    selectionUnit = null;
                }
            }
        }
    }
}
