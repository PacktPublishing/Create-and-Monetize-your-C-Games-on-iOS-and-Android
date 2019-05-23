using Engine.Shared.State;
using Game.Shared.Analytics;
using Game.Shared.Base;
using Game.Shared.Competitive;
using Game.Shared.Objects.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Shared.States
{
    /// <summary> The state which connects to the Google Play Services & analytics </summary>
    public class LoadingState : State
    {
        /// <summary> Called when the state enters - it starts connecting to both services </summary>
        public override void OnEnter()
        {
            FullscreenMessage.Instance.ForceActive("loading", null);
            AnalyticsManager.Instance.Initialise(Constants.ANALYTICS_ID);
            String[] achievementIds = new String[0];
            String[] leaderboardIds = new String[0];
#if __ANDROID__
            achievementIds = Constants.ANDROID_ACHIEVEMENT_IDS;
            leaderboardIds = Constants.ANDROID_LEADERBOARD_IDS;
#elif __IOS__
            achievementIds = Constants.IOS_ACHIEVEMENT_IDS;
            leaderboardIds = Constants.IOS_LEADERBOARD_IDS;
#endif
            CompetitiveManager.Instance.LoadData(achievementIds, leaderboardIds);
            CompetitiveManager.Instance.Connect();
            FullscreenMessage.Instance.ChangeText("", () =>
            {
                StateManager.Instance.ChangeState(new MenuState());
            });
        }

        /// <summary> Updates the state </summary>
        /// <param name="timeSinceUpdate"></param>
        public override void Update(TimeSpan timeSinceUpdate)
        {

        }

        /// <summary> Called when the state exits </summary>
        public override void OnExit()
        {

        }

        /// <summary> Disposes of the state </summary>
        public override void Dispose()
        {

        }
    }
}
