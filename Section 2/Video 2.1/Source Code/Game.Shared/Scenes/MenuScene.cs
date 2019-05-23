using Engine.Shared.Graphics;
using Engine.Shared.Graphics.Drawables;
using Engine.Shared.Touch;
using Game.Shared.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Shared.Scenes
{
    /// <summary> The scene for the main menu </summary>
    public class MenuScene : FadeScene
    {
        /// <summary> The title of the game </summary>
        private readonly TextDisplay _Title;
        
        /// <summary> The button used to play the game </summary>
        public Button PlayButton { get; }
        /// <summary> The button used to enter the store </summary>
        public Button StoreButton { get; }

        /// <summary> The scene for the main menu </summary>
        public MenuScene()
        {
            PlayButton = new Button(ZOrders.SCORE_TEXT, new Sprite(ZippyGame.UICanvas, ZOrders.SCORE_TEXT, Texture.GetTexture("Content/Graphics/UI/PlayButton.png")) { Colour = new OpenTK.Vector4(1, 1, 1, 0) })
            {
                Position = new OpenTK.Vector2(-128, -150),
                Visible = true,
                TouchEnabled = false
            };

            StoreButton = new Button(ZOrders.SCORE_TEXT, new Sprite(ZippyGame.UICanvas, ZOrders.SCORE_TEXT, Texture.GetTexture("Content/Graphics/Buttons/StoreButton.png")) { Colour = new OpenTK.Vector4(1, 1, 1, 0) })
            {
                Position = new OpenTK.Vector2(-128, -470),
                Visible = true,
                TouchEnabled = false
            };

            _Title = new TextDisplay(ZippyGame.UICanvas, ZOrders.SCORE_TEXT, Texture.GetTexture("Content/Graphics/UI/kromasky.png"), Constants.KROMASKY_CHARACTERS, 32, 32)
            {
                Text = "zippy's adventure",
                Position = new OpenTK.Vector2(0, 250),
                Colour = new OpenTK.Vector4(1, 1, 1, 0),
                Visible = true
            };
            _Title.Offset = new OpenTK.Vector2(_Title.Width / 2, _Title.Height / 2);
        }

        /// <summary> Toggles the state of the buttons </summary>
        /// <param name="toEnable"></param>
        public override void ToggleButtonStates(Boolean toEnable)
        {
            PlayButton.TouchEnabled = toEnable;
            StoreButton.TouchEnabled = toEnable;
        }

        /// <summary> Sets the alpha on all elements in the scene </summary>
        /// <param name="alpha"></param>
        protected override void SetElementAlpha(Single alpha)
        {
            PlayButton.Sprite.Colour = new OpenTK.Vector4(1, 1, 1, alpha);
            StoreButton.Sprite.Colour = new OpenTK.Vector4(1, 1, 1, alpha);
            _Title.Colour = new OpenTK.Vector4(1, 1, 1, alpha);
        }

        /// <summary> Disposes of the menu </summary>
        public override void Dispose()
        {
            base.Dispose();
            PlayButton?.Dispose();
            StoreButton?.Dispose();
            _Title.Dispose();
        }
    }
}
