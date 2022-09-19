using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyObject : MonoBehaviour
{
    public float Exit => exit;
    [SerializeField] private bool enable = false;
    [SerializeField] private bool pool = false;
    [SerializeField] private float exit = 10f;
    
    private CoroutineHandle handle;
    private Action callBackBeforeDestroy;

    private void OnEnable()
    {
        if (enable) AutoDestroy();
    }

    public void Set(float exit)
    {
        this.exit = exit;
    }

    public void SetCallBack(Action action)
    {
        this.callBackBeforeDestroy = action;
    }

    public void AutoDestroy()
    {
        if (handle.IsValid) Timing.KillCoroutines(handle);
        handle = Timing.RunCoroutine(_AutoDestroy().CancelWith(this.gameObject));
    }

    private void OnDisable()
    {
        if (handle.IsValid) Timing.KillCoroutines(handle);
    }

    private IEnumerator<float> _AutoDestroy()
    {
        yield return Timing.WaitForSeconds(exit);
        callBackBeforeDestroy?.Invoke();

        if (pool) PoolManager.S.Despawn(this.gameObject);
        else Destroy(this.gameObject);
    }
}
