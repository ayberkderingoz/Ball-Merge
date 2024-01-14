using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{

    public Action OnButtonClick;

    [SerializeField] private GameObject _soundMute;
    [SerializeField] private GameObject _musicMute;
    
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
}
