using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Purchasing;

public class ShopManager : MonoBehaviour, IStoreListener
{
    
    private IStoreController _storeController;
    
    
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        print("Success");
        _storeController = controller;
        CheckNonConsumable("remove_ads");
    }

    void CheckNonConsumable(string id) {
        if (_storeController!=null)
        {
            var product = _storeController.products.WithID(id);
            if (product!=null)
            {
                if (product.hasReceipt)//purchased
                {
                    RemoveAds();
                }
                else {
                    ShowAds();
                }
            }
        }
    }
    void RemoveAds()
    {
        DisplayAds(false);
    }
    void ShowAds()
    {
        DisplayAds(true);

    }
    
    [SerializeField] private GameObject AdsPurchasedPanel;
    void DisplayAds(bool x)
    {
        if (!x)
        {
            AdsPurchasedPanel.SetActive(true);
            //Ads class things
        }
        else
        {
            AdsPurchasedPanel.SetActive(false);
            //Ads class things
        }
    }
    
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        throw new System.NotImplementedException();
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new System.NotImplementedException();
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        throw new System.NotImplementedException();
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        throw new System.NotImplementedException();
    }

}
