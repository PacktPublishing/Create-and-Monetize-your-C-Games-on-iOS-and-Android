using Engine.Shared.State;
using Game.Shared.Analytics;
using Game.Shared.Base;
using Game.Shared.Competitive;
using Game.Shared.Objects.UI;
using Game.Shared.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Shared.States
{
    /// <summary> This state is used once the game has been completed </summary>
    public class GameCompleteState : State
    {
        /// <summary> Called when the state is entered - starts the countdown </summary>
        public override void OnEnter()
        {
            AnalyticsManager.Instance.LogEvent("Game", "Game Complete");
            CompetitiveManager.Instance.UpdateLeaderboardProgress(Constants.LEADERBOARD_SCORE, (Int32)GameScene.Instance.TotalScore);
            FullscreenMessage.Instance.ChangeText($"game complete!\nfinal score: {GameScene.Instance.TotalScore}", () =>
            {
                StateManager.Instance.ChangeState(new MenuState());
            });
        }

        /// <summary> Updates the state </summary>
        /// <param name="timeSinceUpdate"></param>
        public override void Update(TimeSpan timeSinceUpdate)
        {

        }

        /// <summary> Called when the state exits - disposes of the state </summary>
        public override void OnExit()
        {
            Dispose();
        }

        /// <summary> Disposes of the state </summary>
        public override void Dispose()
        {

        }

    }
}
