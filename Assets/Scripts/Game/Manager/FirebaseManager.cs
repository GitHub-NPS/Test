using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using NPS;

#if UNITY_FIREBASE
using Firebase;
using Firebase.Extensions;
using Firebase.RemoteConfig;
#endif

public class FirebaseManager : MonoSingleton<FirebaseManager>
{
    private bool IsInit = false;
#if User_Tier_Definition
    private bool IsAnalytics => IsInit && UserTierDefinition.Runtime.UserTierDefinition.Instance.CountryCode != "CN";
#else
    private bool IsAnalytics => IsInit;
#endif

    private void Start()
    {
#if DEVELOPMENT
        this.PostEvent(EventID.RemoteConfigComplete);
#endif

#if UNITY_FIREBASE
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            Firebase.Analytics.FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available) {
                Initialize();
            } else {
                Debug.Log(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                this.PostEvent(EventID.RemoteConfigComplete);
            }
        });
#endif
    }

    public void Init(Transform parent = null)
    {
        AppManager.Firebase = this;
        if (parent) transform.SetParent(parent);
    }

    private void Initialize()
    {
#if UNITY_FIREBASE
        var defaults = new Dictionary<string, object>();
        foreach (var config in DataManager.Save.RemoteConfig.Configs)
        {
            if (config.Key.Contains("ab"))
            {
                defaults.Add(config.Key, config.Value);
            }
        }

        FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults).ContinueWithOnMainThread(task => {
            FetchDataAsync();
            IsInit = true;
        });
#endif
    }

#if UNITY_FIREBASE
    private System.Threading.Tasks.Task FetchDataAsync()
    {
        var fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(System.TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    private void FetchComplete(System.Threading.Tasks.Task fetchTask)
    {
        var info = FirebaseRemoteConfig.DefaultInstance.Info;
        if (info.LastFetchStatus == LastFetchStatus.Success)
        {
            FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(task => {
                Dictionary<string, string> changes = new Dictionary<string, string>();

                var configs = DataManager.Save.RemoteConfig.Configs;
                foreach (var config in configs)
                {
                    if (config.Key.Contains("ab"))
                    {
                        changes.Add(config.Key, FirebaseRemoteConfig.DefaultInstance.GetValue(config.Key).StringValue);
                    }
                }

                foreach (var change in changes)
                {
                    configs[change.Key] = change.Value;
                }
               
                this.PostEvent(EventID.RemoteConfigComplete);
            });
        }
    }
#endif

    public void AdImpression(string ad_platform, string ad_source, string ad_unit_name, string ad_format, string ad_revenue)
    {
#if UNITY_FIREBASE
        if (IsAnalytics) {
            Firebase.Analytics.Parameter[] arrParams = {
                new Firebase.Analytics.Parameter("ad_platform", ad_platform),
                new Firebase.Analytics.Parameter("ad_source", ad_source),
                new Firebase.Analytics.Parameter("ad_format", ad_format),
                new Firebase.Analytics.Parameter("currency", "USD"),
                new Firebase.Analytics.Parameter("value", ad_revenue)
            };
            
            if (ad_unit_name.Equals("rewarded_video")) {
                Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression_rewarded", arrParams);
            }
            else if (ad_unit_name.Equals("interstitial")) {                Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression_interstitial", arrParams);
            }
            else if (ad_unit_name.Equals("banner")) {
                Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression_banner", arrParams);
            }
        }
#endif
    }

    public void AdImpression(string ad_platform, string ad_unit_name)
    {
#if UNITY_FIREBASE
        if (IsAnalytics)
        {
            Firebase.Analytics.Parameter[] arrParams = {
                new Firebase.Analytics.Parameter("ad_platform", ad_platform),
                new Firebase.Analytics.Parameter("currency", "USD"),
            };

            if (ad_unit_name.Equals("rewarded_video"))
            {
                Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression_rewarded", arrParams);
            }
            else if (ad_unit_name.Equals("interstitial"))
            {
                Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression_interstitial", arrParams);
            }
            else if (ad_unit_name.Equals("banner"))
            {
                Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression_banner", arrParams);
            }
        }
#endif
    }

    public void OnDemoHandler(params object[] data)
    {
        var id = (string)data[0];
#if UNITY_FIREBASE
        if (m_IsInitialized)
        {
            Firebase.Analytics.Parameter[] arrParams = {
                new Firebase.Analytics.Parameter("id", id)
            };
            Firebase.Analytics.FirebaseAnalytics.LogEvent("demo", arrParams);
        }
#endif
    }
}
