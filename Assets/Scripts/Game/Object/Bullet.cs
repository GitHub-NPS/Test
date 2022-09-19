using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private DamageData damage;
    private MoveForwardObject move;

    private void Awake()
    {
        move = GetComponent<MoveForwardObject>();
    }

    public void Set(float angle, float dmg, float speed)
    {
        damage.Value = dmg;
        move.Set(angle, speed);
        move.Move();
    }

    public void HitEnter2D(Collider2D collision)
    {
        var iHit = collision.GetComponent<IHit>();
        if (iHit != null)
        {
            iHit.Hit(damage);

            PoolManager.S.Despawn(this.gameObject);
        }
    }
}
