using Engine.Shared.Graphics;
using Engine.Shared.Graphics.Drawables;
using Engine.Shared.Touch;
using Game.Shared.Scenes;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Shared.Base
{
    /// <summary> This class controls the player </summary>
    public class PlayerControls
    {
        /// <summary> The left arrow button </summary>
        private Button _LeftArrow;
        /// <summary> The right arrow button </summary>
        private Button _RightArrow;
        /// <summary> The button to make Zippy jump </summary>
        private Button _JumpButton;
        /// <summary> Whether or not the buttons are enabled </summary>
        private Boolean _Enabled;
        /// <summary> Whether or not the buttons are visible </summary>
        private Boolean _Visible;
        /// <summary> Whether or not the player is moving left </summary>
        private Boolean _MovingLeft;
        /// <summary> Whether or not the player is moving right </summary>
        private Boolean _MovingRight;
        /// <summary> The instance of the controls </summary>
        private static PlayerControls _Instance;

        /// <summary> Whether or not the controls are enabled </summary>
        public Boolean Enabled
        {
            get { return _Enabled; }
            set
            {
                _Enabled = value;
                _LeftArrow.TouchEnabled = value;
                _RightArrow.TouchEnabled = value;
                _JumpButton.TouchEnabled = value;
                if (!value)
                {
                    _MovingLeft = false;
                    _MovingRight = false;
                }
            }
        }
        /// <summary> Whether or not the controls are enabled </summary>
        public Boolean Visible
        {
            get { return _Visible; }
            set
            {
                _Visible = value;
                _LeftArrow.Visible = value;
                _RightArrow.Visible = value;
                _JumpButton.Visible = value;
            }
        }
        /// <summary> The instance of the controls </summary>
        public static PlayerControls Instance => _Instance ?? (_Instance = new PlayerControls());

        /// <summary> Creates the controls </summary>
        private PlayerControls()
        {
            _LeftArrow = new Button(ZOrders.CONTROLS, new Sprite(ZippyGame.UICanvas, ZOrders.CONTROLS, Texture.GetTexture("Content/Graphics/Buttons/LeftArrow.png")))
            {
                Visible = true,
                Position = new Vector2(-850, -490)
            };
            _LeftArrow.OnButtonPress = OnLeftPress;
            _LeftArrow.OnButtonRelease = OnLeftRelease;
            _RightArrow = new Button(ZOrders.CONTROLS, new Sprite(ZippyGame.UICanvas, ZOrders.CONTROLS, Texture.GetTexture("Content/Graphics/Buttons/RightArrow.png")))
            {
                Visible = true,
                Position = new Vector2(-550, -490)
            };
            _RightArrow.OnButtonPress = OnRightPress;
            _RightArrow.OnButtonRelease = OnRightRelease;
            _JumpButton = new Button(ZOrders.CONTROLS, new Sprite(ZippyGame.UICanvas, ZOrders.CONTROLS, Texture.GetTexture("Content/Graphics/Buttons/JumpButton.png")))
            {
                Visible = true,
                Position = new Vector2(650, -490)
            };
            _JumpButton.OnButtonPress = OnJumpPress;
        }

        /// <summary> Called when the left button has been pressed - it will move the player left </summary>
        private void OnLeftPress(Button button)
        {
            if (_MovingRight || _MovingLeft) return;
            GameScene.Instance.Zippy.MoveLeft();
            _MovingLeft = true;
        }

        /// <summary> Called when the button has been released </summary>
        /// <param name="button"></param>
        private void OnLeftRelease(Button button)
        {
            if (_MovingRight) return;
            GameScene.Instance.Zippy.Stop();
            _MovingLeft = false;
        }

        /// <summary> Called when the right button has been pressed - it will move the player right </summary>
        /// <param name="button"></param>
        private void OnRightPress(Button button)
        {
            if (_MovingLeft || _MovingRight) return;
            GameScene.Instance.Zippy.MoveRight();
            _MovingRight = true;
        }

        /// <summary> Called when the right button has been released </summary>
        /// <param name="button"></param>
        private void OnRightRelease(Button button)
        {
            if (_MovingLeft) return;
            GameScene.Instance.Zippy.Stop();
            _MovingRight = false;
        }

        /// <summary> Called when the jump button has been pressed </summary>
        /// <param name="button"></param>
        private void OnJumpPress(Button button)
        {
            GameScene.Instance.Zippy.Jump();
        }
    }
}
