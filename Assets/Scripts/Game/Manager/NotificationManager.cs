using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using I2.Loc;
using NPS;

#if UNITY_NOTIFICATION && UNITY_ANDROID
using Unity.Notifications.Android;
#elif UNITY_NOTIFICATION && UNITY_IOS
using Unity.Notifications.iOS;
#endif

public class NotificationManager : MonoSingleton<NotificationManager>
{
    private void Start()
    {
#if UNITY_NOTIFICATION && UNITY_ANDROID
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
        AndroidNotificationCenter.CancelAllNotifications();
#elif UNITY_NOTIFICATION && UNITY_IOS
        iOSNotificationCenter.RemoveAllScheduledNotifications();
        iOSNotificationCenter.RemoveAllDeliveredNotifications();
#endif
    }

    public void Init(Transform parent = null)
    {
        AppManager.Notification = this;
        if (parent) transform.SetParent(parent);
    }

    private void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            Alert();
        }
        else
        {
#if UNITY_NOTIFICATION && UNITY_ANDROID
            AndroidNotificationCenter.CancelAllNotifications();
#elif UNITY_NOTIFICATION && UNITY_IOS
            iOSNotificationCenter.RemoveAllScheduledNotifications();
            iOSNotificationCenter.RemoveAllDeliveredNotifications();
#endif
        }
    }

    private void Alert()
    {
        var nowTime = UnbiasedTime.UtcNow;
        var fireTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 20, 0, 0);

        if (fireTime <= nowTime)
        {
            fireTime = fireTime.AddDays(1);
        }

        var repeatInterval = new TimeSpan(1, 0, 0, 0);
        var message = LocalizationManager.GetTranslation("Notification_Msg");
        var intent = "Alert";
        SendLocalNotification(message, fireTime, repeatInterval, intent);
    }

    private void SendLocalNotification(string message, DateTime fireTime, TimeSpan repeatInterval, string intent = "")
    {
#if UNITY_NOTIFICATION && UNITY_ANDROID
        var notification = new AndroidNotification
        {
            Title = Application.productName,
            Text = message,
            FireTime = fireTime,
            RepeatInterval = repeatInterval,
            ShouldAutoCancel = true,
            LargeIcon = "app_icon_large",
            IntentData = intent
        };
        AndroidNotificationCenter.SendNotification(notification, "channel_id");
#elif UNITY_NOTIFICATION && UNITY_IOS
        var calendarTrigger = new iOSNotificationCalendarTrigger()
        {
            Hour = 20,
            Minute = 0,
            Second = 0,
            Repeats = true
        };

        var notification = new iOSNotification()
        {
            Identifier = "notification_01",
            Body = message,
            Subtitle = Application.productName,
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = calendarTrigger,
            Data = intent
        };

        iOSNotificationCenter.ScheduleNotification(notification);
#endif
    }

    public string GetNotificationIntent()
    {
#if UNITY_NOTIFICATION && UNITY_ANDROID
        var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();
        if (notificationIntentData != null)
        {
            return notificationIntentData.Notification.IntentData;
        }

        return string.Empty;
#elif UNITY_NOTIFICATION && UNITY_IOS
        var notification = iOSNotificationCenter.GetLastRespondedNotification();
        if (notification != null)
        {
            return notification.Data;
        }
        return string.Empty;
#else
        return string.Empty;
#endif
    }
}
