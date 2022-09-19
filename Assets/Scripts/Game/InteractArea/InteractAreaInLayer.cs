using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractAreaInLayer : InteractArea
{
    [SerializeField] private LayerInt[] layers;

    protected override bool condition(Collider2D collision)
    {
        if (collision.gameObject.layer.FindLayer(layers)) return true;
        return false;
    }
}
