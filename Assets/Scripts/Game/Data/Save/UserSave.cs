using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using NPS;
using UnityEngine;
using BayatGames.SaveGameFree;

[System.Serializable]
public class UserSave: IDataSave
{
    private string key = "User";

    [ShowInInspector] public Dictionary<CurrencyType, double> Currency = new Dictionary<CurrencyType, double>();

    public string id = "";
    public string name = "You";
    public string avatar = "";
    public int rank = -1;

    public UserSave()
    {
        foreach (CurrencyType currency in (CurrencyType[])Enum.GetValues(typeof(CurrencyType)))
        {
            Currency.Add(currency, 0);
        }

        Currency[CurrencyType.Coin] = Constant.InitCoin;
        Currency[CurrencyType.Diamond] = Constant.InitDiamond;
    }

    public void Fix()
    {
        foreach (CurrencyType currency in (CurrencyType[])Enum.GetValues(typeof(CurrencyType)))
        {
            if (!Currency.ContainsKey(currency))
                Currency.Add(currency, 0);
        }
    }

    public void Save()
    {
        SaveGame.Save(key, this);
    }

    public void IncreaseCurrency(CurrencyData currency)
    {
        IncreaseCurrency(currency.Type, currency.Value);
    }

    public void IncreaseCurrency(CurrencyType type, double value)
    {
        Currency[type] += value;

        Observer.S?.PostEvent(EventID.ChangeCurrency, type);
    }

    public void SubtractCurrency(CurrencyData currency)
    {
        SubtractCurrency(currency.Type, currency.Value);
    }

    public void SubtractCurrency(CurrencyType type, double value)
    {
        Currency[type] = Currency[type] >= value ? Currency[type] - value : 0;

        Observer.S?.PostEvent(EventID.ChangeCurrency, type);
    }
}
