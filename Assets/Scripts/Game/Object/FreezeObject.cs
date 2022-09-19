using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FreezeObject : MonoBehaviour, IFreeze
{
    [SerializeField] private UnityEvent<IceData> OnFreeze;

    public void Freeze(IceData data)
    {
        OnFreeze?.Invoke(data);
    }
}
