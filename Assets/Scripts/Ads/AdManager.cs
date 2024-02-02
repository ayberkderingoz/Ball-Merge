using System;
using System.Collections;
using DefaultNamespace;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Ads
{
    [Serializable]
    public enum AdState
    {
        NotReady,
        Loaded,
        Loading,
        Showing,
        Finished,
        RewardDeserved,
        FailedToShow,
        FailedToLoad,
        Hidden
    }

    public class AdManager : MonoBehaviour, IUnityAdsInitializationListener
    {
        private static string ShowAdsKey = "ShowAds";

        [Header("Config")] [Space, Header("Data")] [SerializeField]
        private string androidAppKey;

        [SerializeField] private string iosAppKey;

        [Space, Header("Activation Settings")] [SerializeField]
        public bool activateAds;

        [SerializeField] public bool testMode;

        [SerializeField] private bool activateInterstitial;
        [SerializeField] private bool activateRewarded;
        [SerializeField] private bool activateBanner;

        [Space, Header("States")] [SerializeField, ReadOnly]
        private AdState rewardedAdState;

        [SerializeField, ReadOnly] private AdState interstitialAdState;
        [SerializeField, ReadOnly] private AdState bannerAdState;

        private InterstitialAdController interstitialAdController;
        private BannerAdController bannerAdController;
        private RewardedAdController rewardedAdController;


        public bool AdsEnabled
        {
            get => activateAds && adsEnabled;
            set => adsEnabled = value;
        }

        private bool adsEnabled;

        private Action onInterstitialFinishedAction;
        private Action onRewardedAdFinishedAction;
        private Action onRewardedAdUserEarnedRewardAction;

        public static AdManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }


            StartCoroutine(InitializeAds());
        }


        private IEnumerator InitializeAds()
        {
            print("Activate ads: " + activateAds);
            if (!activateAds)
            {
                activateInterstitial = false;
                activateRewarded = false;
                activateBanner = false;
                yield break;
            }


            var isInternetAvailable = false;
            yield return StartCoroutine(Helpers.IsInternetAvailable((isAvailable) =>
            {
                isInternetAvailable = isAvailable;
            }));

            print("is internet available: " + isInternetAvailable);
            if (!isInternetAvailable)
            {
                activateInterstitial = false;
                activateRewarded = false;
                activateBanner = false;
                yield break;
            }


            AdsEnabled = PlayerPrefs.GetInt(ShowAdsKey, 1) == 1;
            print("Ads enabled: " + AdsEnabled);
            if (!AdsEnabled)
            {
                activateInterstitial = false;
                activateRewarded = true;
                activateBanner = false;
                yield break;
            }


#if UNITY_ANDROID
        var appKey = androidAppKey;
#elif UNITY_IPHONE
            var appKey = iosAppKey;
#else
        var appKey = "unexpected_platform";
#endif


            interstitialAdController = GetComponent<InterstitialAdController>();
            bannerAdController = GetComponent<BannerAdController>();
            rewardedAdController = GetComponent<RewardedAdController>();


            print("Initialize ads with game id: " + appKey);

            Advertisement.Initialize(appKey, testMode, this);
        }


        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            print("InitializeIronSource: Initialization Failed - " + error + ", " + message);
        }

        public void OnInitializationComplete()
        {
            Debug.Log("InitializeIronSource: SdkInitializationCompletedEvent");

            if (activateInterstitial)
            {
                interstitialAdController.OnStateChanged = OnInterstitialAdStateChanged;
                interstitialAdController.Initialize();
            }

            if (activateRewarded)
            {
                rewardedAdController.OnStateChanged = OnRewardedAdStateChanged;
                rewardedAdController.Initialize();
            }

            if (activateBanner)
            {
                bannerAdController.OnStateChanged = state => bannerAdState = state;
                bannerAdController.Initialize();
            }
        }


        public void ShowInterstitialAd(Action interstitialAdFinishedAction = null)
        {
            print("is interstitial ad state: " + interstitialAdState);

            if (!AdsEnabled) return;
            if (interstitialAdState == AdState.Loaded)
            {
                onInterstitialFinishedAction = interstitialAdFinishedAction;
                interstitialAdController.Show();
            }
        }


        public void ShowRewardedAd(Action rewardedAdFinishedAction = null)
        {
            print("is rewarded ad ready: " + rewardedAdState);

            //if (!AdsEnabled) return;
            if (rewardedAdState == AdState.Loaded)
            {
                onRewardedAdFinishedAction = rewardedAdFinishedAction;
                onRewardedAdUserEarnedRewardAction = rewardedAdFinishedAction;
                rewardedAdController.Show();
            }
        }
        


        private void OnInterstitialAdStateChanged(AdState state)
        {
            interstitialAdState = state;
            switch (state)
            {
                case AdState.Finished:
                    OnInterstitialAdFinished();
                    break;
            }
        }


        private void OnRewardedAdStateChanged(AdState state)
        {
            rewardedAdState = state;
            switch (state)
            {
                case AdState.Finished:
                    OnRewardedAdFinished();
                    break;
                case AdState.RewardDeserved:
                    OnRewardedAdUserEarnedReward();
                    break;
            }
        }


        private void OnRewardedAdFinished()
        {
            onRewardedAdFinishedAction?.Invoke();
            onRewardedAdFinishedAction = null;
        }

        private void OnRewardedAdUserEarnedReward()
        {
            onRewardedAdUserEarnedRewardAction?.Invoke();
            onRewardedAdUserEarnedRewardAction = null;
        }

        private void OnInterstitialAdFinished()
        {
            onInterstitialFinishedAction?.Invoke();
            onInterstitialFinishedAction = null;
        }

        public void RemoveAds()
        {
            AdsEnabled = false;
            bannerAdController.Hide();
            PlayerPrefs.SetInt(ShowAdsKey, 0);
        }
    }
}