using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AOverlap<T> : MonoBehaviour, IOverlap
{
    [SerializeField] private float delay = 0.2f;

    [SerializeField] protected LayerBit[] layers;
    [SerializeField] private List<ObjectTag> tags;

    [SerializeField] private bool isExplosionForce = false;
    [SerializeField] private ExplosionForceData explosionForce;

    [SerializeField] private bool isDamage = false;
    [SerializeField] private DamageData damage;

    [SerializeField] private bool isFreeze = false;
    [SerializeField] private IceData ice;

    [SerializeField] private bool isStun = false;
    [SerializeField] private StunData stun;

    [SerializeField] private bool isBurn = false;
    [SerializeField] private BurnData burn;

    [SerializeField] private bool isSlow = false;
    [SerializeField] private SlowData slow;

    private string source;

    private CoroutineHandle handle;

    public void SetSource(string source)
    {
        this.source = source;

        this.damage.Source = source;
        this.ice.Source = source;
        this.explosionForce.Source = source;
    }

    public virtual void SetSize(float size)
    {
        this.transform.localScale = new Vector3(size, size, 1);
    }

    public void SetDamage(DamageData damage)
    {
        isDamage = damage.Value > 0;
        this.damage = damage;
    }

    public void SetDamage(float dmg)
    {
        isDamage = dmg > 0;
        damage.Value = dmg;
    }

    public void SetExplosionForce(ExplosionForceData explosionForce)
    {
        isExplosionForce = explosionForce.Value > 0;
        this.explosionForce = explosionForce;
    }

    public void SetExplosionForce(float force)
    {
        isExplosionForce = force > 0;
        explosionForce.Value = force;
    }

    public void SetIce(IceData ice)
    {
        isFreeze = ice.Value > 0;
        this.ice = ice;
    }

    public void SetIce(float time)
    {
        isFreeze = time > 0;
        ice.Value = time;
    }

    public void SetStun(StunData stun)
    {
        isStun = stun.Value > 0;
        this.stun = stun;
    }

    public void SetStun(float time)
    {
        isStun = time > 0;
        stun.Value = time;
    }

    public void SetBurn(bool burn)
    {
        isBurn = burn;
    }

    public void SetSlow(float time, float perSlow)
    {
        isSlow = time > 0;
        slow.Duration = time;
        slow.Percent = perSlow;
    }

    public void Ray()
    {
        handle = Timing.RunCoroutine(_Ray());
    }

    private void OnDisable()
    {
        if (handle.IsValid) Timing.KillCoroutines(handle);
    }

    protected abstract Collider2D[] GetCollider();

    private IEnumerator<float> _Ray()
    {
        yield return Timing.WaitForSeconds(delay);

        var cl = GetCollider();
        var objs = cl.GetObject<T>(tags);

        foreach (var obj in objs)
        {
            if (isDamage)
            {
                IHit iHit = obj.GetGameObject().GetComponent<IHit>();
                iHit?.Hit(damage);
            }

            if (isExplosionForce)
            {
                explosionForce.Form = this.transform.position;

                IExplosionForce iEx = obj.GetGameObject().GetComponent<IExplosionForce>();
                iEx?.ExplosionForce(explosionForce);
            }

            if (isFreeze)
            {
                IFreeze iFz = obj.GetGameObject().GetComponent<IFreeze>();
                iFz?.Freeze(ice);
            }

            if (isStun)
            {
                IStun iSt = obj.GetGameObject().GetComponent<IStun>();
                iSt?.Stun(stun);
            }

            if (isBurn)
            {
                IBurn iBr = obj.GetGameObject().GetComponent<IBurn>();
                iBr?.Burn(burn);
            }

            if (isSlow)
            {
                ISlow iSl = obj.GetGameObject().GetComponent<ISlow>();
                iSl?.AddSlow(slow.Clone());
            }
        }
    }
}
