using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TagObject : MonoBehaviour, ITag
{
    [SerializeField] private ObjectTag self;

    public ObjectTag GetTag()
    {
        return self;
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
}
