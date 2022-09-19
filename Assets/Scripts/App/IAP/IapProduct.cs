public enum ProductType
{
    Consumable = 0,
    NonConsumable = 1,
    Subscription = 2
}

[System.Serializable]
public class IapProduct
{
    public string Id;
    public ProductType Type = ProductType.Consumable;
    public string Price;

    public bool Active { set; get; }
    public decimal PriceDecimal { set; get; }
    public string IsoCurrencyCode { set; get; }
    public string LocalizedPrice { set; get; }
}
