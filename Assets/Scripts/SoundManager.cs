using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{


    [SerializeField] private AudioMixer mixer;

    
    //singleton
    private static SoundManager _instance;
    public static SoundManager Instance => _instance;
    
    public AudioSource _gameMusicSource;
    public AudioSource _buttonSource;
    public AudioSource _popSource;
    
    
    const string SFX_VOLUME = "SFX_Volume";
    const string MUSIC_VOLUME = "MUSIC_Volume";
    

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
        //null check for mixer
        if (mixer == null)
        {
            Debug.LogError("No mixer found");
        }
        
    }

    public void MuteSFX()
    {
        mixer.SetFloat(SFX_VOLUME, -80f);
    }
    public void MuteGameMusic()
    {
        mixer.SetFloat(MUSIC_VOLUME, -80f);
    }
    public void UnMuteSFX()
    {
        mixer.SetFloat(SFX_VOLUME, 0f);
    }
    public void UnMuteGameMusic()
    {
        mixer.SetFloat(MUSIC_VOLUME, 0f);
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
