using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapCapsule<T> : AOverlap<T>
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector2 size;
    [SerializeField] private CapsuleDirection2D direction;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position + offset + new Vector3(-size.x / 2 + size.y / 2, 0), size.y / 2);
        Gizmos.DrawWireSphere(this.transform.position + offset + new Vector3(size.x / 2 - size.y / 2, 0), size.y / 2);
    }
#endif

    protected override Collider2D[] GetCollider()
    {
        return Physics2D.OverlapCapsuleAll(this.transform.position + offset, size, direction, this.transform.localRotation.z, Utils.FindLayer(layers));
    }
}
