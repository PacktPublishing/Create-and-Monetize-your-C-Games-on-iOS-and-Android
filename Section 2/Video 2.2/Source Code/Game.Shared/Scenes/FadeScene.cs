using Engine.Shared.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Shared.Scenes
{
    /// <summary> This scene fades in/out elements </summary>
    public abstract class FadeScene : Scene
    {
        /// <summary> The amount of time it takes to fade in/out </summary>
        protected readonly TimeSpan _FadeTime = TimeSpan.FromSeconds(0.33);
        /// <summary> The target alpha for the menu scene </summary>
        protected Single _TargetAlpha;
        /// <summary> The starting alpha of the scene </summary>
        protected Single _StartAlpha;
        /// <summary> The action to fire when the fade is complete </summary>
        protected Action _OnFade;
        /// <summary> Whether or not the scene is fading </summary>
        protected Boolean _Fading;
        /// <summary> The amount of time elapsed for fading in </summary>
        protected TimeSpan _ElapsedTime;

        /// <summary> Whether or not the scene is fading </summary>
        public Boolean Fading => _Fading;

        /// <summary> Sets the alpha of all elements in a scene </summary>
        /// <param name="alpha"></param>
        protected abstract void SetElementAlpha(Single alpha);
        /// <summary> Toggles the Enabled state of all buttons in the scene </summary>
        /// <param name="toEnable"></param>
        public abstract void ToggleButtonStates(Boolean toEnable);

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
            ToggleButtonStates(false);
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
    }
}
