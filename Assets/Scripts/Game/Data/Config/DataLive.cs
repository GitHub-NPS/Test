using Sirenix.OdinInspector;
using UnityEngine;

public class DataLive<T>: ScriptableObject, IDataLive
{
    public T Value;

    [Button]
    public virtual void Clear()
    {
        Value = default;
    }
}
