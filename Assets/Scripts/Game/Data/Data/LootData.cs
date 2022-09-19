using System;

[System.Serializable]
public class LootData
{
    public LootType Type;
    public object Data;

    public LootData()
    {

    }

    public LootData(string content)
    {
        string[] str = content.Split(';');
        Enum.TryParse(str[0], out LootType lootType);
        Type = lootType;

        switch (lootType)
        {
            case LootType.Currency:
                Data = new CurrencyData(content);
                break;
        }
    }
}
