using System.Collections.Generic;

namespace SocialPlatforms
{
    public static class AchievementID
    {
        private static Dictionary<AchievementType, string> IOSIDS = new()
        {
            {AchievementType.Supporter, "supporter"},
            {AchievementType.BallCollector, "ballcollector"},
            {AchievementType.SportsFanatic, "sportsfanatic"},
            {AchievementType.FootballFrenzy, "footballfrenzy"},
            {AchievementType.CascadeChampion, "cascadechampion"},
            {AchievementType.PrecisionDropper, "precisiondropper"},
            {AchievementType.BallisticBlunder, "ballisticblunder"},
        };

        private static readonly Dictionary<AchievementType, string> AndroidIDS = new()
        {

            {AchievementType.Supporter, "CgkI8dy52uQXEAIQAQ"},
            {AchievementType.BallCollector, "CgkI8dy52uQXEAIQAg"},
            {AchievementType.SportsFanatic, "CgkI8dy52uQXEAIQAw"},
            {AchievementType.FootballFrenzy, "CgkI8dy52uQXEAIQBA"},
            {AchievementType.CascadeChampion, "CgkI8dy52uQXEAIQBQ"},
            {AchievementType.PrecisionDropper, "CgkI8dy52uQXEAIQBg"},
            {AchievementType.BallisticBlunder, "CgkI8dy52uQXEAIQBw"},

        };

/*
 * achievementSupporter,"CgkI8dy52uQXEAIQAQ"
achievementBallCollector,"CgkI8dy52uQXEAIQAg"
achievementSportsFanatic,"CgkI8dy52uQXEAIQAw"
achievementFootballFrenzy,"CgkI8dy52uQXEAIQBA"
achievementCascadeChampion,"CgkI8dy52uQXEAIQBQ"
achievementPrecisionDropper,"CgkI8dy52uQXEAIQBg"
achievementBallisticBlunder,"CgkI8dy52uQXEAIQBw"
 */
        public static string GetIDRespectfulToPlatform(AchievementType type)
        {
#if UNITY_ANDROID
            return AndroidIDS[type];
#elif UNITY_IOS
            return IOSIDS[type];
#endif
            return null;
        }

        public static string GetAchievementID(AchievementType type)
        {
            return GetIDRespectfulToPlatform(type);
        }
    }
}