#if __ANDROID__
using Android.Content;
using Android.Preferences;
using Engine.Android;
#elif __IOS__
using Foundation;
#endif
using Game.Shared.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Shared.Competitive
{
    /// <summary> The achievement that the player can earn through playing the game </summary>
    public class Achievement
    {
        /// <summary> The ID of the achievement - related to Google Play Services or Game Center </summary>
        public String Id { get; }
        /// <summary> The percentage that the player has won for the achievement </summary>
        public Single PercentageComplete { get; protected set; }
        /// <summary> Whether or not the achievement has been achieved </summary>
        public Boolean Achieved { get; protected set; }

        /// <summary> Creates the achievement & loads data for the current progress </summary>
        /// <param name="id"></param>
        public Achievement(String id)
        {
            Id = id;
#if __ANDROID__
            ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(GameActivity.Instance);
            PercentageComplete = preferences.GetFloat($"{id}-Percentage", 0);
            Achieved = preferences.GetBoolean($"{id}-Achieved", false);
#elif __IOS__
            PercentageComplete = NSUserDefaults.StandardUserDefaults.FloatForKey($"{id}-Percentage");
            Achieved = NSUserDefaults.StandardUserDefaults.BoolForKey($"{id}-Achieved");
#endif
        }

        /// <summary> Sets the progress of the achievement </summary>
        /// <param name="percentageProgress"></param>
        public void SetProgress(Single percentageProgress)
        {
            PercentageComplete = percentageProgress;
            PercentageComplete = Math.Min(PercentageComplete, 1);
            PercentageComplete = Math.Max(PercentageComplete, 0);
            if (Math.Abs(PercentageComplete - 1) <= Constants.EPSILON) Achieved = true;
            Save();
        }

        /// <summary> Increments the progress for the achievement </summary>
        /// <param name="percentageProgress"></param>
        public void IncrementProgress(Single percentageProgress)
        {
            SetProgress(percentageProgress + PercentageComplete);
        }

        /// <summary> Resets the progress of the achievement </summary>
        public void ResetProgress()
        {
            PercentageComplete = 0;
            Save();
        }

        /// <summary> Saves the achievement to storage </summary>
        public void Save()
        {
#if __ANDROID__
            ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(GameActivity.Instance);
            ISharedPreferencesEditor editor = preferences.Edit();
            editor.PutFloat($"{Id}-Percentage", PercentageComplete);
            editor.PutBoolean($"{Id}-Achieved", Achieved);
            editor.Apply();
#elif __IOS__
            NSUserDefaults.StandardUserDefaults.SetFloat(PercentageComplete, $"{Id}-Percentage");
            NSUserDefaults.StandardUserDefaults.SetBool(Achieved, $"{Id}-Achieved");
            NSUserDefaults.StandardUserDefaults.Synchronize();
#endif
        }
    }
}
