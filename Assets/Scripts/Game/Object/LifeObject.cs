using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LifeObject : MonoBehaviour
{
    [SerializeField] private UnityEvent OnHit;
    [SerializeField] private UnityEvent OnDie;

    protected int life = 5;

    public void Set(int life)
    {
        this.life = life;
    }

    public virtual void Hit(DamageData damage)
    {
        if (life <= 0)
        {
            return;
        }

        HitSuccess();

        if (life == 0)
        {
            Die();
        }
    }

    protected virtual void HitSuccess()
    {
        life--;
        OnHit?.Invoke();
    }

    protected virtual void Die()
    {
        OnDie?.Invoke();
        PoolManager.S.Despawn(this.gameObject);
    }

    public void OneShot(DamageData damage)
    {
        Hit(damage);
        if (life >= 1)
            Die();
    }
}
