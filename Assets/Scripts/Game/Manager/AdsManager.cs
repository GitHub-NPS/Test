using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NPS;

#if UNITY_APPSFLYER
using AppsFlyerSDK;
#endif

public class AdsManager : MonoSingleton<AdsManager>
{
    public bool IsInit => m_IsInit;
    public bool IsShowBanner => m_IsShowBanner;

    private string AppKey = "140ded3a1";
    private System.Action<bool> m_OnCompleteShowAd;
    private static int m_LoadInterstitialCount;
    private static int m_LoadBannerCount;
    private AdType m_AdType;
    private string m_PlacementId;

    private bool m_IsShowBanner = false;
    private bool m_IsInit = false;

#if UNITY_APPLOVIN_SDK
    private string maxSdkKey = "Z0Tb-K4SHT-_MDs_V9Tbg_LoJ5UB2_KEQD9HpRMOwm3HdkoEulhgL-tdHmTTsI6nsgNZzxPczhk5R-ZzqRNnVt";

    private string adUnitId = "3b6bb70916767fab";
    private string adRewardUnitId = "af9471b0caf55b19";
    private string bannerAdUnitId = "6541dc28f63a0b27";

    private int retryAttempt;
    private int retryAttemptRev;
#endif

    private void Awake()
    {
#if ANDROID_FREE_PRODUCTION
        AppKey = "140ded3a1";
#elif IOS_FREE_PRODUCTION
        AppKey = "1600189085";
        
        adUnitId = "e66f39aea83e90d8";
        adRewardUnitId = "2718c7d70c356bba";
        bannerAdUnitId = "5e04fd94f3a9ad7a";
#else
        AppKey = "11b623b6d";
#endif
    }

    private void Start()
    {
#if UNITY_IRONSOURCE
        IronSource.Agent.validateIntegration();
        IronSource.Agent.init(AppKey);
#endif

#if UNITY_APPLOVIN_SDK
        MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
        {
            InitializeInterstitialAds();
            InitializeRewardedAds();
            InitializeBannerAds();

            m_IsInit = true;
        };

        MaxSdk.SetSdkKey(maxSdkKey);
        MaxSdk.SetUserId(SystemInfo.deviceUniqueIdentifier);
        MaxSdk.SetVerboseLogging(true);
        MaxSdk.InitializeSdk();
#endif

#if UNITY_EDITOR || DEVELOPMENT
        m_IsInit = true;
#endif
    }

    private void OnEnable()
    {
#if UNITY_IRONSOURCE
        IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
        IronSourceEvents.onBannerAdLoadFailedEvent += BannerAdLoadFailedEvent;

        IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
        IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
        IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
        IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;

        IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
        IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;
        IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
        IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
        IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;

        IronSourceEvents.onImpressionSuccessEvent += ImpressionSuccessEvent;

        IronSourceEvents.onSdkInitializationCompletedEvent += OnSdkInitializationCompleted;
#endif
    }

    private void OnSdkInitializationCompleted()
    {
#if UNITY_IRONSOURCE
        m_IsInit = true;

        IronSource.Agent.loadInterstitial();
        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
        IronSource.Agent.loadRewardedVideo();
#endif
    }

    private void OnDisable()
    {
#if UNITY_IRONSOURCE
        IronSourceEvents.onBannerAdLoadedEvent -= BannerAdLoadedEvent;
        IronSourceEvents.onBannerAdLoadFailedEvent -= BannerAdLoadFailedEvent;

        IronSourceEvents.onRewardedVideoAdRewardedEvent -= RewardedVideoAdRewardedEvent;
        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent -= RewardedVideoAvailabilityChangedEvent;
        IronSourceEvents.onRewardedVideoAdOpenedEvent -= RewardedVideoAdOpenedEvent;
        IronSourceEvents.onRewardedVideoAdStartedEvent -= RewardedVideoAdStartedEvent;
        IronSourceEvents.onRewardedVideoAdClickedEvent -= RewardedVideoAdClickedEvent;

        IronSourceEvents.onInterstitialAdLoadFailedEvent -= InterstitialAdLoadFailedEvent;
        IronSourceEvents.onInterstitialAdClosedEvent -= InterstitialAdClosedEvent;
        IronSourceEvents.onInterstitialAdReadyEvent -= InterstitialAdReadyEvent;
        IronSourceEvents.onInterstitialAdOpenedEvent -= InterstitialAdOpenedEvent;
        IronSourceEvents.onInterstitialAdClickedEvent -= InterstitialAdClickedEvent;

        IronSourceEvents.onImpressionSuccessEvent -= ImpressionSuccessEvent;

        IronSourceEvents.onSdkInitializationCompletedEvent -= OnSdkInitializationCompleted;
#endif
    }

    public void Init(Transform parent = null)
    {
        AppManager.Ads = this;
        if (parent) transform.SetParent(parent);
    }

    private void OnApplicationPause(bool isPaused)
    {
#if UNITY_IRONSOURCE
        IronSource.Agent.onApplicationPause(isPaused);
#endif
    }

#if UNITY_APPLOVIN_SDK
    private void InitializeInterstitialAds()
    {
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;

        LoadInterstitial();
    }

    private void InitializeRewardedAds()
    {
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        LoadRewardedAd();
    }

    private void InitializeBannerAds()
    {
        MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);
        MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, Color.clear);
    }

    private void OnInterstitialLoadedEvent(string _adUnitId, MaxSdkBase.AdInfo _adInfo)
    {
        retryAttempt = 0;

#if UNITY_APPSFLYER
        AppManager.AppsFlyer.InterstitialAdReadyTracking();
#endif
    }

    private void OnInterstitialLoadFailedEvent(string _adUnitId, MaxSdkBase.ErrorInfo _errorInfo)
    {
        retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));

        Invoke("LoadInterstitial", (float)retryDelay);
    }

    private void OnInterstitialDisplayedEvent(string _adUnitId, MaxSdkBase.AdInfo _adInfo) 
    {
#if UNITY_APPSFLYER
        AppManager.AppsFlyer.InterstitialAdOpenTracking();
#endif
#if UNITY_FIREBASE
        AppManager.Firebase.AdImpression("MaxApplovin", "interstitial");
#endif
    }

    private void OnInterstitialAdFailedToDisplayEvent(string _adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo _adInfo)
    {
        LoadInterstitial();
    }

    private void OnInterstitialClickedEvent(string _adUnitId, MaxSdkBase.AdInfo _adInfo) { }

    private void OnInterstitialHiddenEvent(string _adUnitId, MaxSdkBase.AdInfo _adInfo)
    {
        LoadInterstitial();
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        retryAttemptRev = 0;

#if UNITY_APPSFLYER
        AppManager.AppsFlyer.RewardedAdReadyTracking();
#endif
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        retryAttemptRev++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttemptRev));

        Invoke("LoadRewardedAd", (float)retryDelay);
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) 
    {
#if UNITY_APPSFLYER
        AppManager.AppsFlyer.RewardedAdOpenTracking();
#endif
#if UNITY_FIREBASE
        AppManager.Firebase.AdImpression("MaxApplovin", "rewarded_video");
#endif
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        LoadRewardedAd();
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        LoadRewardedAd();
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo) 
    {
        StartCoroutine(InvokeEventAd(true));
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void LoadInterstitial()
    {
        MaxSdk.LoadInterstitial(adUnitId);
    }

    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(adRewardUnitId);
    }
#endif

    public void ShowInterstitialAd(System.Action<bool> onCompleteShowAd = null, string placementId = null)
    {
        Debug.Log("Show Interstitial");

        m_OnCompleteShowAd = onCompleteShowAd;
        m_PlacementId = placementId;
        m_AdType = AdType.Interstitial;

        if (!DataManager.Save.General.Ads) return;

#if UNITY_APPSFLYER
        AppManager.AppsFlyer.InterstitialAdEligibleTracking();
#endif

#if UNITY_IRONSOURCE
        if (IronSource.Agent.isInterstitialReady())
        {
            if (string.IsNullOrEmpty(placementId))
            {
                IronSource.Agent.showInterstitial();
            }
            else
            {
                IronSource.Agent.showInterstitial(placementId);
            }
        }
        else
        {
            IronSource.Agent.loadInterstitial();
        }
#endif

#if UNITY_APPLOVIN_SDK
        if (MaxSdk.IsInterstitialReady(adUnitId))
        {
            MaxSdk.ShowInterstitial(adUnitId);
        }
#endif
    }

    public void ShowRewardedAd(System.Action<bool> onCompleteShowAd = null, string placementId = null)
    {
        Debug.Log("ShowRewardedAd");

        m_OnCompleteShowAd = onCompleteShowAd;
        m_PlacementId = placementId;
        m_AdType = AdType.Rewarded;

#if UNITY_APPSFLYER
        AppManager.AppsFlyer.RewardedAdEligibleTracking();
#endif
        if (IsRewardedAdReady())
        {
#if UNITY_IRONSOURCE
            if (string.IsNullOrEmpty(placementId))
            {
                IronSource.Agent.showRewardedVideo();
            }
            else
            {
                IronSource.Agent.showRewardedVideo(placementId);
            }

            Debug.Log("Show Rewarded Ad success");

            AppManager.Firebase.OnAdsReward();
#endif

#if UNITY_APPLOVIN_SDK
            if (MaxSdk.IsRewardedAdReady(adRewardUnitId))
            {
                MaxSdk.ShowRewardedAd(adRewardUnitId, m_PlacementId);
            }
#endif
        }
        else
        {
#if UNITY_EDITOR || DEVELOPMENT
            StartCoroutine(InvokeEventAd(true));
#else
            StartCoroutine(InvokeEventAd(false));
#endif
        }
    }

    public bool IsRewardedAdReady()
    {
#if UNITY_IRONSOURCE
        Debug.Log("Rewarded Ad Ready");
        return IronSource.Agent.isRewardedVideoAvailable();
#elif UNITY_APPLOVIN_SDK
        return true;
#else
        return false;
#endif
    }

    private IEnumerator InvokeEventAd(bool isComplete)
    {
        yield return new WaitForEndOfFrame();

        m_OnCompleteShowAd?.Invoke(isComplete);
        m_OnCompleteShowAd = null;
    }

    public void ShowBanner()
    {
        m_IsShowBanner = true;

#if UNITY_IRONSOURCE
        IronSource.Agent.displayBanner();
#endif

#if UNITY_APPLOVIN_SDK
        MaxSdk.ShowBanner(bannerAdUnitId);
#endif
    }

    public void HideBanner()
    {
        m_IsShowBanner = false;

#if UNITY_IRONSOURCE
        IronSource.Agent.hideBanner();
#endif

#if UNITY_APPLOVIN_SDK
        MaxSdk.HideBanner(bannerAdUnitId);
#endif
    }

    public void DestroyBanner()
    {
        m_IsShowBanner = false;

#if UNITY_IRONSOURCE
        IronSource.Agent.destroyBanner();
#endif

#if UNITY_APPLOVIN_SDK
        MaxSdk.DestroyBanner(bannerAdUnitId);
#endif
    }

    private void BannerAdLoadedEvent()
    {
        m_LoadBannerCount = 0;
    }
#if UNITY_IRONSOURCE
    private void BannerAdLoadFailedEvent(IronSourceError error)
    {
        if (m_LoadBannerCount++ < 3)
        {
            IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
        }
        else
        {
            m_LoadBannerCount = 0;
        }
    }
#endif

#if UNITY_IRONSOURCE
    private void RewardedVideoAdRewardedEvent(IronSourcePlacement placement)
    {
        StartCoroutine(InvokeEventAd(true));
    }
#endif

    private void RewardedVideoAvailabilityChangedEvent(bool available)
    {
        if (available)
        {
#if UNITY_APPSFLYER
            AppManager.AppsFlyer.RewardedAdReadyTracking();
#endif
        }

#if APP_METRICA
        AppMetrica.OnVideoAds("video_ads_available", "rewarded", m_PlacementId, available ? "success" : "not_available");
#endif
    }

    private void RewardedVideoAdStartedEvent()
    {
#if APP_METRICA
        AppMetrica.OnVideoAds("video_ads_started", "rewarded", m_PlacementId, "start");
#endif
    }

    private void RewardedVideoAdOpenedEvent()
    {
#if UNITY_APPSFLYER
        AppManager.AppsFlyer.RewardedAdOpenTracking();
#endif

#if APP_METRICA
        AppMetrica.OnVideoAds("video_ads_watch", "rewarded", m_PlacementId, "watched");
#endif
    }

#if UNITY_IRONSOURCE
    private void RewardedVideoAdClickedEvent(IronSourcePlacement obj)
    {
#if APP_METRICA
        AppMetrica.OnVideoAds("video_ads_watch", "rewarded", m_PlacementId, "clicked");
#endif
    }
#endif

#if UNITY_IRONSOURCE
    private void InterstitialAdLoadFailedEvent(IronSourceError source)
    {
        if (m_LoadInterstitialCount++ < 3)
        {
            IronSource.Agent.loadInterstitial();
        }
        else
        {
            m_LoadInterstitialCount = 0;
        }

#if APP_METRICA
        AppMetrica.OnVideoAds("video_ads_available", "interstitial", m_PlacementId, "not_available");
#endif
    }
#endif

    private void InterstitialAdClosedEvent()
    {
        m_LoadInterstitialCount = 0;
#if UNITY_IRONSOURCE
        IronSource.Agent.loadInterstitial();
#endif

#if APP_METRICA
        AppMetrica.OnVideoAds("video_ads_available", "interstitial", m_PlacementId, "watched");
#endif
    }

    private void InterstitialAdReadyEvent()
    {
#if UNITY_APPSFLYER
        AppManager.AppsFlyer.InterstitialAdReadyTracking();
#endif

#if APP_METRICA
        AppMetrica.OnVideoAds("video_ads_available", "interstitial", m_PlacementId, "success");
#endif
    }

    private void InterstitialAdOpenedEvent()
    {
#if UNITY_APPSFLYER
        AppManager.AppsFlyer.InterstitialAdOpenTracking();
#endif

#if APP_METRICA
        AppMetrica.OnVideoAds("video_ads_watch", "interstitial", m_PlacementId, "start");
#endif
    }

    private void InterstitialAdClickedEvent()
    {
#if APP_METRICA
        AppMetrica.OnVideoAds("video_ads_watch", "interstitial", m_PlacementId, "clicked");
#endif
    }

#if UNITY_IRONSOURCE
    private void ImpressionSuccessEvent(IronSourceImpressionData impressionData)
    {
        if (impressionData != null)
        {
#if UNITY_FIREBASE
            AppManager.Firebase.AdImpression("ironSource", impressionData.adNetwork, impressionData.adUnit, impressionData.instanceName,
                impressionData.revenue.ToString());
#endif
#if UNITY_APPSFLYER
            AppManager.AppsFlyer.AdImpression("ironSource", impressionData.adNetwork, impressionData.adUnit, impressionData.instanceName,
                impressionData.revenue.ToString());
#endif
        }
    }
#endif

    public enum AdType
    {
        Banner,
        Interstitial,
        Rewarded,
    }
}
