using Engine.Shared.State;
using Engine.Shared.Touch;
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
            GameScene.Instance.Visible = false;
            FullscreenMessage.Instance.ForceActive("", null);
            _Menu = new MenuScene();
            _Menu.StartFade(0f, 1f, () => { _Menu.PlayButton.TouchEnabled = true; });
            _Menu.PlayButton.OnButtonRelease += OnPlay;
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
