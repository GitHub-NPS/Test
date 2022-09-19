using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearParticleSystem : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private bool enable = false;
    [SerializeField] private bool disable = false;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (ps == null) ps = GetComponent<ParticleSystem>();
    }
#endif

    private void OnEnable()
    {
        if (enable) Clear();
    }

    private void OnDisable()
    {
        if (disable) Clear();
    }

    private void Clear()
    {
        ps.Clear();
    }
}
