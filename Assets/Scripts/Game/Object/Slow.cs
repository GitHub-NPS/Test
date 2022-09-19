using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : MonoBehaviour
{
    public void SetSize(float value)
    {
        this.transform.localScale = new Vector3(value, value, 1);
    }

    protected virtual SlowData getData()
    {
        return new SlowData();
    }

    public void OnSlowEnter2D(Collider2D collision)
    {
        ISlow obj = collision.GetComponent<ISlow>();
        obj?.AddSlow(getData());
    }

    public void OnSlowExit2D(Collider2D collision)
    {
        ISlow obj = collision.GetComponent<ISlow>();
        obj?.RemoveSlow(getData());
    }
}
