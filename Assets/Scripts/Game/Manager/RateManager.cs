using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPS;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

#if UNITY_REVIEW
using Google.Play.Review;
#endif

public class RateManager : MonoSingleton<RateManager>
{
#if UNITY_ANDROID && UNITY_REVIEW
    private ReviewManager m_ReviewManager;
#endif

    public void Init(Transform parent = null)
    {
        AppManager.Rate = this;
        if (parent != null)
        {
            transform.SetParent(parent);
        }
    }

    private void Start()
    {
#if UNITY_ANDROID && UNITY_REVIEW
        m_ReviewManager = new ReviewManager();
#endif
    }

#if UNITY_ANDROID && UNITY_REVIEW
    private IEnumerator LaunchInAppReview()
    {
        var requestFlowOperation = m_ReviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            Debug.Log("Request err:::::::::::: " + requestFlowOperation.Error.ToString());
            yield break;
        }

        var playReviewInfo = requestFlowOperation.GetResult();
        var launchFlowOperation = m_ReviewManager.LaunchReviewFlow(playReviewInfo);
        yield return launchFlowOperation;
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            Debug.Log("Launch err:::::::::::: " + launchFlowOperation.Error.ToString());
            yield break;
        }
    }
#endif

    public void OpenPromptReview()
    {
#if UNITY_EDITOR
        Debug.Log("Editor Not Support!");
#endif
#if UNITY_ANDROID && UNITY_REVIEW
        StartCoroutine(LaunchInAppReview());
#endif
#if UNITY_IOS
        Device.RequestStoreReview();
#endif
    }

    public void OpenStoreReview()
    {
#if UNITY_EDITOR
        Debug.Log("Editor Not Support!");
#elif UNITY_ANDROID
        Application.OpenURL("market://details?id=" + Application.identifier);
#elif UNITY_IOS
        Application.OpenURL("itms-apps://itunes.apple.com/app/id1600189085?action=write-review");
#endif
    }
}