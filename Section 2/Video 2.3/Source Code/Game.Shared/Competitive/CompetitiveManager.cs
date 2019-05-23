#if __ANDROID__ 
using Game.Android.Source.Competitive;
#elif __IOS__
using Game.iOS.Source.Competitive;
#endif
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Shared.Competitive
{
    /// <summary> This manager will control all achievements & leaderboard progress </summary>
    public class CompetitiveManager
    {
        /// <summary> Whether or not the manager is connected to the relevant service</summary>
        private Boolean _Connected;
        /// <summary> The active achievements </summary>
        private Achievement[] _Achievements;
        /// <summary> The controller responsible for reporting platform-specific achievements and leaderboards </summary>
        private CompetitiveController _PlatformController;
        /// <summary> The instance of the manager </summary>
        private static CompetitiveManager _Instance;

        /// <summary> The instance of the manager </summary>
        public static CompetitiveManager Instance => _Instance ?? (_Instance = new CompetitiveManager());
        /// <summary> The platform specific controller </summary>
        public CompetitiveController PlatformController => _PlatformController;

        /// <summary> Creates the manager & creates all achievements </summary>
        private CompetitiveManager()
        {
#if __ANDROID__
            _PlatformController = new PlayServicesController();
#elif __IOS__
            _PlatformController = new GameCenterController();
#endif
        }

        /// <summary> Loads the data for the achievements & leaderboards </summary>
        /// <param name="achievementIds"></param>
        public void LoadData(String[] achievementIds)
        {
            _Achievements = new Achievement[achievementIds.Length];
            for (Int32 i = 0; i < _Achievements.Length; i++)
            {
                _Achievements[i] = new Achievement(achievementIds[i]);
            }
        }

        /// <summary> Gets the achievement at the given index </summary>
        /// <returns></returns>
        public Achievement GetAchievement(Int32 index)
        {
            if (index < 0 || index >= _Achievements.Length) throw new IndexOutOfRangeException($"There is no achievement at the given index - {index}");
            return _Achievements[index];
        }

        /// <summary> Connects the competitive manager to the relevant service based on the platform </summary>
        public void Connect()
        {
            if (_Connected) return;
            _PlatformController.Connect(connected =>
            {
                _Connected = connected;
            });
        }

        /// <summary> Updates the achievement on either Google Play Services or Game Center </summary>
        /// <param name="achievement"></param>
        private void UpdatePlatformAchievement(Achievement achievement)
        {
            if (!_Connected) return;
            _PlatformController.UpdateAchievement(achievement);
        }

        /// <summary> Increments the progress for the given achievement </summary>
        /// <param name="achievementIndex"></param>
        /// <param name="progress"></param>
        public void IncrementAchievementProgress(Int32 index, Single progress)
        {
            if (_Achievements[index].Achieved) return;
            _Achievements[index].IncrementProgress(progress);
            UpdatePlatformAchievement(_Achievements[index]);
        }

        /// <summary> Sets the progress for the given achievement </summary>
        /// <param name="index"></param>
        /// <param name="progress"></param>
        public void SetAchievementProgress(Int32 index, Single progress)
        {
            if (_Achievements[index].Achieved) return;
            _Achievements[index].SetProgress(progress);
            UpdatePlatformAchievement(_Achievements[index]);
        }

        /// <summary> Resets the progress for the given achievement </summary>
        /// <param name="index"></param>
        public void ResetProgress(Int32 index)
        {
            if (_Achievements[index].Achieved) return;
            _Achievements[index].ResetProgress();
            UpdatePlatformAchievement(_Achievements[index]);
        }

        /// <summary> Views all the achievements in either Google Play or Game Center </summary>
        public void ViewAchievements()
        {
            if (!_Connected) return;
            _PlatformController.ViewAchievements();
        }
    }
}
