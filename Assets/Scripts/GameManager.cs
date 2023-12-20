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
        _loadBanner = adsManager.GetComponent<LoadBanner>();
        _loadInterstitial = adsManager.GetComponent<LoadInterstitial>();
        _loadRewarded = adsManager.GetComponent<LoadRewarded>();
        _loadBanner.loadBanner();
    }
    public Action OnRestart;
    public void Restart()
    {
        OnRestart?.Invoke();
        isPaused = false;
        _restartCount++;
        if (_restartCount % 3 == 0)
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
        PanelManager.Instance.CloseGameOverPanel();
        _loadRewarded.LoadAd();
    }
    
}
