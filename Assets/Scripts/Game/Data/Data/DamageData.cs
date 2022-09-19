[System.Serializable]
public class DamageData
{
    public ObjectTag Agent;
    public float Value;
    public bool IsCrit;
    public bool IsDirect = true;
    public bool IsFlash = true;
    public string Source = null;
}
