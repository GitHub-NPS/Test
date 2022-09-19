using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandomItem : SpawnObject
{
    [SerializeField] private List<GameObject> items;

    public override void Spawn()
    {
        prefab = items[Random.Range(0, items.Count)];
        base.Spawn();
    }
}
