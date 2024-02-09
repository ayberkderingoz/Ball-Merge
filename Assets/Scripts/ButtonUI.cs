using System;
using System.Collections;
using System.Collections.Generic;
using SocialPlatforms;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{

    public Action OnButtonClick;

    [SerializeField] private GameObject _soundMute;
    [SerializeField] private GameObject _musicMute;
    [SerializeField] private GameObject _vibrationMute;
    
    [SerializeField] private GameObject _settingsMenu;
    
    private bool _isSoundMute;
    private bool _isMusicMute;
    
    
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


    public void OpenShop()
    {
        OnButtonClick?.Invoke();
        PanelManager.Instance.OpenShopPanel();
    }
    
    public void CloseShop()
    {
        OnButtonClick?.Invoke();
        PanelManager.Instance.CloseShopPanel();
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
    public void OpenSettings()
    {
        OnButtonClick?.Invoke();
        _settingsMenu.SetActive(true);
    }
    public void CloseSettings()
    {
        OnButtonClick?.Invoke();
        _settingsMenu.SetActive(false);
    }
    
 
    public void MuteSFX()
    {
        OnButtonClick?.Invoke();
        _isSoundMute = !_isSoundMute;
        if (_isSoundMute)
        {
            SoundManager.Instance.MuteSFX();
            _soundMute.SetActive(true);
        }
        else
        {
            SoundManager.Instance.UnMuteSFX();
            _soundMute.SetActive(false);
        }
    }
    public void MuteMusic()
    {
        OnButtonClick?.Invoke();
        _isMusicMute = !_isMusicMute;
        if (_isMusicMute)
        {
            SoundManager.Instance.MuteGameMusic();
            _musicMute.SetActive(true);
        }
        else
        {
            SoundManager.Instance.UnMuteGameMusic();
            _musicMute.SetActive(false);
        }
    }
    
    
    public void MuteVibration()
    {
        OnButtonClick?.Invoke();
        _vibrationMute.SetActive(!_vibrationMute.activeSelf);
        Vibrator.SetVibrationEnabled(!_vibrationMute.activeSelf);
        
    }
    
    public void OpenLeaderboard()
    {
        OnButtonClick?.Invoke();
        SocialPlatformManager.Instance.ShowLeaderboards();
    }
    
    
}
