using System;
using UnityEngine;

#if HAS_LION_GAME_ANALYTICS_SDK
using LionStudios.Suite.Analytics;
#endif

public class TrackingEvent : MonoBehaviour
{
    public static void StartInit()
    {
#if HAS_LION_GAME_ANALYTICS_SDK
        LionAnalytics.GameStart();
        Debug.Log("[GAME][LION EVENT] lion analytic start game");
#endif
    }

    public static void LogEventPlayLevel(int level, int attemptNum = 1, int? score = null)
    {
        try
        {
#if HAS_LION_GAME_ANALYTICS_SDK
            LionAnalytics.LevelStart(level, attemptNum, score);
            Debug.Log( string.Format("[GAME][LION EVENT] Play level: {0}, attemptNum: {1}",level.ToString(), attemptNum.ToString()));
#endif
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public static void LogEventCompleteLevel(int level, int attemptNum = 1, int? score = null)
    {
        try
        {
#if HAS_LION_GAME_ANALYTICS_SDK
            LionAnalytics.LevelComplete(level, attemptNum, score);
            Debug.Log( string.Format("[GAME][LION EVENT] Complete level: {0}, attemptNum: {1}",level.ToString(), attemptNum.ToString()));
#endif
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public static void LogEventFailLevel(int level, int attemptNum = 1, int? score = null)
    {
        try
        {
#if HAS_LION_GAME_ANALYTICS_SDK
            LionAnalytics.LevelFail(level, attemptNum, score);
            Debug.Log( string.Format("[GAME][LION EVENT] Fail level: {0}, attemptNum: {1}",level.ToString(), attemptNum.ToString()));
#endif
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public static void LogEventRetryLevel(int level, int attemptNum = 1, int? score = null)
    {
        try
        {
#if HAS_LION_GAME_ANALYTICS_SDK
            LionAnalytics.LevelRestart(level, attemptNum, score);
            Debug.Log( string.Format("[GAME][LION EVENT] Restart level: {0}, attemptNum: {1}",level.ToString(), attemptNum.ToString()));
#endif
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

}
