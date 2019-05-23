using Engine.Shared.Base;
using Engine.Shared.Graphics;
using Engine.Shared.Graphics.Drawables;
using Engine.Shared.Interfaces;
using Game.Shared.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Shared.Objects.UI
{
    /// <summary> An object that displays a message on the whole screen </summary>
    public class FullscreenMessage : IUpdatable
    {
        /// <summary> The current state of the message </summary>
        private enum State
        {
            IDLE,
            TRANSITION_IN,
            HOLDING,
            FADING_OUT_TEXT,
            FADING_IN_TEXT,
            TRANSITION_OUT
        }

        /// <summary> The current state of the message </summary>
        private State _CurrentState;
        /// <summary> The background of the message </summary>
        private Sprite _Background;
        /// <summary> The text for the message </summary>
        private TextDisplay _Text;
        /// <summary> The next text to display on the message </summary>
        private String _NextText;
        /// <summary> The amount of time until the state progresses </summary>
        private TimeSpan _TimeTilProgress;
        /// <summary> The action to fire when the message has completed its current action </summary>
        private Action _OnComplete;
        /// <summary> The instance of the message </summary>
        private static FullscreenMessage _Instance;

        /// <summary> The instance of the message </summary>
        public static FullscreenMessage Instance => _Instance ?? (_Instance = new FullscreenMessage());

        /// <summary> Creates the fullscreen message </summary>
        private FullscreenMessage()
        {
            _Background = new Sprite(ZippyGame.UICanvas, ZOrders.TRANSITION_BG, Texture.GetPixel())
            {
                Width = Renderer.Instance.TargetDimensions.X,
                Height = Renderer.Instance.TargetDimensions.Y,
                Visible = true,
                Colour = new OpenTK.Vector4(0, 0, 0, 0),
                Offset = new OpenTK.Vector2(Renderer.Instance.TargetDimensions.X / 2, Renderer.Instance.TargetDimensions.Y / 2)
            };
            _Text = new TextDisplay(ZippyGame.UICanvas, ZOrders.TRANSITION_TEXT, Texture.GetTexture("Content/Graphics/UI/kromasky.png"), Constants.KROMASKY_CHARACTERS, 32, 32)
            {
                Visible = true,
                Colour = new OpenTK.Vector4(0, 0, 0, 0),
                TextAlignment = TextDisplay.Alignment.CENTER
            };
            _CurrentState = State.IDLE;
            UpdateManager.Instance.AddUpdatable(this);
        }

        /// <summary> Sets the text on the message </summary>
        /// <param name="text"></param>
        private void SetText(String text)
        {
            _Text.Text = text;
            _Text.Offset = new OpenTK.Vector2(_Text.Width / 2, _Text.Height / 2);
        }

        /// <summary> Forces the message to be active </summary>
        /// <param name="text"></param>
        public void ForceActive(String text, Action onComplete)
        {
            if (_Text.Text.Equals(text)) _TimeTilProgress = TimeSpan.FromSeconds(0);
            else _TimeTilProgress = TimeSpan.FromSeconds(3);

            _Background.Colour = new OpenTK.Vector4(0, 0, 0, 1);
            SetText(text);
            _Text.Colour = new OpenTK.Vector4(1, 1, 1, 1);
            _OnComplete = onComplete;
            _CurrentState = State.HOLDING;
        }

        /// <summary> Hides the text on the message </summary>
        public void HideText()
        {
            _Text.Colour = new OpenTK.Vector4(1, 1, 1, 0);
        }

        /// <summary> Transitions in the message </summary>
        public void TransitionIn(String text, Action onComplete)
        {
            SetText(text);
            _OnComplete = onComplete;
            _CurrentState = State.TRANSITION_IN;
            _TimeTilProgress = TimeSpan.FromSeconds(0.5);
        }

        /// <summary> Transitions out the message </summary>
        /// <param name="onComplete"></param>
        public void TransitionOut(Action onComplete)
        {
            _OnComplete = onComplete;
            _CurrentState = State.TRANSITION_OUT;
            _TimeTilProgress = TimeSpan.FromSeconds(0.5);
        }

        /// <summary> This is used to change the text on the message </summary>
        /// <param name="text"></param>
        /// <param name="onComplete"></param>
        public void ChangeText(String text, Action onComplete)
        {
            _NextText = text;
            _OnComplete = onComplete;
            _CurrentState = State.FADING_OUT_TEXT;
            _TimeTilProgress = TimeSpan.FromSeconds(0.25);
        }

        /// <summary> Whether or not the message can update </summary>
        /// <returns></returns>
        public bool CanUpdate()
        {
            return true;
        }

        /// <summary> Updates the message based on the current state </summary>
        /// <param name="timeSinceUpdate"></param>
        public void Update(TimeSpan timeSinceUpdate)
        {
            _TimeTilProgress -= timeSinceUpdate;
            switch (_CurrentState)
            {
                case State.TRANSITION_IN:
                    {
                        Single alpha = Math.Min(1f - ((Single)_TimeTilProgress.TotalSeconds / 0.5f), 1f);
                        _Background.Colour = new OpenTK.Vector4(0, 0, 0, alpha);
                        _Text.Colour = new OpenTK.Vector4(1, 1, 1, alpha);
                        if (_TimeTilProgress <= TimeSpan.Zero)
                        {
                            _CurrentState = State.HOLDING;
                            _TimeTilProgress = TimeSpan.FromSeconds(2);
                        }
                    }
                    break;

                case State.HOLDING:
                    if (_TimeTilProgress <= TimeSpan.Zero)
                    {
                        _CurrentState = State.IDLE;
                        _OnComplete?.Invoke();
                    }
                    break;

                case State.FADING_OUT_TEXT:
                    {
                        Single alpha = Math.Max(0, (Single)_TimeTilProgress.TotalSeconds / 0.25f);
                        _Text.Colour = new OpenTK.Vector4(1, 1, 1, alpha);
                        if (_TimeTilProgress <= TimeSpan.Zero)
                        {
                            _CurrentState = State.FADING_IN_TEXT;
                            SetText(_NextText);
                            _TimeTilProgress = TimeSpan.FromSeconds(0.25);
                        }
                    }
                    break;

                case State.FADING_IN_TEXT:
                    {
                        Single alpha = Math.Min(1, 1f - ((Single)_TimeTilProgress.TotalSeconds / 0.25f));
                        _Text.Colour = new OpenTK.Vector4(1, 1, 1, alpha);
                        if (_TimeTilProgress <= TimeSpan.Zero)
                        {
                            _CurrentState = State.HOLDING;
                            _TimeTilProgress = TimeSpan.FromSeconds(2);
                        }
                    }
                    break;

                case State.TRANSITION_OUT:
                    {
                        Single alpha = Math.Max(0, (Single)_TimeTilProgress.TotalSeconds / 0.5f);
                        _Text.Colour = new OpenTK.Vector4(1, 1, 1, alpha);
                        _Background.Colour = new OpenTK.Vector4(0, 0, 0, alpha);
                        if (_TimeTilProgress <= TimeSpan.Zero)
                        {
                            _CurrentState = State.IDLE;
                            _OnComplete?.Invoke();
                        }
                    }
                    break;
            }

        }
    }
}
