using System;
using System.Collections.Generic;
using System.Text;
using Engine.Shared.State;
using Game.Shared.Scenes;
using Game.Shared.Base;

namespace Game.Shared.States
{
    /// <summary> The playing state </summary>
    public class PlayingState : State
    {
        /// <summary> Called when the state is entered - enables the scene </summary>
        public override void OnEnter()
        {
            GameScene.Instance.Visible = true;
            PlayerControls.Instance.Enabled = true;
            GameScene.Instance.LevelComplete = OnLevelComplete;
            GameScene.Instance.OnDeath = OnDeath;
        }

        public override void OnExit()
        {
            Dispose();
            PlayerControls.Instance.Enabled = false;
            GameScene.Instance.LevelComplete = null;
            GameScene.Instance.OnDeath = null;
            GameScene.Instance.Stop();
        }

        public override void Update(TimeSpan timeSinceUpdate)
        {

        }

        private void OnLevelComplete()
        {
            StateManager.Instance.ChangeState(new LevelCompleteState());
        }

        /// <summary> Called when the player dies - changes the state to the start state if there are lives remaining </summary>
        private void OnDeath()
        {
            if (GameScene.Instance.Zippy.Lives <= 0) StateManager.Instance.ChangeState(new GameOverState());
            else StateManager.Instance.ChangeState(new StartState());
            GameScene.Instance.UpdateLivesText();
        }

        public override void Dispose()
        {

        }
    }
}
