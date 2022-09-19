using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StunObject : MonoBehaviour, IStun
{
    [SerializeField] private UnityEvent<StunData> OnStun;

    public void Stun(StunData data)
    {
        OnStun?.Invoke(data);
    }
}
