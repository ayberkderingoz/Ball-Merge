using System;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

namespace Ads
{
    public class BannerAdController : MonoBehaviour
    {
        [SerializeField] BannerPosition bannerPosition = BannerPosition.BOTTOM_CENTER;

        [SerializeField] string androidAdUnitId = "Banner_Android";
        [SerializeField] string iOSAdUnitId = "Banner_iOS";
        string adUnitId = null; // This will remain null for unsupported platforms.
        public Action<AdState> OnStateChanged;
        private AdState state;

        private void SetState(AdState state)
        {
            this.state = state;
            OnStateChanged?.Invoke(this.state);
        }

        public void Initialize()
        {
            // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
            adUnitId = iOSAdUnitId;
#elif UNITY_ANDROID
        adUnitId = androidAdUnitId;
#endif
            Advertisement.Banner.SetPosition(bannerPosition);

            Load();
        }

        private void Load()
        {
            BannerLoadOptions options = new BannerLoadOptions
            {
                loadCallback = OnBannerLoaded,
                errorCallback = OnBannerError
            };

            Advertisement.Banner.Load(adUnitId, options);
        }

        public void Hide()
        {
            Advertisement.Banner.Hide();
        }

        private void OnBannerLoaded()
        {
            Debug.Log("Banner loaded");
            Advertisement.Banner.Show(adUnitId);

            SetState(AdState.Loaded);
        }

        private void OnBannerError(string message)
        {
            Debug.Log($"Banner Error: {message}");
            SetState(AdState.FailedToLoad);
        }

        private void OnBannerClicked()
        {
            print("Banner Clicked");
        }

        private void OnBannerShown()
        {
            print("Banner Shown");
            SetState(AdState.Showing);
        }

        private void OnBannerHidden()
        {
            print("Banner Hidden");
            SetState(AdState.Hidden);
        }
    }
}