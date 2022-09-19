using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager S;

    private void Awake()
    {
        if (!S) S = this;
    }

    private void Start()
    {

    }

    public T Spawn<T>(T prefab) where T : Component
    {
        return LeanPool.Spawn(prefab);
    }

    public T Spawn<T>(T prefab, Transform parent) where T : Component
    {
        return LeanPool.Spawn(prefab, parent);
    }

    public GameObject Spawn(GameObject prefab)
    {
        return LeanPool.Spawn(prefab);
    }

    public GameObject Spawn(GameObject prefab, Transform parent)
    {
        return LeanPool.Spawn(prefab, parent);
    }

    public void Despawn(Component clone, float delay = 0)
    {
        LeanPool.Despawn(clone, delay);
    }

    public void Despawn(GameObject clone, float delay = 0)
    {
        LeanPool.Despawn(clone, delay);
    }


    public void DespawnAll()
    {
        LeanPool.DespawnAll();
    }
}
