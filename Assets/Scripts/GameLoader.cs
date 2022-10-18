using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Zenject.Asteroids;

public class GameLoader : MonoInstaller
{
    public GameStartSettings gameStartSettings;
    public UnitObjectsScriptable units;
    public DatabasesManager database;

    public GameObject selectionIndicator;
    public GameObject movementIndicator;

    public override void InstallBindings()
    {
        Container.BindInstance(units);
        Container.BindInstance(database);
        Container.BindInstance(gameStartSettings);

        Container.Bind<ActionsController>().AsSingle();
        Container.Bind<DatabaseController>().AsSingle();
        Container.Bind<UnitsController>().AsSingle();
        
        Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
        Container.BindInterfacesAndSelfTo<InputController>().AsSingle().WithArguments(selectionIndicator, movementIndicator);
    }
}

[System.Serializable]
public class GameStartSettings
{
    public int players = 3;
}