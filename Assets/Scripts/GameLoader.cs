using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class GameLoader : MonoInstaller
{
    public UnitObjectsScriptable units;
    public DatabasesManager database;

    public override void InstallBindings()
    {
        Container.BindInstance(units);
        Container.BindInstance(database);

        Container.Bind<ActionsController>().AsSingle().NonLazy();
        Container.Bind<DatabaseController>().AsSingle().NonLazy();
        Container.Bind<UnitsController>().AsSingle().NonLazy();
        Container.Bind<GameController>().FromInstance(FindObjectOfType<GameController>());
    }
}

[System.Serializable]
public class GameStartSettings
{
    public int players = 3;
}