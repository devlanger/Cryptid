using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DatabaseController
{
    public DatabasesManager Manager;

    [Inject]
    public void Construct(DatabasesManager manager)
    {
        this.Manager = manager;
    }
}
