using System;
using UnityEngine;

[System.Serializable]
public class IceData: ICloneable
{
    public ObjectTag Agent;
    public float Value;
    public string Source = null;

    public object Clone()
    {
        return base.MemberwiseClone();
    }
}
