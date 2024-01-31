using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    //singleton
    private static PanelManager _instance;
    public static PanelManager Instance => _instance;
    
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private GameObject _addsButton;

    public Action OnPanelOpen;
    public Action OnPanelClose;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
    
    
    void Start()
    {
        //_gameOverPanel.SetActive(false);
        _shopPanel.SetActive(false);
        
        GameManager.Instance.OnGameOver += OnGameOver;
        GameManager.Instance.OnRestart += OnRestart;
    }

    private void OnGameOver()
    {
        OpenGameOverPanel();
    }
    private void OnRestart()
    {
        CloseGameOverPanel();
        OnPanelClose?.Invoke();
    }
    

    
    
    public void OpenShopPanel()
    {
        OnPanelOpen?.Invoke();
        _shopPanel.SetActive(true);
        if (!GameManager.Instance.AdsEnabled)
            _addsButton.SetActive(false);

    }
    public void CloseShopPanel()
    {
        OnPanelClose?.Invoke();
        _shopPanel.SetActive(false);
        
    }
    
    public void DisableAdsButton()
    {
        Invoke(nameof(DisableAdsButtonDelayed), 0.5f);
    }

    private void DisableAdsButtonDelayed()
    {
        _addsButton.SetActive(false);
    }
    
    public void EnableAdsButton()
    {
        _addsButton.SetActive(true);
    }
    
    public void OpenGameOverPanel()
    {
        _gameOverPanel.SetActive(true);
        OnPanelOpen?.Invoke();
    }
    public void CloseGameOverPanel()
    {
        _gameOverPanel.SetActive(false);
        OnPanelClose?.Invoke();
    }
    
    
    
}
