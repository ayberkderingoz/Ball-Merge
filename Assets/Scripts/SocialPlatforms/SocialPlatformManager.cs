using System;
using UnityEngine;

#if UNITY_ANDROID
#elif UNITY_IOS
using UnityEngine.SocialPlatforms.GameCenter;
#endif

namespace SocialPlatforms
{
    public class SocialPlatformManager : MonoBehaviour
    {
        public static SocialPlatformManager Instance;

        public Action OnLoginSuccess;
        private bool SuccessfullLogin;
        public static string UserId { get; private set; }


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            AuthenticateSocialPlatform();
        }

        public void AuthenticateSocialPlatform()
        {
            if (SuccessfullLogin) return;
#if UNITY_EDITOR
            print("Social platforms are not supported in the editor.");
#elif UNITY_ANDROID
            PlayGamesPlatform.Activate();
            PlayGamesPlatform.Instance.Authenticate((signInStatus) =>
            {
                var status = false || signInStatus == SignInStatus.Success;
                AuthenticateSocialCallBack(status);
            });
#elif UNITY_IOS
            Social.localUser.Authenticate(AuthenticateSocialCallBack);
            GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
#endif
        }

        private void AuthenticateSocialCallBack(bool success)
        {
            if (success)
            {
                print("Successfully logged into social platform");
                // Make request to get loaded achievements and register a callback for processing them
                UserId = Social.localUser.id;
                print("Social platform UserID: " + Social.localUser.id);
                print("Social platform UserName: " + Social.localUser.userName);
                SuccessfullLogin = true;

                OnLoginSuccess?.Invoke();
            }
            else
            {
                print("Failed to log into social platform");
            }
        }

        public void ShowLeaderboards()
        {
            Debug.Log("ShowAchievement");
#if UNITY_EDITOR
            Debug.LogError("Cannot show Leaderboard UI in Editor");
#elif UNITY_IOS
				Social.ShowLeaderboardUI();
#elif UNITY_ANDROID
				PlayGamesPlatform.Instance.ShowLeaderboardUI();
#endif
        }

        public void ShowAchievements()
        {
            Debug.Log("ShowAchievement");
#if UNITY_EDITOR
            Debug.LogError("Cannot show achievement UI in Editor");
#elif UNITY_IOS
				Social.ShowAchievementsUI ();
#elif UNITY_ANDROID
				PlayGamesPlatform.Instance.ShowAchievementsUI();
#endif
        }

        public void ResetAllAchievements(Action<bool> result)
        {
            result?.Invoke(false);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}