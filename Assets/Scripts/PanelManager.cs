using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    //singleton
    private static PanelManager _instance;
    public static PanelManager Instance => _instance;
    
    [SerializeField] private GameObject _restartPanel;
    [SerializeField] private GameObject _gameOverPanel;

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
        _restartPanel.SetActive(false);
        _gameOverPanel.SetActive(false);
        GameManager.Instance.OnGameOver += OnGameOver;
        GameManager.Instance.OnRestart += OnRestart;
    }

    private void OnGameOver()
    {
        OpenGameOverPanel();
    }
    private void OnRestart()
    {
        CloseRestartPanel();
        CloseGameOverPanel();
        OnPanelClose?.Invoke();
    }
    
    public void OpenRestartPanel()
    {
        OnPanelOpen?.Invoke();
        _restartPanel.SetActive(true);
        
    }
    public void CloseRestartPanel()
    {
        OnPanelClose?.Invoke();
        _restartPanel.SetActive(false);
        
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
