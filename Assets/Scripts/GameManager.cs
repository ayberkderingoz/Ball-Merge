using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //singleton
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    public bool isPaused = false;
    
    
    
    [SerializeField] private GameObject _restartPanel;
    [SerializeField] private GameObject _menuPanel;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
    public Action OnRestart;
    public void Restart()
    {
        OnRestart?.Invoke(); // TODO
        //restart the game
        BallManager.Instance.Restart();
        _restartPanel.SetActive(false);
        _menuPanel.SetActive(false);
        isPaused = false;
        ScoreManager.Instance.SaveMaxScore();
        ScoreManager.Instance.ResetScore();
        
        
    }



    public void GameOver()
    {
        isPaused = true;
        BallManager.Instance.Restart();
        OpenRestartPanel();
        ScoreManager.Instance.ShowGameOverScore();
    }
    public void OpenRestartPanel()
    {
        _restartPanel.SetActive(true);
    }

    public void OpenMenu()
    {
        _menuPanel.SetActive(true);
    }

    public void CloseMenu()
    {
        _menuPanel.SetActive(false);
    }
    
}
