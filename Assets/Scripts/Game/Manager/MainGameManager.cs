using MEC;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using NPS;

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager S;
    private GeneralSave generalSave;

    private void Awake()
    {
        if (!S) S = this;

        this.RegisterListener(EventID.ChangeAds, OnChangeAds);
    }

    private void OnChangeAds(object obj)
    {
        if (!generalSave.Ads) AppManager.Ads.DestroyBanner();
        else AppManager.Ads?.HideBanner();
    }

    private void OnDestroy()
    {
        Timing.KillCoroutines();

        this.RemoveListener(EventID.ChangeAds, OnChangeAds);
    }

    private void Start()
    {
        generalSave = DataManager.Save.General;
        if (generalSave.Ads) AppManager.Ads?.ShowBanner();
    }

    public void PauseCoroutines()
    {
        foreach (TimingTag tag in (TimingTag[])Enum.GetValues(typeof(TimingTag)))
        {
            Timing.PauseCoroutines(tag.ToString());
        }
    }

    public void ResumeCoroutines()
    {
        foreach (TimingTag tag in (TimingTag[])Enum.GetValues(typeof(TimingTag)))
        {
            Timing.ResumeCoroutines(tag.ToString());
        }
    }
}
