using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExplosionForceObject : MonoBehaviour, IExplosionForce
{
    [SerializeField] private UnityEvent<ExplosionForceData> OnExplosionForce;

    public void ExplosionForce(ExplosionForceData data)
    {
        OnExplosionForce?.Invoke(data);
    }
}
