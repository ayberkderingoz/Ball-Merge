using System;
using Ads;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAdController : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string androidAdUnitId = "Rewarded_Android";
    [SerializeField] private string iOSAdUnitId = "Rewarded_iOS";
    private string adUnitId;

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

        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        adUnitId = iOSAdUnitId;
#elif UNITY_ANDROID
        adUnitId = androidAdUnitId;
#endif
        print("RewardedAdController Initialize");
        Load();
    }

    private void Load()
    {
        Debug.Log("Loading Ad: " + adUnitId);
        SetState(AdState.Loading);

        Advertisement.Load(adUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Ad Loaded: " + placementId);

        if (!placementId.Equals(adUnitId)) return;
        Debug.Log("Rewarded OnUnityAdsAdLoaded");
        SetState(AdState.Loaded);
    }


    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (!placementId.Equals(this.adUnitId) ||
            !showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED)) return;
        Debug.Log("Unity Ads Rewarded Ad Completed");
        SetState(AdState.RewardDeserved);
        Load();
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {placementId}: {error.ToString()} - {message}");
        SetState(AdState.FailedToLoad);
        Load();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {placementId}: {error.ToString()} - {message}");
        SetState(AdState.FailedToShow);
        Load();
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        print("Showing ad unit: " + adUnitId);
        SetState(AdState.Showing);
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        print("Clicked ad unit: " + adUnitId);
    }


    public void Show()
    {
        print("Showing ad unit: " + adUnitId);
        Advertisement.Show(adUnitId, this);
    }
}