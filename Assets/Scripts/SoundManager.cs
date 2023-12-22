using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    

    
    //singleton
    private static SoundManager _instance;
    public static SoundManager Instance => _instance;
    
    public AudioSource _gameMusicSource;
    public AudioSource _buttonSource;
    public AudioSource _popSource;




    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
    
    void Start()
    {
        _gameMusicSource.volume = 0.5f;
        _gameMusicSource.loop = true;
        _gameMusicSource.Play();
        BallManager.Instance.OnBallMerge += OnBallMerge;
        ButtonUI.Instance.OnButtonClick += OnButtonClick;
    }
    
    
    
    public void OnButtonClick()
    {
        //start the sound at time 0.2f
        _buttonSource.time = 0.2f;
        _buttonSource.PlayOneShot(_buttonSource.clip);
    }
    
    public void OnBallMerge()
    {
        _popSource.time = 0.3f;
        _popSource.PlayOneShot(_popSource.clip);
    }
    
}
