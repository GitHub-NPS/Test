[System.Serializable]
public class BurnData
{
    public ObjectTag Agent;
    public string Source = null;

    public object Clone()
    {
        return base.MemberwiseClone();
    }
}
