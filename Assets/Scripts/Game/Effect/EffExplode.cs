using MEC;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffExplode : MonoBehaviour
{
    private IOverlap overlap;
    private PlaySound sound;

    private void Awake()
    {
        overlap = GetComponent<IOverlap>();
        sound = GetComponent<PlaySound>();
    }

    public void SetSource(string source)
    {
        overlap.SetSource(source);
    }

    public void SetSize(float size)
    {
        overlap.SetSize(size);
    }

    public void SetDmg(float dmg)
    {
        overlap.SetDamage(dmg);
    }

    public void SetForce(float force)
    {
        overlap.SetExplosionForce(force);
    }

    public void SetIce(float time)
    {
        overlap.SetIce(time);
    }

    public void SetStun(float time)
    {
        overlap.SetStun(time);
    }

    public void SetBurn(bool active)
    {
        overlap.SetBurn(active);
    }

    public void SetSlow(float time, float perSlow)
    {
        overlap.SetSlow(time, perSlow);
    }

    public void Ray()
    {
        overlap.Ray();
    }

    public void Sound()
    {
        sound?.Play();
    }
}
