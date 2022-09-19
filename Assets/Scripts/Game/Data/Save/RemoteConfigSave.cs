using BayatGames.SaveGameFree;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RemoteConfigSave: IDataSave
{
    private string key = "RemoteConfig";

    [ShowInInspector] public Dictionary<string, string> Configs = new Dictionary<string, string>();

    public RemoteConfigSave()
    {
        Configs.Add(RemoteConfigKey.Demo, "1");
    }

    public void Fix()
    {

    }

    public void Save()
    {
        SaveGame.Save(key, this);
    }
}

public class RemoteConfigKey
{
    public const string Demo = "ab_demo";
}
