using Cryptid.Shared;
using Cryptid.Shared.Logic.Actions;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using static UnityEngine.UI.CanvasScaler;

public class InputController : IInitializable, ITickable
{
    private Unit selectionUnit;
    private PopupsController popupsController;
    private GameController gameController;
    private ActionsController actionsController;
    private UnitsController unitsController;
    private ConnectionController connectionController => ConnectionController.Instance;
    private CameraController cameraController;

    private GameObject selectionIndicatorPrefab;
    private GameObject movementIndicatorPrefab;

    private GameObject selectionIndicator;
    private GameObject movementIndicator;
    private CommandExecutor commandExecutor;

    public event Action<Unit> UnitSelected;

    [Inject]
    public void Construct(
        UnitsController unitsController, 
        GameController gameController, 
        ActionsController actionsController, 
        PopupsController popupsController,
        CommandExecutor commandExecutor)
    {
        this.popupsController = popupsController;
        this.gameController = gameController;
        this.actionsController = actionsController;
        this.unitsController = unitsController;
        this.commandExecutor = commandExecutor;
    }

    public InputController(GameObject selectionIndicator, GameObject movementIndicator)
    {
        this.selectionIndicatorPrefab = selectionIndicator;
        this.movementIndicatorPrefab = movementIndicator;
    }

    public void Initialize()
    {
        this.selectionIndicator = GameObject.Instantiate(selectionIndicatorPrefab);
        this.movementIndicator = GameObject.Instantiate(movementIndicatorPrefab);
        cameraController = GameObject.FindObjectOfType<CameraController>();
        gameController.OnFinishedTurn += Instance_OnFinishedTurn;
    }

    private void Instance_OnFinishedTurn(GameState obj)
    {
        DeselectUnit();
    }

    public void Tick()
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
                    commandExecutor.ExecuteCommand(gameController.gameState, new AttackAction.Command
                    {
                        id = CommandType.ATTACK_TARGET,
                        gameId = gameController.CurrentGameId,
                        UnitId = selectionUnit.UnitId,
                        TargetId = unit.UnitId,
                    });
                }
                else
                {
                    selectionUnit = unit;
                    selectionIndicator.SetActive(true);
                    if (selectionUnit.IsMine && !selectionUnit.state.moved)
                    {
                        if(selectionUnit.state != null && selectionUnit.state.ownerId == gameController.CurrentUserId)
                        {
                            movementIndicator.SetActive(true);
                        }
                        else
                        {
                            movementIndicator.SetActive(false);
                        }
                    }
                    else
                    {
                        movementIndicator.SetActive(false);
                    }
                    Vector3 pos = selectionUnit.transform.position;
                    pos.y = selectionIndicator.transform.position.y;

                    selectionIndicator.transform.position = pos;
                    movementIndicator.transform.position = pos;

                    UnitSelected?.Invoke(unit);
                }
            }
            else
            {
                if (selectionUnit != null)
                {
                    Vector2Int pos = new Vector2Int((int)hit.point.x, (int)hit.point.z);
                    commandExecutor.ExecuteCommand(gameController.gameState, new MoveAction.Command
                    {
                        id = CommandType.MOVE,
                        gameId = gameController.CurrentGameId,
                        unitId = selectionUnit.UnitId,
                        posX = pos.x,
                        posZ = pos.y
                    });
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
        UnitSelected?.Invoke(null);
    }
}
