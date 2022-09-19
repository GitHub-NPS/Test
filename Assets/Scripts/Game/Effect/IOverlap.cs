public interface IOverlap
{
    void SetDamage(DamageData damage);

    void SetDamage(float dmg);
    
    void SetSource(string source);

    void SetExplosionForce(ExplosionForceData explosionForce);

    void SetExplosionForce(float force);

    void SetIce(IceData ice);

    void SetIce(float time);

    void SetStun(float time);

    void SetSize(float size);

    void SetBurn(bool active);

    void SetSlow(float time, float perSlow);

    void Ray();
}
