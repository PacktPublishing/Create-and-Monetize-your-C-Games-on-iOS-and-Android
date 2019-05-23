using Engine.Shared.State;
using Game.Shared.Base;
using Game.Shared.Level;
using Game.Shared.Objects.UI;
using Game.Shared.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Shared.States
{
    /// <summary> This state is used to initialise the game </summary>
    public class StartState : State
    {
        /// <summary> Called when the state is entered - it creates the scene from the level data </summary>
        public override void OnEnter()
        {
            GameScene.Instance.LoadLevel();
            GameScene.Instance.Visible = true;
            FullscreenMessage.Instance.ForceActive($"level {LevelController.Instance.CurrentLevelIndex}", () =>
            {
                PlayerControls.Instance.Visible = true;
                GameScene.Instance.Start();
                FullscreenMessage.Instance.TransitionOut(() => { StateManager.Instance.ChangeState(new PlayingState()); });
            });
        }
        /// <summary> Updates the state </summary>
        /// <param name="timeSinceUpdate"></param>
        public override void Update(TimeSpan timeSinceUpdate)
        {
        }
        /// <summary> Called when the state exits - it starts the game </summary>
        public override void OnExit()
        {
            Dispose();
        }

        public override void Dispose()
        {

        }


    }
}
