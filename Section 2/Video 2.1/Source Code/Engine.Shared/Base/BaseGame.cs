using Engine.Shared.Audio;
using Engine.Shared.State;
using Engine.Shared.Touch;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Shared.Base
{
    /// <summary> This is the base that all games will extend from </summary>
    public abstract class BaseGame
    {
        /// <summary> The target resolution for the game </summary>
        public abstract Vector2 InitialResolution { get; }

        /// <summary> Creates the game with the chosen resolution </summary>
        /// <param name="targetResolution"></param>
        protected BaseGame()
        {
        }

        /// <summary> Initialises the game </summary>
        /// <param name="targetResolution"></param>
        public virtual void Init(Vector2 displayDimensions)
        {
            Single newHeight = (InitialResolution.X / displayDimensions.X) * displayDimensions.Y;
            Vector2 resolution = new Vector2(InitialResolution.X, newHeight);
            Renderer.Instance.Init(resolution, displayDimensions, CalculateExtraOffset(newHeight - InitialResolution.Y));
            StateManager.Instance.Init();
        }

        /// <summary> Pauses the game </summary>
        public virtual void Pause()
        {
            UpdateManager.Instance.Pause();
            AudioManager.Instance.Pause();
            TouchManager.Instance.CancelTouches();
        }

        /// <summary> Resumes the game </summary>
        public virtual void Resume()
        {
            UpdateManager.Instance.Resume();
            AudioManager.Instance.Resume();
        }

        /// <summary> Runs the game </summary>
        internal void Update()
        {
            UpdateManager.Instance.Update();
            Renderer.Instance.Update();
            AudioManager.Instance.Update();
            TouchManager.Instance.Update();
        }

        /// <summary> Loads the content for the game </summary>
        public abstract void LoadContent();

        /// <summary> Calculates the offset based on the height difference </summary>
        /// <param name="heightDifference"></param>
        /// <returns></returns>
        protected abstract Vector2 CalculateExtraOffset(Single heightDifference);

        /// <summary> Draws our objects </summary>
        internal void Draw()
        {
            Renderer.Instance.Draw();
        }
    }
}
