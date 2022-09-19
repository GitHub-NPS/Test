using System;
using System.Collections.Generic;
using MEC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private TextMeshProUGUI txtVersion;
    [SerializeField] private GameObject btnTap2Play;
    [SerializeField] private Image loadingImg;
    [SerializeField] private GameObject loadingBar;
    [SerializeField] private TextMeshProUGUI txtLoading;

    private CoroutineHandle handle;
    private float time = 1.5f;
    private Action loadComplete;
    private Action tap2Continue;

    private float t = 0;

    public void Start()
    {
        txtVersion.text = "Version: " + Application.version;
    }

    public void Loading(float time, Action callback = null, bool reset = true)
    {
        this.time = time;
        this.loadComplete = callback;

        if (reset) Reset();

        //GC.Collect();

        if (handle.IsValid) Timing.KillCoroutines(handle);
        handle = Timing.RunCoroutine(_Loading(), Segment.RealtimeUpdate);

        content.SetActive(true);
    }

    private void Reset()
    {
        t = 0;
        loadingImg.fillAmount = 0f;

        btnTap2Play.SetActive(false);
        loadingBar.SetActive(true);
    }

    private IEnumerator<float> _Loading()
    {
        while (true)
        {
            t += Timing.DeltaTime;
            loadingImg.fillAmount = t / time;
            if (t >= time) break;

            yield return Timing.DeltaTime;
        }

        loadingImg.fillAmount = 1f;
        loadComplete?.Invoke();

        yield break;
    }

    public void Tap2Continue()
    {
        tap2Continue?.Invoke();
    }

    public void Tap2Continue(Action callback)
    {
        this.tap2Continue = callback;

        btnTap2Play.SetActive(true);
        loadingBar.SetActive(false);
        txtLoading.gameObject.SetActive(false);
    }

    public void Hide()
    {
        content.SetActive(false);
    }
}
