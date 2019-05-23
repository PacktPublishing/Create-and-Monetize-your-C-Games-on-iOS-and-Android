using Engine.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Shared.Base
{
    /// <summary> The manager that updates all the updatables </summary>
    public class UpdateManager
    {
        /// <summary> The objects that can be updated </summary>
        private static List<IUpdatable> _Updatables = new List<IUpdatable>();
        /// <summary> The time that the last update was called </summary>
        private DateTime _TimeLastUpdated;
        /// <summary> Whether or not the update manager is paused </summary>
        private Boolean _Paused;
        /// <summary> The instance of the update manager </summary>
        private static UpdateManager _Instance;

        /// <summary> The instance of the update manager </summary>
        public static UpdateManager Instance => _Instance ?? (_Instance = new UpdateManager());

        /// <summary> Creates the update manager </summary>
        private UpdateManager()
        {
            _TimeLastUpdated = DateTime.Now;
        }

        /// <summary> Adds the updatable object to the list of updatables so it can be updated </summary>
        /// <param name="updatable"></param>
        public void AddUpdatable(IUpdatable updatable)
        {
            _Updatables.Add(updatable);
        }

        /// <summary> Pauses the update manager </summary>
        public void Pause()
        {
            _Paused = true;
        }

        /// <summary> Resumes the update manager </summary>
        public void Resume()
        {
            _Paused = false;
            _TimeLastUpdated = DateTime.Now;
        }

        /// <summary> Updates all objects </summary>
        public void Update()
        {
            if (_Paused) return;
            DateTime current = DateTime.Now;
            TimeSpan timeSinceUpdate = current - _TimeLastUpdated;

            foreach (IUpdatable obj in _Updatables.Where(s => s.CanUpdate()).ToList())
            {
                obj.Update(timeSinceUpdate);
            }
            _TimeLastUpdated = DateTime.Now;
        }

        /// <summary> Removes the updatable object so that it can stop being updated </summary>
        /// <param name="updatable"></param>
        public void RemoveUpdatable(IUpdatable updatable)
        {
            _Updatables.Remove(updatable);
        }
    }
}
