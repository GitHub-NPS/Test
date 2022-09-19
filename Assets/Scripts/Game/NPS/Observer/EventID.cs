using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPS
{
    public enum EventID
    {
        None = 0,
        LoadSuccess,
        ChangeAds,
        Tap2ContinueTutorial,
        CompleteTutorial,
        NextStepTutorial,
        RemoteConfigComplete,
        ChangeCurrency
    }
}
