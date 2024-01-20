using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    public bool isPaused = false;
    
    private LoadBanner _loadBanner;
    private LoadInterstitial _loadInterstitial;
    private LoadRewarded _loadRewarded;
    public GameObject adsManager;
    
    
    [SerializeField] private bool _adsEnabled = true;
    public  bool AdsEnabled => _adsEnabled;




    private int _restartCount = 0;
    
    
    
    

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    void Start()
    {

        if (PlayerPrefs.HasKey("RemoveAds"))
        {
            _adsEnabled = false;
        }
        else
        {
            _adsEnabled = true;
        }

        _loadRewarded = adsManager.GetComponent<LoadRewarded>();
        if (!_adsEnabled) return;
        _loadBanner = adsManager.GetComponent<LoadBanner>();
        _loadInterstitial = adsManager.GetComponent<LoadInterstitial>();
        _loadBanner.loadBanner();
    }
    public Action OnRestart;
    public void Restart()
    {
        OnRestart?.Invoke();
        isPaused = false;
        _restartCount++;
        if (_restartCount % 3 == 0 && _adsEnabled)
        {
            _loadInterstitial.LoadAd();
        }

    }
    
    public Action OnGameOver;
    public void GameOver()
    {
        isPaused = true;
        OnGameOver?.Invoke();
    }

    public void ContinueWithAd()
    {
        //if (!_adsEnabled) return; //TODO: bu durmali mi durmamali mi emin olmayan??
        _loadRewarded.LoadAd();
    }
    public void RemoveAds()
    {
        _adsEnabled = false;
        _loadBanner.HideBannerAd();

        PlayerPrefs.SetInt("RemoveAds", 1);
        PlayerPrefs.Save();
        
        PanelManager.Instance.DisableAdsButton();
    }
    
}
