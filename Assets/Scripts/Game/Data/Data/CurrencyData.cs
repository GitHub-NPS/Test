using System;

[System.Serializable]
public class CurrencyData
{
    public CurrencyType Type = CurrencyType.Coin;
    public ulong Value = 0;

    public CurrencyData Clone()
    {
        CurrencyData clone = new CurrencyData();
        clone.Type = Type;
        clone.Value = Value;

        return clone;
    }

    public CurrencyData()
    {

    }

    public CurrencyData(string content)
    {
        string[] str = content.Split(';');
        Enum.TryParse(str[1], out CurrencyType currencyType);
        Type = currencyType;
        Value = uint.Parse(str[2]);
    }
}
