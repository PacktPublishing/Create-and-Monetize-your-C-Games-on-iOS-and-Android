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
    public class MenuScene : Scene
    {
        /// <summary> The amount of time it takes to fade in/out </summary>
        private readonly TimeSpan _FadeTime = TimeSpan.FromSeconds(0.33);
        /// <summary> The title of the game </summary>
        private readonly TextDisplay _Title;
        /// <summary> The target alpha for the menu scene </summary>
        private Single _TargetAlpha;
        /// <summary> The starting alpha of the scene </summary>
        private Single _StartAlpha;
        /// <summary> The action to fire when the fade is complete </summary>
        private Action _OnFade;
        /// <summary> Whether or not the scene is fading </summary>
        private Boolean _Fading;
        /// <summary> The amount of time elapsed for fading in </summary>
        private TimeSpan _ElapsedTime;

        /// <summary> The button used to play the game </summary>
        public Button PlayButton { get; }

        /// <summary> The scene for the main menu </summary>
        public MenuScene()
        {
            PlayButton = new Button(ZOrders.SCORE_TEXT, new Sprite(ZippyGame.UICanvas, ZOrders.SCORE_TEXT, Texture.GetTexture("Content/Graphics/UI/PlayButton.png")) { Colour = new OpenTK.Vector4(1, 1, 1, 0) })
            {
                Position = new OpenTK.Vector2(-128, -250),
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

        /// <summary> Sets the alpha on all elements in the scene </summary>
        /// <param name="alpha"></param>
        private void SetElementAlpha(Single alpha)
        {
            PlayButton.Sprite.Colour = new OpenTK.Vector4(1, 1, 1, alpha);
            _Title.Colour = new OpenTK.Vector4(1, 1, 1, alpha);
        }

        /// <summary> Starts fading in/out the scene </summary>
        /// <param name="startAlpha"></param>
        /// <param name="targetAlpha"></param>
        /// <param name="onComplete"></param>
        public void StartFade(Single startAlpha, Single targetAlpha, Action onComplete)
        {
            _ElapsedTime = TimeSpan.Zero;
            _StartAlpha = startAlpha;
            _TargetAlpha = targetAlpha;
            _OnFade = onComplete;
            _Fading = true;
            PlayButton.TouchEnabled = false;
        }

        /// <summary> Updates the scene's alpha if fading in/out </summary>
        /// <param name="timeSinceUpdate"></param>
        public override void Update(TimeSpan timeSinceUpdate)
        {
            if (!_Fading) return;

            _ElapsedTime += timeSinceUpdate;
            if (_ElapsedTime >= _FadeTime)
            {
                _Fading = false;
                SetElementAlpha(_TargetAlpha);
                _OnFade?.Invoke();
            }
            else
            {
                Single alphaRange = _TargetAlpha - _StartAlpha;
                Single currentAlpha = _StartAlpha + (alphaRange * (Single)(_ElapsedTime.TotalSeconds / _FadeTime.TotalSeconds));
                SetElementAlpha(currentAlpha);
            }
        }

        /// <summary> Disposes of the menu </summary>
        public override void Dispose()
        {
            base.Dispose();
            PlayButton?.Dispose();
            _Title.Dispose();
        }
    }
}
