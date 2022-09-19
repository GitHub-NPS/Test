using System.Collections;
using UnityEngine;
using MEC;
using NPS;

public class SplashGameManager : MonoBehaviour
{
    [SerializeField] UILoading Loading;

    private void Awake()
    {
        this.RegisterListener(EventID.LoadSuccess, OnLoadSuccess);
    }

    private void OnLoadSuccess(object obj)
    {
        MonoScene.S.LoadAsync("Main");
    }

    private void OnDestroy()
    {
        Timing.KillCoroutines();

        this.RemoveListener(EventID.LoadSuccess, OnLoadSuccess);
    }

    private void Start()
    {
        DataManager.S.Init();
        AppManager.S.Init();        

        Loading.Loading(3f, () =>
        {
            Loading.Tap2Continue(() =>
            {
                MonoScene.S.Active();
            });
        }, false);

        AppManager.Ads?.HideBanner();
    }
}
