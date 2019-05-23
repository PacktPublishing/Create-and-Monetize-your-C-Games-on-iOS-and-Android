#if __ANDROID__

#elif __IOS__
using Game.iOS.Source.Analytics;
#endif
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Shared.Analytics
{
    /// <summary> This manager will be used to log events, screens & purchases on Google Analytics </summary>
    public class AnalyticsManager
    {
        /// <summary> The current screen being displayed </summary>
        private String _CurrentScreen = "";
        /// <summary> The platform specific controller used to log in either Android or iOS </summary>
        private AnalyticsController _PlatformController;
        /// <summary> The instance of the manager </summary>
        private static AnalyticsManager _Instance;

        /// <summary> The instance of the manager </summary>
        public static AnalyticsManager Instance => _Instance ?? (_Instance = new AnalyticsManager());

        /// <summary> Creates the analytics manager </summary>
        private AnalyticsManager()
        {
#if __ANDROID__

#elif __IOS__
            _PlatformController = new IOSAnalyticsController();
#endif
        }

        /// <summary> Initialises the analytics manager & the base platform </summary>
        /// <param name="id"></param>
        public void Initialise(String id)
        {
            _PlatformController.Initialise(id);
        }

        /// <summary> Changes the screen on the manager </summary>
        /// <param name="screenName"></param>
        public void ChangeScreen(String screenName)
        {
            if (_CurrentScreen.Equals(screenName)) return;
            _CurrentScreen = screenName;
            _PlatformController.SetScreen(screenName);
        }

        /// <summary> Logs the given event on Google Analytics </summary>
        /// <param name="category"></param>
        /// <param name="eventName"></param>
        public void LogEvent(String category, String action, String label = "")
        {
            _PlatformController.LogEvent(category, action, label);
        }
    }
}
