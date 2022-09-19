[System.Serializable]
public class SlowData
{
    public ObjectTag Agent;
    public float Percent;
    public bool IsSwitch = false;
    public float Duration = -1f;
    public bool IsColor = false;

    public float Value { get; private set; } = 0;

    public void SetValue(float value)
    {
        Value = value;
    }

    public SlowData Clone()
    {
        SlowData clone = new SlowData();
        clone.Agent = Agent;
        clone.Percent = Percent;
        clone.IsSwitch = IsSwitch;
        clone.Duration = Duration;
        clone.Value = Value;
        clone.IsColor = IsColor;

        return clone;
    }
}
