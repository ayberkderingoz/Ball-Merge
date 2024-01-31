using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Ads
{
    public class InterstitialAdController : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        [SerializeField] private string androidAdUnitId = "Interstitial_Android";
        [SerializeField] private string iOSAdUnitId = "Interstitial_iOS";
        [SerializeField] private int retryLoading = 5;

        private string adUnitId;

        private int retryLoadingCounter;

        public bool IsReady { get; private set; }

        public Action<AdState> OnStateChanged;
        private AdState state;

        private void SetState(AdState state)
        {
            this.state = state;
            OnStateChanged?.Invoke(this.state);
        }
        

        public void Initialize()
        {
            SetState(AdState.NotReady);

#if UNITY_IOS
            adUnitId = iOSAdUnitId;
#elif UNITY_ANDROID
            adUnitId = androidAdUnitId;
#endif

            retryLoadingCounter = retryLoading;
            print("InterstitialAdController Initialize");

            Load();
        }

        private void Load()
        {

            retryLoadingCounter--;
            if (retryLoadingCounter <= 0)
            {
                print("cant load the ad");
                SetState(AdState.FailedToLoad);
                return;
            }

            SetState(AdState.Loading);
            print("Loading ad");
            Advertisement.Load(adUnitId, this);
        }


        public void OnUnityAdsAdLoaded(string placementId)
        {
            if (!placementId.Equals(adUnitId)) return;

            retryLoadingCounter = retryLoading;
            Debug.Log("Interstitial OnUnityAdsAdLoaded");
            SetState(AdState.Loaded);
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.Log("Interstitial OnUnityAdsFailedToLoad");
            SetState(AdState.FailedToLoad);
            Load();
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.Log("Interstitial OnUnityAdsShowFailure");
            SetState(AdState.FailedToShow); 
            Load();
        }

        public void OnUnityAdsShowStart(string placementId)
        {
            Debug.Log("Interstitial OnUnityAdsShowStart");
            SetState(AdState.Showing);
        }

        public void OnUnityAdsShowClick(string placementId)
        {
            Debug.Log("Interstitial OnUnityAdsShowClick");
            
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (placementId.Equals(this.adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                Debug.Log("Interstitial OnUnityAdsClosed");
                SetState(AdState.Finished);
                Load();
            }
        }


        public void Show()
        {
            Debug.Log("Showing interstitial ad: " + adUnitId);
            Advertisement.Show(adUnitId, this);
        }
    }
}