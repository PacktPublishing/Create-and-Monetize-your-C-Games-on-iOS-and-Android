using Engine.Shared.State;
using Game.Shared.Analytics;
using Game.Shared.Objects.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Shared.States
{
    /// <summary> This state is entered when the player has lost all lives </summary>
    public class GameOverState : State
    {
        /// <summary> Called when the state is entered - will show the game over screen then transitions </summary>
        public override void OnEnter()
        {
            AnalyticsManager.Instance.LogEvent("Game", "Game Over");
            FullscreenMessage.Instance.TransitionIn("game over!", () =>
            {
                FullscreenMessage.Instance.HideText();
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
            Dispose();
        }

        /// <summary> Disposes of the state </summary>
        public override void Dispose()
        {

        }
    }
}
