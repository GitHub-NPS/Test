using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPS;

public class DatabaseManager : MonoSingleton<DatabaseManager>
{
    public GeneralTable General = new GeneralTable();

    private void Start()
    {
        
    }

    public void Init(Transform parent = null)
    {
        DataManager.Base = this;
        if (parent) transform.SetParent(parent);

        General.GetDatabase();
    }
}
