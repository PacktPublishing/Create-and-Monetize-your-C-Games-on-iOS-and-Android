using OpenTK;
using System;

namespace Engine.Shared.Touch
{
    /// <summary> The type of event for the input </summary>
    public struct TouchEvent
    {
        /// <summary> The type of input event </summary>
        public enum Type
        {
            /// <summary> Used when the screen is pressed </summary>
            PRESS,
            /// <summary> Used when the touch has moved </summary>
            MOVE,
            /// <summary> Used when the touch has been released </summary>
            RELEASE
        }

        /// <summary> The type of touch </summary>
        public Type TouchType { get; }
        /// <summary> The position of the InputEvent </summary>
        public Vector2 Position { get; }
        /// <summary> The ID of the touch </summary>
        public Int32 Id { get; }

        /// <summary> Creates the event </summary>
        /// <param name="id"></param>
        /// <param name="position"></param>
        /// <param name="type"></param>
        public TouchEvent(Int32 id, Vector2 position, Type type)
        {
            TouchType = type;
            Position = position;
            Id = id;
        }
    }
}
