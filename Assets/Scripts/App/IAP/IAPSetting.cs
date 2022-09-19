using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IAPSetting", menuName = "Config/IAP", order = 1)]
[System.Serializable]
public class IAPSetting : ScriptableObject
{
    public List<IapProduct> Products = new List<IapProduct>();
}
