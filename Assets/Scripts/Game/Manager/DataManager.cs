using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPS;

public class DataManager : MonoSingleton<DataManager>
{
    public static DatasaveManager Save;
    public static DatabaseManager Base;
    public static DataliveManager Live;

    public void Init()
    {
        DataliveManager.S.Init(transform);
        DatabaseManager.S.Init(transform);
        DatasaveManager.S.Init(transform);
    }
}
