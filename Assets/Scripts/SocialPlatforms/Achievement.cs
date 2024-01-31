using UnityEngine;

namespace SocialPlatforms
{
    public class Achievement
    {
        public bool Completed;
        public int CurrentSteps;
        public string ID;
        public bool IsIncremental;
        public int TotalSteps;
        public AchievementType Type;

        public Achievement(AchievementType type, bool isIncremental = false, int totalSteps = 1)
        {
            Type = type;
            ID = AchievementID.GetIDRespectfulToPlatform(type);
            IsIncremental = isIncremental;
            TotalSteps = totalSteps;
            CurrentSteps = PlayerPrefs.GetInt(Type + "_Progress", 0);
            Completed = CurrentSteps >= TotalSteps;
        }

        public void ReportProgress(int steps = 1)
        {
            if (Completed) return;
            CurrentSteps += steps;
            PlayerPrefs.SetInt(Type + "_Progress", CurrentSteps);
            if (CurrentSteps >= TotalSteps) SetCompleted();
        }

        public void SetCompleted()
        {
            if (Completed) return;
            CurrentSteps = TotalSteps;
            Completed = true;
            AchievementsManager.Instance.ReportAchievement(ID);
        }
    }
}