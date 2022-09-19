using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFollowTarget : MonoBehaviour
{
    private CoroutineHandle handle;

    private void OnDisable()
    {
        UnFollow();
    }

    public void Follow(Transform target)
    {
        if (handle.IsValid) Timing.KillCoroutines(handle);
        handle = Timing.RunCoroutine(_AutoFollow(target), Segment.SlowUpdate, TimingTag.Update.ToString());
    }

    public void UnFollow()
    {
        if (handle.IsValid) Timing.KillCoroutines(handle);
    }

    private IEnumerator<float> _AutoFollow(Transform target)
    {
        while (true)
        {
            if (!target || !target.gameObject.activeSelf) break;
            
            this.transform.position = target.position;
            yield return Timing.DeltaTime;
        }

        UnFollow();
    }
}
