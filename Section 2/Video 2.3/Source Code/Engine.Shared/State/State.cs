using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Shared.State
{
    public abstract class State : IDisposable
    {
        /// <summary> Function that is called when the state is entered </summary>
        public abstract void OnEnter();

        /// <summary> Function that is called when the state is exited </summary>
        public abstract void OnExit();

        /// <summary> Updates the state </summary>
        /// <param name="timeTilUpdate"></param>
        public abstract void Update(TimeSpan timeSinceUpdate);

        /// <summary> Disposes of the state </summary>
        public abstract void Dispose();
    }
}
