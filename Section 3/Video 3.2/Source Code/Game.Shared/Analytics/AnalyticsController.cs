using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Shared.Analytics
{
    /// <summary> This class will be used to call the platform-specific methods in both Android and iOS </summary>
    public abstract class AnalyticsController
    {
        /// <summary> Initialises the controller </summary>
        /// <param name="id"></param>
        public abstract void Initialise(String id);
        /// <summary> Sets the current screen </summary>
        /// <param name="screenName"></param>
        public abstract void SetScreen(String screenName);
        /// <summary> Logs a specific event in the app </summary>
        /// <param name="category"></param>
        /// <param name="eventName"></param>
        public abstract void LogEvent(String category, String action, String label);
    }
}
