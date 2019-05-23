using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Game.Shared.Analytics;
using Android.Gms.Analytics;
using Engine.Android;

namespace Game.Android.Source.Analytics
{
    /// <summary> The android specific Google Analytics controller </summary>
    public class AndroidAnalyticsController : AnalyticsController
    {
        /// <summary> The instance of Google Analytics </summary>
        private GoogleAnalytics _AnalyticsInstance;
        /// <summary> The Google Analytics tracker </summary>
        private Tracker _AnalyticsTracker;

        /// <summary> Initialises the controller </summary>
        /// <param name="id"></param>
        public override void Initialise(String id)
        {
            _AnalyticsInstance = GoogleAnalytics.GetInstance(GameActivity.Instance);
            _AnalyticsInstance.SetLocalDispatchPeriod(5);

            _AnalyticsTracker = _AnalyticsInstance.NewTracker(id);
            _AnalyticsTracker.EnableExceptionReporting(true);
        }

        /// <summary> Logs an event in Google Analytics </summary>
        /// <param name="category"></param>
        /// <param name="eventName"></param>
        public override void LogEvent(String category, String action, String label)
        {
            HitBuilders.EventBuilder builder = new HitBuilders.EventBuilder();
            builder.SetCategory(category);
            builder.SetAction(action);
            if (!String.IsNullOrEmpty(label)) builder.SetLabel(label);

            _AnalyticsTracker.Send(builder.Build());
        }

        /// <summary> Sets the current screen on Google Analytics </summary>
        /// <param name="screenName"></param>
        public override void SetScreen(String screenName)
        {
            _AnalyticsTracker.SetScreenName(screenName);
            _AnalyticsTracker.Send(new HitBuilders.ScreenViewBuilder().Build());
        }
    }
}