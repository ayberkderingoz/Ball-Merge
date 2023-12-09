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
        isPaused = false;
        //TODO: restart the score
        
    }



    public void GameOver()
    {
        isPaused = true;
        OpenRestartPanel();
    }
    public void OpenRestartPanel()
    {
        _restartPanel.SetActive(true);
    }
}
