using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{

    public Action OnButtonClick;
    
    
    //singleton
    private static ButtonUI _instance;
    public static ButtonUI Instance => _instance;
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
    public void Restart()
    {
        Debug.Log("Restart");
        GameManager.Instance.Restart();
        OnButtonClick?.Invoke();
    }
    
    //open menu
    public void OpenMenu()
    {
        OnButtonClick?.Invoke();
        PanelManager.Instance.OpenRestartPanel();
        
    }

    public void CloseMenu()
    {
        OnButtonClick?.Invoke();
        PanelManager.Instance.CloseRestartPanel();  
    }
    
    public void ContinueWithAd()
    {
        GameManager.Instance.ContinueWithAd();
    }

    public void StartGame()
    {
        
        OnButtonClick?.Invoke();
        SceneManager.LoadScene("Game Scene");
        
        
        
    }
}
