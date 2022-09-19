using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BurnObject : MonoBehaviour, IBurn
{
    [SerializeField] private UnityEvent<BurnData> OnBurn;

    public void Burn(BurnData data)
    {
        OnBurn?.Invoke(data);
    }
}
