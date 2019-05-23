using Engine.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Shared.Touch
{
    public class TouchManager
    {
        /// <summary> The events that are to be processed </summary>
        private List<TouchEvent> _EventsToProcess = new List<TouchEvent>();
        /// <summary> The TouchListeners that are listening to input </summary>
        private List<ITouchListener> _TouchListeners = new List<ITouchListener>();
        /// <summary> Whether or not the touch listseners have been added/removed </summary>
        private Boolean _TouchListenersChanged;

        /// <summary> The instance of the TouchManager </summary>
        private static TouchManager _Instance;
        /// <summary> The instance of the TouchManager </summary>
        public static TouchManager Instance => _Instance ?? (_Instance = new TouchManager());

        /// <summary> Creates the touch manager </summary>
        private TouchManager()
        {

        }

        /// <summary> Adds the touch event to the TouchManagerv</summary>
        /// <param name="touchEvent"></param>
        internal void AddEvent(TouchEvent touchEvent)
        {
            _EventsToProcess.Add(touchEvent);
        }

        /// <summary> Adds the listener to the TouchManager </summary>
        /// <param name="listener"></param>
        internal void AddTouchListener(ITouchListener listener)
        {
            _TouchListeners.Add(listener);
            _TouchListenersChanged = true;
        }

        /// <summary> Removes the touch listener from the list </summary>
        /// <param name="listener"></param>
        internal void RemoveTouchListener(ITouchListener listener)
        {
            _TouchListeners.Remove(listener);
        }

        /// <summary> Updates the touch manager </summary>
        public void Update()
        {
            if (_TouchListenersChanged || _TouchListeners.Any(s => s.TouchOrderChanged))
            {
                _TouchListeners = _TouchListeners.OrderBy(s => s.TouchOrder).ToList();
                foreach (ITouchListener listener in _TouchListeners) listener.TouchOrderChanged = false;
                _TouchListenersChanged = false;
            }

            foreach (TouchEvent touchEvent in _EventsToProcess.ToList())
            {
                switch (touchEvent.TouchType)
                {
                    case TouchEvent.Type.PRESS:
                        foreach (ITouchListener listener in _TouchListeners.Where(s => s.TouchEnabled))
                        {
                            if (listener.IsTouched(touchEvent.Position))
                            {
                                Boolean shouldConsume = listener.OnPress(touchEvent.Id, touchEvent.Position);
                                if (shouldConsume) break;
                            }
                        }
                        break;

                    case TouchEvent.Type.MOVE:
                        foreach (ITouchListener listener in _TouchListeners.Where(s => s.TouchEnabled && s.ListeningForMove))
                        {
                            Boolean shouldConsume = listener.OnMove(touchEvent.Id, touchEvent.Position);
                            if (shouldConsume) break;
                        }
                        break;

                    case TouchEvent.Type.RELEASE:
                        foreach (ITouchListener listener in _TouchListeners.Where(s => s.TouchEnabled))
                        {
                            listener.OnRelease(touchEvent.Id, touchEvent.Position);
                        }
                        break;
                }

                _EventsToProcess.Remove(touchEvent);
            }
        }

        /// <summary> Cancels all touch events </summary>
        public void CancelTouches()
        {
            foreach (ITouchListener listener in _TouchListeners) listener.OnCancel();
            _EventsToProcess.Clear();
        }
    }
}
