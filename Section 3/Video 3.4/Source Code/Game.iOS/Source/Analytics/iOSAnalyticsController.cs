using Game.Shared.Analytics;
using Google.Analytics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.iOS.Source.Analytics
{
    /// <summary> This is the analytics controller to log events on iOS </summary>
    public class IOSAnalyticsController : AnalyticsController
    {
        /// <summary> The tracker for the analytics </summary>
        private ITracker _Tracker;

        /// <summary> Initialises the analytics controller </summary>
        /// <param name="id"></param>
        public override void Initialise(String id)
        {
            Gai.SharedInstance.DispatchInterval = 5;
            Gai.SharedInstance.TrackUncaughtExceptions = true;

            _Tracker = Gai.SharedInstance.GetTracker(id);
        }

        /// <summary> Logs an event on Google Analytics </summary>
        /// <param name="category"></param>
        /// <param name="action"></param>
        /// <param name="label"></param>
        public override void LogEvent(String category, String action, String label)
        {
            Gai.SharedInstance.DefaultTracker.Send(DictionaryBuilder.CreateEvent(category, action, label, null).Build());
        }

        /// <summary> Sets the current screen on Google Analytics </summary>
        /// <param name="screenName"></param>
        public override void SetScreen(String screenName)
        {
            Gai.SharedInstance.DefaultTracker.Set(GaiConstants.ScreenName, screenName);
            Gai.SharedInstance.DefaultTracker.Send(DictionaryBuilder.CreateScreenView().Build());
        }
    }
}
