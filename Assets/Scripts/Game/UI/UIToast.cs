using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIToast : MonoBehaviour
{
    [SerializeField] private GameObject m_ObjContent;
    [SerializeField] private TextMeshProUGUI m_TxtContent;

    private CoroutineHandle handle;
    public void Show(string content, float time = 1f)
    {        
        m_TxtContent.text = content;
        m_ObjContent.SetActive(true);
        if (handle.IsValid) Timing.KillCoroutines(handle);
        handle = Timing.RunCoroutine(_Show(time));
    }

    private void OnDestroy()
    {
        if (handle.IsValid) Timing.KillCoroutines(handle);
    }

    private IEnumerator<float> _Show(float time)
    {
        yield return Timing.WaitForSeconds(time);
        m_ObjContent.SetActive(false);
    }
}
