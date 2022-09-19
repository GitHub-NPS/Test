using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using NPS;
using System.Globalization;
using System.Text.RegularExpressions;

#if UNITY_IAP
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
#endif

#if UNITY_IAP
public class IAPManager : MonoSingleton<IAPManager>, IStoreListener
{
    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;
    [ShowInInspector] private List<IapProduct> m_Products;
    private ConfigurationBuilder m_Builder;
    private UnityAction<PurchaseState, IapProduct> OnPurchaseComplete;

    public void Init(Transform parent = null)
    {
        AppManager.Iap = this;
        if (parent) transform.SetParent(parent);

        InitializeIap();
    }

    private void InitializeIap()
    {
        var settings = Resources.Load<IAPSetting>("IAPSetting");
        if (settings == null)
        {
            Debug.LogError("No products available");
            return;
        }

        m_Products = settings.Products;

        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
    }

    private void InitializePurchasing()
    {
        if (IsInitialized()) return;

        m_Builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        for (int i = 0; i < m_Products.Count; i++)
        {
            var productId = m_Products[i].Id;
            var productType = (UnityEngine.Purchasing.ProductType)((int)m_Products[i].Type);
            m_Builder.AddProduct(productId, productType);
        }

        UnityPurchasing.Initialize(this, m_Builder);
    }

    public bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
        for (var i = 0; i < m_Products.Count; i++)
        {
            var product = m_StoreController.products.WithID(m_Products[i].Id);

            Debug.Log(this + product.metadata.localizedTitle + " is available " + product.availableToPurchase);

            if (m_Products[i].Type == ProductType.Subscription)
            {
                if (product != null && product.hasReceipt)
                {
                    if (ReceiptIsValid(m_Products[i].Id, product.receipt, out var exception))
                    {
                        m_Products[i].Active = true;
                    }
                }
            }

            if (m_Products[i].Type == ProductType.NonConsumable)
            {
                if (product != null && product.hasReceipt)
                {
                    if (ReceiptIsValid(m_Products[i].Id, product.receipt, out var exception))
                    {
                        m_Products[i].Active = true;
                    }
                }
            }

            if (product != null && product.availableToPurchase)
            {
                m_Products[i].LocalizedPrice = product.metadata.localizedPriceString;
                m_Products[i].PriceDecimal = System.Decimal.ToInt32(product.metadata.localizedPrice);
                m_Products[i].IsoCurrencyCode = product.metadata.isoCurrencyCode;
            }
        }
    }

    public void BuyProduct(string productId, UnityAction<PurchaseState, IapProduct> onPurchaseComplete)
    {
        Debug.Log(this + "Buy Process Started for " + productId);

        OnPurchaseComplete = onPurchaseComplete;

        for (var i = 0; i < m_Products.Count; i++)
        {
            if (m_Products[i].Id.Equals(productId))
            {
                BuyProductWithId(m_Products[i].Id);
            }
        }
    }

    private void BuyProductWithId(string productId)
    {
        Debug.Log(this + "Buy product with id: " + productId);

        if (IsInitialized())
        {
            var product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log(this + "BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");

                OnPurchaseComplete?.Invoke(PurchaseState.Fail, null);
            }
        }
        else
        {
            Debug.Log(this + "BuyProductID FAIL. Store not initialized.");

            if (OnPurchaseComplete != null)
            {
                OnPurchaseComplete(PurchaseState.Fail, null);
            }
        }
    }

    public decimal GetPrice(string productId)
    {
        if (IsInitialized())
        {
            var product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                return System.Decimal.ToInt32(product.metadata.localizedPrice);
            }
            else
            {
                Debug.LogError("No products available");
            }
        }

        Debug.LogError("Not Initialized");
        return decimal.Parse(m_Products.First(p => string.Equals(p.Id, productId)).Price.Replace("$", string.Empty));
    }

    public string GetIsoCurrencyCode(string productId)
    {
        if (IsInitialized())
        {
            var product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                //return product.metadata.isoCurrencyCode;

                string price = product.metadata.localizedPriceString;
                string regex = @"[^0-9\.,]";
                var group = Regex.Match(price, regex).Groups;
                if (group.Count > 0) return group[0].Value;
                else return "$";
            }
            else
            {
                Debug.LogError("No products available");
            }
        }

        Debug.LogError("Not Initialized");
        return "$";
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.Log(this + "Buy Product failed for " + product.metadata.localizedTitle + " Failed. Reason: " + reason);
        OnPurchaseComplete?.Invoke(PurchaseState.Fail, null);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        var prodId = e.purchasedProduct.definition.id;
        var price = e.purchasedProduct.metadata.localizedPrice;
        var currency = e.purchasedProduct.metadata.isoCurrencyCode;

        var receipt = e.purchasedProduct.receipt;
        var receiptToJson = (Dictionary<string, object>)AFMiniJSON.Json.Deserialize(receipt);

        for (int i = 0; i < m_Products.Count; i++)
        {
            if (string.Equals(prodId, m_Products[i].Id, StringComparison.Ordinal))
            {
                bool validPurchase = ReceiptIsValid(m_Products[i].Id, receipt, out var exception);
                if (validPurchase)
                {
                    if (m_Products[i].Type == ProductType.Subscription || m_Products[i].Type == ProductType.NonConsumable)
                    {
                        m_Products[i].Active = true;
                    }

#if UNITY_IOS
                    var transactionId = (string) receiptToJson["TransactionID"];
                    AppManager.AppsFlyer.iOSRevenueTracking(prodId, decimal.Multiply(price, 0.63m).ToString(), currency, transactionId);
#elif UNITY_ANDROID
                    var payloadToJson = (Dictionary<string, object>)AFMiniJSON.Json.Deserialize((string)receiptToJson["Payload"]);
                    var purchaseData = (string)payloadToJson["json"];
                    var signature = (string)payloadToJson["signature"];
                    AppManager.AppsFlyer.AndroidRevenueTracking(signature, purchaseData, decimal.Multiply(price, 0.63m).ToString(), currency);
#endif

#if APP_METRICA
                    YandexAppMetricaReceipt yaReceipt = new YandexAppMetricaReceipt();
                    if (receipt != null)
                    {
                        yaReceipt.Data = purchaseData;
#if UNITY_ANDROID
                        yaReceipt.Signature = signature;
                        
#elif UNITY_IPHONE
                        yaReceipt.TransactionID = transactionId;
#endif
                    }
                    AppMetrica.AndroidRevenueTracking(price, currency, yaReceipt);
#endif

                    OnPurchaseComplete?.Invoke(PurchaseState.Success, m_Products[i]);

                    AppManager.Firebase.OnIAPComplete(m_Products[i].Id);
                }
                else
                {
                    OnPurchaseComplete?.Invoke(PurchaseState.Fail, null);
                }
                break;
            }
        }

        return PurchaseProcessingResult.Complete;
    }

    private bool ReceiptIsValid(string productId, string receipt, out IAPSecurityException exception)
    {
        exception = null;
        bool validPurchase = true;
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        CrossPlatformValidator validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);

        try
        {
            validator.Validate(receipt);
            Debug.Log(this + " Receipt is valid for " + productId);
        }
        catch (IAPSecurityException ex)
        {
            exception = ex;
            Debug.Log(this + " Receipt is NOT valid for " + productId);
            validPurchase = false;
        }
#endif
        return validPurchase;
    }
}

#else
public class IAPManager : MonoSingleton<IAPManager>
{
    [ShowInInspector] private List<IapProduct> products;

    public void Init(Transform parent = null)
    {
        AppManager.Iap = this;
        if (parent) transform.SetParent(parent);

        InitializeIap();
    }

    private void InitializeIap()
    {
        var settings = Resources.Load<IAPSetting>("IAPSetting");
        if (settings == null)
        {
            Debug.LogError("No products available");
            return;
        }

        products = settings.Products;
    }

    public void BuyProduct(string productId, UnityAction<PurchaseState, IapProduct> onPurchaseComplete)
    {
        var product = products.First(cond => string.Equals(cond.Id, productId));
        onPurchaseComplete?.Invoke(PurchaseState.Success, product);
    }

    public decimal GetPrice(string productId)
    {
        if (products != null)
        {
            return decimal.Parse(products.First(p => string.Equals(p.Id, productId)).Price.Replace("$", string.Empty), CultureInfo.InvariantCulture.NumberFormat);
        }

        Debug.LogError("No products available");
        return 0;
    }

    public string GetIsoCurrencyCode(string productId)
    {
        return "$";
    }
}

#endif

public enum PurchaseState
{
    Success,
    Fail
}