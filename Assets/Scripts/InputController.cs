using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : Singleton<InputController>
{
    private Unit selectionUnit;

    [SerializeField] private GameObject selectionIndicator;
    [SerializeField] private GameObject movementIndicator;
    [SerializeField] private CameraController cameraController;

    private void Awake()
    {
        GameController.Instance.OnFinishedTurn += Instance_OnFinishedTurn;
    }

    private void Instance_OnFinishedTurn(GameState obj)
    {
        DeselectUnit();
    }

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
                if (selectionUnit != null)
                {
                    var gState = GameController.Instance.gameState;
                    ActionsController.Instance.Execute(new AttackAction(gState, gState.CurrentPlayerId, selectionUnit.UnitId, unit.UnitId));
                }
                else
                {
                    selectionUnit = unit;
                    selectionIndicator.SetActive(true);
                    if (selectionUnit.IsMine && !selectionUnit.state.moved)
                    {
                        movementIndicator.SetActive(true);
                    }
                    else
                    {
                        movementIndicator.SetActive(false);
                    }
                    Vector3 pos = selectionUnit.transform.position;
                    pos.y = selectionIndicator.transform.position.y;

                    selectionIndicator.transform.position = pos;
                    movementIndicator.transform.position = pos;
                }
            }
            else
            {
                if (selectionUnit != null)
                {
                    Vector2Int pos = new Vector2Int((int)hit.point.x, (int)hit.point.z);
                    var gState = GameController.Instance.gameState;
                    ActionsController.Instance.Execute(new MoveAction(gState, gState.CurrentPlayerId, selectionUnit.state.unitId, pos));
                    DeselectUnit();
                }
            }
        }
    }

    private void DeselectUnit()
    {
        selectionIndicator.SetActive(false);
        movementIndicator.SetActive(false);

        selectionUnit = null;
    }
}
