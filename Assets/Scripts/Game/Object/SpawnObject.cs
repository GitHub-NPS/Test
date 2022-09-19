using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] protected GameObject prefab;
    [SerializeField] private bool IsParent;    
    [SerializeField] private Transform content;
    [SerializeField] private bool pool = true;

    private Transform parent;

    private void Awake()
    {
        Transform self = content != null ? content : this.transform;
        parent = IsParent ? self.parent : content;
    }

    public virtual void Spawn()
    {
        var obj = pool ? PoolManager.S.Spawn(prefab, parent) : Instantiate(prefab, parent);
        obj.transform.position = this.transform.position;
    }
}
