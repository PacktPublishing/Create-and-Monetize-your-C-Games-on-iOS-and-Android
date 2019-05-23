using Engine.Shared.Base;
using Engine.Shared.Interfaces;
using System;

namespace Engine.Shared.State
{
    /// <summary> The state manager that controls all the states </summary>
    public class StateManager : IUpdatable
    {
        /// <summary> The current state </summary>
        private State _CurrentState;
        /// <summary> The next state that will be entered </summary>
        private State _NextState;
        /// <summary> The instance of the StateManager </summary>
        private static StateManager _Instance;

        /// <summary> The instance of the StateManager </summary>
        public static StateManager Instance => _Instance ?? (_Instance = new StateManager());

        private StateManager()
        {

        }

        internal void Init()
        {
            UpdateManager.Instance.AddUpdatable(this);
        }

        /// <summary> Starts the given state </summary>
        public void StartState(State newState)
        {
            _CurrentState?.OnExit();
            _CurrentState?.Dispose();

            _CurrentState = newState;
            _CurrentState.OnEnter();
        }

        /// <summary> Updates the current state </summary>
        /// <param name="timeSinceUpdate"></param>
        public void Update(TimeSpan timeSinceUpdate)
        {
            if (_CurrentState == null) return;
            _CurrentState.Update(timeSinceUpdate);

            if (_NextState != null)
            {
                StartState(_NextState);
                _NextState = null;
            }
        }

        /// <summary> Changes the state </summary>
        /// <param name="state"></param>
        public void ChangeState(State state)
        {
            _NextState = state;
        }

        /// <summary> Whether or not the state service can be updated </summary>
        /// <returns></returns>
        public Boolean CanUpdate()
        {
            return _CurrentState != null;
        }
    }
}
