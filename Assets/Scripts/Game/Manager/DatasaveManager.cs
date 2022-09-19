using BayatGames.SaveGameFree;
using BayatGames.SaveGameFree.Serializers;
using MEC;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using NPS;

public class DatasaveManager : MonoSingleton<DatasaveManager>
{
    #region General
    public GeneralSave General
    {
        get => general;
        set { general = value; }
    }

    [SerializeField] private GeneralSave general;
    #endregion

    #region User
    public UserSave User
    {
        get => user;
        set { user = value; }
    }

    [SerializeField] private UserSave user;
    #endregion

    #region RemoteConfig
    public RemoteConfigSave RemoteConfig
    {
        get => remoteConfig;
        set { remoteConfig = value; }
    }

    [SerializeField] private RemoteConfigSave remoteConfig;
    #endregion

    #region Tutorial
    public TutorialSave Tutorial
    {
        get => tutorial;
        set { tutorial = value; }
    }

    [SerializeField] private TutorialSave tutorial;
    #endregion

    private bool encode = true;
    private string password = "NPS";

    public void Init(Transform parent = null)
    {
        DataManager.Save = this;
        if (parent) transform.SetParent(parent);

#if DEVELOPMENT || UNITY_EDITOR
        encode = false;
#endif
        SaveGame.Encode = encode;
        SaveGame.EncodePassword = password;
        SaveGame.Serializer = new SaveGameJsonSerializer();
        Load();
        FixData();

        this.PostEvent(EventID.LoadSuccess);
    }

    private void FixData()
    {
        remoteConfig.Fix();
        general.Fix();
        tutorial.Fix();
        user.Fix();
    }

    //private void OnApplicationPause(bool pause)
    //{
    //    Debug.Log("Pause: " + pause);
    //    if (pause) Save();
    //}

    private void OnApplicationQuit()
    {
        Debug.Log("Quit");
        Save();
    }

    private void Save()
    {
        remoteConfig.Save();
        general.Save();
        tutorial.Save();
        user.Save();
    }

    private void Load()
    {
        general = SaveGame.Load("General", new GeneralSave());
        remoteConfig = SaveGame.Load("RemoteConfig", new RemoteConfigSave());
        tutorial = SaveGame.Load("Tutorial", new TutorialSave());
        user = SaveGame.Load("User", new UserSave());
    }
}
