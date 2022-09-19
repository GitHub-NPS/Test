using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrateObject : MonoBehaviour
{
    private Quaternion rotation;
    private CoroutineHandle handle;

    private void Awake()
    {
        rotation = this.transform.localRotation;
    }

    private void OnDestroy()
    {
        if (handle.IsValid) Timing.KillCoroutines(handle);
    }

    public void Vibrate()
    {
        if (handle.IsValid)
        {
            this.transform.localRotation = rotation;
            Timing.KillCoroutines(handle);
        }
        handle = Timing.RunCoroutine(_Vibrate());
    }

    private IEnumerator<float> _Vibrate()
    {
        float value = 0.5f;        
        int count = Mathf.RoundToInt(1f / 0.1f);
        float t = 0.05f / count;

        for (int i = 0; i < count; i++)
        {
            this.transform.Rotate(new Vector3() { z = value });
            yield return Timing.WaitForSeconds(t);
        }

        this.transform.localRotation = rotation;

        yield break;
    }
}
