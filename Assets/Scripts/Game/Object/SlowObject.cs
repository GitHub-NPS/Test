using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlowObject : MonoBehaviour, ISlow
{
    [SerializeField] private UnityEvent<SlowData> OnAddSlow;
    [SerializeField] private UnityEvent<SlowData> OnRemoveSlow;

    public void AddSlow(SlowData data)
    {
        OnAddSlow?.Invoke(data);
    }

    public void RemoveSlow(SlowData data)
    {
        OnRemoveSlow?.Invoke(data);
    }
}
