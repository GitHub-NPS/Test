using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEnableObject : MonoBehaviour
{
    [SerializeField] private UnityEvent Action;

    private void OnEnable()
    {
        Action?.Invoke();
    }
}
