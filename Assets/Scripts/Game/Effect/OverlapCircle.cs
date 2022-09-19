using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapCircle<T> : AOverlap<T>
{
    [SerializeField] private Vector3 offset;
    [SerializeField] protected float radius = 2.5f;

    protected float curRadius = 0;

    private void Awake()
    {
        curRadius = radius;
    }

    public override void SetSize(float size)
    {
        base.SetSize(size);

        curRadius = this.radius * size;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        curRadius = radius;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position + offset, curRadius);
    }
#endif

    protected override Collider2D[] GetCollider()
    {
        return Physics2D.OverlapCircleAll(this.transform.position + offset, curRadius, Utils.FindLayer(layers));
    }
}
