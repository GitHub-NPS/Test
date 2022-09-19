using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitObject : MonoBehaviour, IHit
{
    [SerializeField] private UnityEvent<DamageData> OnHit;

    public void Hit(DamageData data)
    {
        OnHit?.Invoke(data);
    }
}
