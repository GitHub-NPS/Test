using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPS;

#if UNITY_APPSFLYER
using AppsFlyerSDK;
#endif

public class AppsFlyerManager : MonoSingleton<AppsFlyerManager>
{
    private string devKey = "HWAAfxs6ec2wwZfsRpjipJ";

    private string androidPublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAlZYAqA286v62RdPKgThx14xtqk49CIviIaX1DWaR5DI8ov0gFyRiLp6OtTJOF62TDbul//b5zIFMHPwC5KBXTRjwcLfsVTAfGFP0+PPZJEe4luNU9FnT3rfgzKEkspfq772gIFFlqZ4fVLEz5KwF1d1qHhBy1VlevP64Uzm1BlndpBdQ0qUOlCDXdrx+KQuqyDpMCSOqceC/76Ju9N+e1QWZ1C/TYxjVILD3ovJ7eDN5cl1pffQomBAAMPYe0zxEuNTSu1znaKm3aLawTO3gZqVuRDP483imhtZV1Dz6nPpAw/PfuJfM1P6KPxyjvaD3ZlkieQvCUg6mi5r2qNc//QIDAQAB";
    private string iOSAppId = "1600189085";
    private bool m_IsInitialized = false;

    private bool tokenSent;

    private void Awake()
    {
#if ANDROID_FREE_PRODUCTION
#if ABI_PUBLISH
        devKey = "G3MBmMRHTuEpXbqyqSWGeK";
#endif
        devKey = "HWAAfxs6ec2wwZfsRpjipJ";
#elif IOS_FREE_PRODUCTION
        devKey = "G3MBmMRHTuEpXbqyqSWGeK";
        iOSAppId = "1600189085";
#else
        devKey = "HWAAfxs6ec2wwZfsRpjipJ";
        iOSAppId = "1600189085";
#endif
    }

    private void Start()
    {
#if UNITY_APPSFLYER
        AppsFlyer.setIsDebug(false);
        AppsFlyer.initSDK(devKey, iOSAppId);
        AppsFlyer.startSDK();
        m_IsInitialized = true;

#if UNITY_IOS
        UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert | UnityEngine.iOS.NotificationType.Badge | UnityEngine.iOS.NotificationType.Sound);
#endif
#endif
    }

    public void Init(Transform parent = null)
    {
        AppManager.AppsFlyer = this;
        if (parent) transform.SetParent(parent);
    }

    private void Update()
    {
#if UNITY_APPSFLYER
#if UNITY_IOS
        if (!tokenSent)
        {
            byte[] token = UnityEngine.iOS.NotificationServices.deviceToken;
            if (token != null)
            {
                AppsFlyeriOS.registerUninstall(token);
                tokenSent = true;
            }
        }
#endif
#endif
    }

    public void AdImpression(string ad_platform, string ad_source, string ad_unit_name, string ad_format, string ad_revenue)
    {
#if UNITY_APPSFLYER
        if (m_IsInitialized) {
            var eventParams = new Dictionary<string, string>();
            eventParams.Add("af_platform", ad_platform);
            eventParams.Add("af_source", ad_source);
            eventParams.Add("af_format", ad_format);
            eventParams.Add("af_currency", "USD");
            eventParams.Add("af_value", ad_revenue);

            if (ad_unit_name.Equals("rewarded_video")) {
                AppsFlyer.sendEvent("af_impression_rewarded", eventParams);
            } else if (ad_unit_name.Equals("interstitial")) {
                AppsFlyer.sendEvent("af_impression_interstitial", eventParams);
            } else if (ad_unit_name.Equals("banner")) {
                AppsFlyer.sendEvent("af_impression_banner", eventParams);
            }
        }
#endif
    }

    public void InterstitialAdEligibleTracking()
    {
#if UNITY_APPSFLYER
        //AppsFlyer.sendEvent("af_inters_ad_eligible", null);

        var general = DataManager.Save.General;
        general.CountInterAds++;
        if (3 <= general.CountInterAds && general.CountInterAds <= 9)
        {
            var eventParams = new Dictionary<string, string>();
            eventParams.Add("af_count", general.CountInterAds.ToString());

            AppsFlyer.sendEvent("af_ad_inters", eventParams);
        }
#endif
    }

    public void InterstitialAdReadyTracking()
    {
#if UNITY_APPSFLYER
        AppsFlyer.sendEvent("af_inters_api_called", null);
#endif
    }

    public void InterstitialAdOpenTracking()
    {
#if UNITY_APPSFLYER
        AppsFlyer.sendEvent("af_inters_displayed", null);
#endif
    }

    public void RewardedAdEligibleTracking()
    {
#if UNITY_APPSFLYER
        //AppsFlyer.sendEvent("af_rewarded_ad_eligible", null);

        var general = DataManager.Save.General;
        general.CountRewardAds++;
        if (1 <= general.CountRewardAds && general.CountRewardAds <= 5)
        {
            var eventParams = new Dictionary<string, string>();
            eventParams.Add("af_count", general.CountRewardAds.ToString());

            AppsFlyer.sendEvent("af_ad_reward", eventParams);
        }
#endif
    }

    public void RewardedAdReadyTracking()
    {
#if UNITY_APPSFLYER
        AppsFlyer.sendEvent("af_rewarded_api_called", null);
#endif
    }

    public void RewardedAdOpenTracking()
    {
#if UNITY_APPSFLYER
        AppsFlyer.sendEvent("af_rewarded_ad_displayed", null);
#endif
    }

    public void UninstallTracking(string token)
    {
#if UNITY_APPSFLYER
#if UNITY_ANDROID
        AppsFlyerAndroid.updateServerUninstallToken(token);
#endif
#endif
    }

    public void AndroidRevenueTracking(string signature, string purchaseData, string price, string currency)
    {
#if UNITY_APPSFLYER
#if UNITY_ANDROID
        AppsFlyerAndroid.validateAndSendInAppPurchase(androidPublicKey, signature, purchaseData, price, currency, null, this);
#endif
#endif
    }

    public void iOSRevenueTracking(string prodId, string price, string currency, string transactionId)
    {
#if UNITY_APPSFLYER
#if UNITY_IOS
        AppsFlyeriOS.validateAndSendInAppPurchase(prodId, price, currency, transactionId, null, this);
#endif
#endif
    }

    public void CompleteTutorialTracking(bool af_success, int af_tutorial_id, string af_content)
    {
#if UNITY_APPSFLYER
        var eventParams = new Dictionary<string, string>();
        eventParams.Add("af_success", af_success ? "true" : "false");
        eventParams.Add("af_tutorial_id", af_tutorial_id.ToString());
        eventParams.Add("af_content", af_content);

        AppsFlyer.sendEvent("af_tutorial_completion", eventParams);
#endif
    }

    public void LoginSuccessTracking()
    {
#if UNITY_APPSFLYER
        AppsFlyer.sendEvent("af_login", null);
#endif
    }
}
