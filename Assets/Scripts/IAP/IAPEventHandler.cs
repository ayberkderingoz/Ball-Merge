using System;
using System.Collections.Generic;
using Ads;
using SocialPlatforms;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace IAP
{
    public class IAPEventHandler : MonoBehaviour
    {
        public static IAPEventHandler Instance;
        [SerializeField] private GameObject waitingPanel;
        [SerializeField] private GameObject failText;
        [SerializeField] private GameObject loadingImage;
        [SerializeField] private List<GameObject> objectsToHideAfterRemoveAds;

        private void Awake()
        {
            Instance = this;
        }

        public void OpenWaitingScreen()
        {
            failText.SetActive(false);
            waitingPanel.SetActive(true);
            loadingImage.SetActive(true);
        }


        public void OnPurchaseCompleted(Product p)
        {
            print("Purchase completed: " + p.definition.id);
            print("Receipt: " + p.receipt);
            AchievementsManager.Instance.GetAchievement(AchievementType.Supporter).SetCompleted();

            var iapItemType = IAPItems.GetIAPItemType(p.definition.id);

            switch (iapItemType)
            {
                case IAPItemType.RemoveAds:
                    BoughtRemoveAds();
                    break;
                case IAPItemType.Unknown:
                    print("Unknown IAP item type: " + p.definition.id);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            waitingPanel.SetActive(false);
        }

        private void BoughtRemoveAds()
        {
            AdManager.Instance.RemoveAds();
            objectsToHideAfterRemoveAds.ForEach(o => o.SetActive(false));
        }


        public void OnPurchaseFailed(Product p, PurchaseFailureDescription r)
        {
            loadingImage.SetActive(false);
            failText.SetActive(true);
            switch (r.reason)
            {
                case PurchaseFailureReason.PurchasingUnavailable:
                    break;
                case PurchaseFailureReason.ExistingPurchasePending:
                    break;
                case PurchaseFailureReason.ProductUnavailable:
                    break;
                case PurchaseFailureReason.SignatureInvalid:
                    break;
                case PurchaseFailureReason.UserCancelled:
                    break;
                case PurchaseFailureReason.PaymentDeclined:
                    break;
                case PurchaseFailureReason.DuplicateTransaction:
                    break;
                case PurchaseFailureReason.Unknown:
                    break;
            }

            print("Purchase failed: " + p.definition.id + " " + r);
            print("Failure reason: " + r);

            waitingPanel.SetActive(false);
            loadingImage.SetActive(true);
        }

        public void RestoreButton()
        {
        }
    }
}