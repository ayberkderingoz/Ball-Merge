using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;
#if UNITY_ANDROID
using GooglePlayGames;

#elif UNITY_IOS
using UnityEngine.SocialPlatforms.GameCenter;
#endif

namespace SocialPlatforms
{
    public class AchievementsManager : MonoBehaviour
    {
        public static AchievementsManager Instance;

        
        private static Achievement Supporter => new(AchievementType.Supporter);
        private static Achievement BallCollector => new(AchievementType.BallCollector);
        private static Achievement SportsFanatic => new(AchievementType.SportsFanatic);
        private static Achievement FootballFrenzy => new(AchievementType.FootballFrenzy);
        private static Achievement CascadeChampion => new(AchievementType.CascadeChampion);
        private static Achievement PrecisionDropper => new(AchievementType.PrecisionDropper);
        private static Achievement BallisticBlunder => new(AchievementType.BallisticBlunder);
        


        private static List<Achievement> Achievements => new()
        {
            Supporter,
            BallCollector,
            SportsFanatic,
            FootballFrenzy,
            CascadeChampion,
            PrecisionDropper,
            BallisticBlunder,
            
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
            LoadAchievements();
        }

        private void LoadAchievements()
        {
            Social.LoadAchievements(LoadAchievementsCallBack);
        }

        private void LoadAchievementsCallBack(IAchievement[] achievements)
        {
            if (achievements.Length == 0)
                print("Error: no achievements found");
            else
                foreach (var achievement in achievements)
                {
                    var a = Achievements.First(x => x.ID == achievement.id);
                    a.Completed = achievement.completed;
                }
        }

        public Achievement GetAchievement(AchievementType type)
        {
            return Achievements.First(x => x.ID == AchievementID.GetIDRespectfulToPlatform(type));
        }

        public void ReportAchievement(string achievementID)
        {
            if (Social.localUser.authenticated == false)
            {
                SocialPlatformManager.Instance.AuthenticateSocialPlatform();
                return;
            }

#if UNITY_ANDROID
            PlayGamesPlatform.Instance.ReportProgress(achievementID, 100, AchievementReportCallBack);
#elif UNITY_IOS
            Social.ReportProgress(achievementID, 100, AchievementReportCallBack);
#endif
        }

        private void AchievementReportCallBack(bool result)
        {
            if (result)
            {
                print("Successfully reported achievement ");
                LoadAchievements();
            }
            else
            {
                print("Failed to report achievement ");
            }
        }
    }
}