using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadTopScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI maxScoreText;
    [SerializeField] private TextMeshProUGUI americanScoreText;
    

    void Start()
    {
        LoadMaxScore();
        LoadAmericanScore();
    }
    
    private void LoadMaxScore()
    {
        maxScoreText.text = PlayerPrefs.GetInt("MaxScore").ToString();
    }
    
    private void LoadAmericanScore()
    {
        americanScoreText.text = PlayerPrefs.GetInt("MaxAmericanScore").ToString();
    }
}
