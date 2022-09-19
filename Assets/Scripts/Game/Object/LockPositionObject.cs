using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPositionObject : MonoBehaviour
{
    [SerializeField] private bool enable = false;
    
    private CoroutineHandle handle;

    private void OnEnable()
    {
        if (enable) LockPosition();
    }

    public void LockPosition()
    {
        handle = Timing.RunCoroutine(_LockPosition());
    }

    private void OnDisable()
    {
        if (handle.IsValid) Timing.KillCoroutines(handle);
    }

    private IEnumerator<float> _LockPosition()
    {
        while (true)
        {
            this.transform.localPosition = Vector3.zero;
            yield return Timing.DeltaTime;
        }
    }
}
