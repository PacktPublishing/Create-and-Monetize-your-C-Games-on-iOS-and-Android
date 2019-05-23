using Engine.Shared.Base;
using Engine.Shared.Graphics.Drawables;
using Engine.Shared.Interfaces;
using System;
using System.Collections.Generic;

namespace Engine.Shared.Graphics
{
    /// <summary> The main scene </summary>
    public abstract class Scene : IUpdatable, IDisposable
    {
        /// <summary> The drawables in the scene </summary>
        protected readonly List<Drawable> _Drawables = new List<Drawable>();
        /// <summary> Whether or not the scene is visible </summary>
        protected Boolean _Visible;

        /// <summary> Whether or not the scene is visible </summary>
        public Boolean Visible
        {
            get { return _Visible; }
            set
            {
                _Visible = value;
                foreach (Drawable drawable in _Drawables) drawable.ParentVisible = value;
            }
        }

        /// <summary> Creates the scene </summary>
        protected Scene()
        {
            UpdateManager.Instance.AddUpdatable(this);
        }

        /// <summary> Adds the drawable to the scene </summary>
        /// <param name="drawable"></param>
        public void AddDrawable(Drawable drawable)
        {
            _Drawables.Add(drawable);
            drawable.ParentVisible = Visible;
        }

        /// <summary> Removes the drawable from the scene </summary>
        /// <param name="drawable"></param>
        public void RemoveDrawable(Drawable drawable)
        {
            _Drawables.Remove(drawable);
            drawable.ParentVisible = true;
        }

        /// <summary> Whether or not the scene can be updated </summary>
        /// <returns></returns>
        public virtual Boolean CanUpdate()
        {
            return true;
        }

        /// <summary> Disposes of the scene </summary>
        public virtual void Dispose()
        {
            foreach (Drawable drawable in _Drawables) drawable.Dispose();
            _Drawables.Clear();
            UpdateManager.Instance.RemoveUpdatable(this);
        }

        /// <summary> Updates the scene </summary>
        /// <param name="timeSinceUpdate"></param>
        public abstract void Update(TimeSpan timeSinceUpdate);
    }
}
