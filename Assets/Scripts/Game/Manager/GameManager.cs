using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPS;

#if HAS_LION_GAME_ANALYTICS_SDK
using GameAnalyticsSDK;
#endif

public class GameManager : MonoSingleton<GameManager>
{
    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

#if HAS_LION_GAME_ANALYTICS_SDK
        GameAnalytics.Initialize();
#endif
        TrackingEvent.StartInit();
        DontDestroyOnLoad(this);
    }

    private void Start()
    {

    }
}
