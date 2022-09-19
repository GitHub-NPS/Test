using Lean.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanDestroyPool : LeanDestroy
{
    private ExecuteType m_execute;
    private float m_seconds;

    private void Awake()
    {
        m_execute = Execute;
        m_seconds = Seconds;
    }

    public override void DestroyNow()
    {
        Execute = m_execute;
        Seconds = m_seconds;

        PoolManager.S.Despawn(Target != null ? Target : gameObject);
    }
}
