#if __ANDROID__
using Android.Content;
using Android.Preferences;
using Engine.Android;
#elif __IOS__
using Foundation;
#endif
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Shared.Competitive
{
    /// <summary> The score entry for the leaderboard </summary>
    public class LeaderboardScore
    {
        /// <summary> The ID of the leaderboard - related to Google Play Services or Game Center </summary>
        public String Id { get; }
        /// <summary> The current score </summary>
        public Int32 Score { get; private set; }

        public LeaderboardScore(String id)
        {
            Id = id;
#if __ANDROID__
            ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(GameActivity.Instance);
            Score = preferences.GetInt($"{id}-Score", 0);
#elif __IOS__
            Score = (Int32)NSUserDefaults.StandardUserDefaults.IntForKey($"{id}-Score");
#endif
        }

        /// <summary> Saves the score </summary>
        public void Save()
        {
#if __ANDROID__
            ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(GameActivity.Instance);
            ISharedPreferencesEditor editor = preferences.Edit();
            editor.PutInt($"{Id}-Score", Score);
            editor.Apply();
#elif __IOS__
            NSUserDefaults.StandardUserDefaults.SetInt(Score, $"{Id}-Score");
            NSUserDefaults.StandardUserDefaults.Synchronize();
#endif
        }

        /// <summary> Updates the score if it is larger </summary>
        /// <param name="score"></param>
        public void UpdateScore(Int32 score)
        {
            if (score <= Score) return;
            Score = score;
            Save();
        }

        /// <summary> Resets the score </summary>
        public void Reset()
        {
            Score = 0;
            Save();
        }
    }
}
