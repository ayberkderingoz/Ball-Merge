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
    
    public void Restart()
    {
        //restart the game
        BallManager.Instance.Restart();
        _restartPanel.SetActive(false);
        _menuPanel.SetActive(false);
        isPaused = false;
        ScoreManager.Instance.SaveMaxScore();
        ScoreManager.Instance.ResetScore();
        
        //TODO: restart the score
        
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
