using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPS;

#if UNITY_FB
using Facebook.Unity;
#endif

public class FacebookManager : MonoSingleton<FacebookManager>
{
    private void Start()
    {
#if UNITY_FB
        if (!FB.IsInitialized) {
            FB.Init(InitCallback, OnHideUnity);
        } else {
            FB.ActivateApp();
        }
#endif
    }

    public void Init(Transform parent = null)
    {
        AppManager.Facebook = this;
        if (parent) transform.SetParent(parent);
    }

    private void InitCallback()
    {
#if UNITY_FB
        if (FB.IsInitialized) {
            FB.ActivateApp();
        } else {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
#endif
    }

    private void OnHideUnity(bool isGameShown)
    {
#if UNITY_FB
        if (!isGameShown) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }
#endif
    }
}
