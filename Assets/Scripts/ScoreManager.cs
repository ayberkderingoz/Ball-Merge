using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI americanScoreText;
        [SerializeField] private TextMeshProUGUI gameOverScoreText;
        
        
        private static ScoreManager _instance;
        public static ScoreManager Instance => _instance;
        
        private void Awake()
        {
                if (_instance == null)
                {
                        _instance = this;
                }
        }

        void Start()
        {
                GameManager.Instance.OnRestart += OnRestart;
                GameManager.Instance.OnGameOver += OnGameOver;
        }
        
        private void OnRestart()
        {
                SaveMaxScore();
                ResetScore();
        }

        private void OnGameOver()
        {
                ShowGameOverScore();
        }
        
        public void AddScore(int score)
        {
                scoreText.text = (int.Parse(scoreText.text) + score).ToString();
        }

        public void AddAmericanFootballScore()
        {
                americanScoreText.text = (int.Parse(americanScoreText.text) + 1).ToString();
        }
        
        public void ResetScore()
        {
                scoreText.text = "0";
                americanScoreText.text = "0";
        }
        
        
        
        public void SaveMaxScore()
        {
                var maxScore = PlayerPrefs.GetInt("MaxScore");
                var currentScore = int.Parse(scoreText.text);
                if (currentScore > maxScore)
                {
                        PlayerPrefs.SetInt("MaxScore",currentScore);
                }
        }
        
        public void LoadMaxScore()
        {
                scoreText.text = PlayerPrefs.GetInt("MaxScore").ToString();
        }
        
        public void ShowGameOverScore()
        {
                gameOverScoreText.text = scoreText.text;
        }
        
        
}
