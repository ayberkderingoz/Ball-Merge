using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANDROID
using GooglePlayGames;
#endif

namespace SocialPlatforms
{
    public class LeaderboardsManager : MonoBehaviour
    {
        public static LeaderboardsManager Instance;

        private readonly Dictionary<LeaderboardType, string> AndroidIDS = new()
        {
            {LeaderboardType.HighScore, "CgkI8dy52uQXEAIQCA"},
            
        };

        private Dictionary<LeaderboardType, string> IOSIDS = new()
        {
            {LeaderboardType.HighScore, "highscore"},

        };


        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            SocialPlatformManager.Instance.OnLoginSuccess += OnLoginSuccess;
        }

        private void OnLoginSuccess()
        {
        }


        public void ReportScore(LeaderboardType leaderboardType, long score)
        {
            if (Social.localUser.authenticated == false)
            {
                SocialPlatformManager.Instance.AuthenticateSocialPlatform();
                return;
            }
#if UNITY_IOS
            Social.ReportScore(score, GetLeaderboardID(leaderboardType), success =>
            {
                if (success)
                {
                    Debug.Log("Score submitted");
                }
                else
                {
                    Debug.Log("Failed to submit score");
                }
            });
#elif UNITY_ANDROID

            PlayGamesPlatform.Instance.ReportScore(score, GetLeaderboardID(leaderboardType), success =>
            {
                if (success)
                    Debug.Log("Score submitted");
                else
                    Debug.Log("Failed to submit score");
            });
#endif
        }


        public void ShowLeaderboard()
        {
            Social.ShowLeaderboardUI();
        }

        private string GetLeaderboardID(LeaderboardType type)
        {
#if UNITY_ANDROID
            return AndroidIDS[type];
#elif UNITY_IOS
            return IOSIDS[type];
#endif
            return null;
        }
    }
}