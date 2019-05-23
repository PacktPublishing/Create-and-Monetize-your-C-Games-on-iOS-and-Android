using OpenTK;
using System;

namespace Engine.Shared.Interfaces
{
    /// <summary> This interface will be used to detect touch input </summary>
    public interface ITouchListener
    {
        /// <summary> Function that is called when the touch listener is pressed </summary>
        /// <param name="id"></param>
        /// <param name="position"></param>
        Boolean OnPress(Int32 id, Vector2 position);

        /// <summary> Function that is called when the mouse moves </summary>
        /// <param name="id"></param>
        /// <param name="position"></param>
        Boolean OnMove(Int32 id, Vector2 position);

        /// <summary> Function that is called when the mouse is released </summary>
        /// <param name="id"></param>
        /// <param name="position"></param>
        void OnRelease(Int32 id, Vector2 position);

        /// <summary> Called when the touch has been cancelled </summary>
        void OnCancel();

        /// <summary> Check to see if the listener has been touched </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        Boolean IsTouched(Vector2 position);

        /// <summary> Whether or not the listener is enabled </summary>
        Boolean TouchEnabled { get; set; }

        /// <summary> The priority of the touch </summary>
        Int32 TouchOrder { get; set; }

        /// <summary> Whether or not the touch order has changed </summary>
        Boolean TouchOrderChanged { get; set; }

        /// <summary> Whether or not the listener is listening for a touch moving event </summary>
        Boolean ListeningForMove { get; set; }
    }
}
