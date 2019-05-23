using Engine.Shared.State;
using Engine.Shared.Touch;
using Game.Shared.Analytics;
using Game.Shared.Competitive;
using Game.Shared.Objects.UI;
using Game.Shared.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Shared.States
{
    /// <summary> The state for the main menu </summary>
    public class MenuState : State
    {
        /// <summary> The scene for the menu </summary>
        private MenuScene _Menu;

        /// <summary> Calls when the state is entered </summary>
        public override void OnEnter()
        {
            AnalyticsManager.Instance.ChangeScreen("Menu");
            GameScene.Instance.Visible = false;
            FullscreenMessage.Instance.ForceActive("", null);
            _Menu = new MenuScene();
            _Menu.StartFade(0f, 1f, () => { _Menu.PlayButton.TouchEnabled = true; });
            _Menu.PlayButton.OnButtonRelease += OnPlay;
            _Menu.StoreButton.OnButtonRelease += OnStore;
            _Menu.AchievementButton.OnButtonRelease += OnAchievement;
            _Menu.LeaderboardButton.OnButtonRelease += OnLeaderboard;
        }

        /// <summary> Called when the play button is pressed - starts a game </summary>
        /// <param name="button"></param>
        private void OnPlay(Button button)
        {
            _Menu.StartFade(1, 0, () =>
            {
                GameScene.Instance.InitialiseGame();
                StateManager.Instance.ChangeState(new StartState());
            });
        }

        /// <summary> Called when the play button is pressed - starts a game </summary>
        /// <param name="button"></param>
        private void OnStore(Button button)
        {
            _Menu.StartFade(1, 0, () =>
            {
                StateManager.Instance.ChangeState(new StoreState());
            });
        }

        /// <summary> Called when the achievement button has been pressed -  opens the achievement screen </summary>
        /// <param name="button"></param>
        private void OnAchievement(Button button)
        {
            CompetitiveManager.Instance.ViewAchievements();
        }

        /// <summary> Called when the leaderboard button has been pressed - opens the leaderboard screen </summary>
        /// <param name="button"></param>
        private void OnLeaderboard(Button button)
        {
            CompetitiveManager.Instance.ViewLeaderboards();
        }

        /// <summary> Updates the state </summary>
        /// <param name="timeSinceUpdate"></param>
        public override void Update(TimeSpan timeSinceUpdate)
        {

        }

        /// <summary> Calls when the state is exiting </summary>
        public override void OnExit()
        {

        }

        /// <summary> Disposes of the state </summary>
        public override void Dispose()
        {
            _Menu?.Dispose();
            _Menu = null;
        }
    }
}
