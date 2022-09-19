[System.Serializable]
public class StunData
{
    public ObjectTag Agent;
    public float Value;
    public string Source = null;

    public object Clone()
    {
        return base.MemberwiseClone();
    }
}
