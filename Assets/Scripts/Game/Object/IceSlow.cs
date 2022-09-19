using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSlow: Slow
{
    private SlowData data;

    public void Set(SlowData data)
    {
        this.data = data;
    }

    protected override SlowData getData()
    {
        return data.Clone();
    }
}
