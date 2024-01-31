using System;
using System.Collections;
using System.Collections.Generic;
using Ads;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    public bool isPaused = false;

    private int _restartCount = 0;

    
    
    

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        Application.targetFrameRate = 60;
    }

    void Start()
    {


    }
    public Action OnRestart;
    public void Restart()
    {
        OnRestart?.Invoke();
        isPaused = false;
        _restartCount++;
        if (_restartCount % 3 == 0 && AdManager.Instance.AdsEnabled)
        {
            AdManager.Instance.ShowInterstitialAd();
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
        AdManager.Instance.ShowRewardedAd( PanelManager.Instance.CloseGameOverPanel);
    }
    public void RemoveAds()
    {
        AdManager.Instance.RemoveAds();
        PanelManager.Instance.DisableAdsButton();
    }
    
}
