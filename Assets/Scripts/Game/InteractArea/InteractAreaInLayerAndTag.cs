using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractAreaInLayerAndTag<T> : InteractArea
{
    [SerializeField] private LayerInt[] layers;
    [SerializeField] private List<ObjectTag> tags;    

    protected override bool condition(Collider2D collision)
    {
        if (!collision.gameObject.layer.FindLayer(layers)) return false;
        T iHit = Utils.GetObject<T>(collision, tags);
        if (iHit != null) return true;
        return false;
    }
}
