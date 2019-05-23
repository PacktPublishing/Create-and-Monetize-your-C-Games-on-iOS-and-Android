using Engine.Shared.Base;
using Engine.Shared.Graphics.Drawables;
using Engine.Shared.Interfaces;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Shared.Touch
{
    public class Button : ITouchListener
    {
        /// <summary> The order of the touch input </summary>
        protected Int32 _TouchOrder;
        /// <summary> The sprite that shows the button </summary>
        protected readonly Sprite _Sprite;
        /// <summary> Whether or not the button is being pressed </summary>
        protected Boolean _Pressed;
        /// <summary> Whether or not the button is visible </summary>
        protected Boolean _Visible;
        /// <summary> The ID of the touch that pressed the button </summary>
        protected Int32 _TouchId;
        /// <summary> Whether or not the button is enabled </summary>
        protected Boolean _TouchEnabled;

        /// <summary> Whether or not the button is enabled </summary>
        public virtual Boolean TouchEnabled
        {
            get { return _TouchEnabled; }
            set
            {
                _TouchEnabled = value;
                if (!value) OnCancel();
            }
        }
        /// <summary> The order of the touches </summary>
        public virtual Int32 TouchOrder
        {
            get { return _TouchOrder; }
            set
            {
                _TouchOrder = value;
                TouchOrderChanged = true;
            }
        }
        /// <summary> Whether or not the touch order has changed </summary>
        public virtual Boolean TouchOrderChanged { get; set; }
        /// <summary> Whether or not the button is listening to move events </summary>
        public virtual Boolean ListeningForMove { get; set; }
        /// <summary> Sets the visibility of the button </summary>
        public virtual Boolean Visible
        {
            get { return _Visible; }
            set
            {
                _Visible = value;
                _Sprite.Visible = value;
            }
        }
        /// <summary> The position of the buttonbv</summary>
        public Vector2 Position
        {
            get { return _Sprite.Position; }
            set
            {
                _Sprite.Position = value;
            }
        }
        /// <summary> The action to call when the button has been pressed </summary>
        public Action<Button> OnButtonPress;
        /// <summary> The action to call when the button has been released </summary>
        public Action<Button> OnButtonRelease;
        /// <summary> The sprite of the button </summary>
        public Sprite Sprite => _Sprite;

        /// <summary> Creates the button </summary>
        /// <param name="touchOrder"></param>
        /// <param name="sprite"></param>
        public Button(Int32 touchOrder, Sprite sprite)
        {
            TouchOrder = touchOrder;
            _Sprite = sprite;

            TouchManager.Instance.AddTouchListener(this);
        }

        /// <summary> Whether or not the button has been touched </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public virtual Boolean IsTouched(Vector2 position)
        {
            position.X -= Renderer.Instance.TargetDimensions.X / 2;
            position.Y -= Renderer.Instance.TargetDimensions.Y / 2;
            position.Y *= -1;
            return position.X > _Sprite.Position.X &&
                position.X < _Sprite.Position.X + _Sprite.Width &&
                position.Y > _Sprite.Position.Y &&
                position.Y < _Sprite.Position.Y + _Sprite.Height;
        }

        /// <summary> Called when the button is pressed - will trigger the OnButtonPress action </summary>
        /// <param name="id"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public Boolean OnPress(Int32 id, Vector2 position)
        {
            if (_Pressed) return false;
            _Pressed = true;
            _TouchId = id;
            OnButtonPress?.Invoke(this);
            return true;
        }

        /// <summary> Called when the button has been moved - does not do anything </summary>
        /// <param name="id"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public Boolean OnMove(Int32 id, Vector2 position)
        {
            return false;
        }

        /// <summary> Called when the button is released - if the ID is correct it will fire the OnRelease event // </summary>
        /// <param name="id"></param>
        /// <param name="position"></param>
        public void OnRelease(Int32 id, Vector2 position)
        {
            if (!_Pressed || id != _TouchId) return;
            _Pressed = false;
            _TouchId = -1;
            OnButtonRelease?.Invoke(this);
        }

        /// <summary> Called to cancel the press </summary>
        public void OnCancel()
        {
            _Pressed = false;
            _TouchId = -1;
        }

        /// <summary> Disposes of the button </summary>
        public void Dispose()
        {
            _Sprite.Dispose();
            OnButtonPress = null;
            OnButtonRelease = null;
            TouchManager.Instance.RemoveTouchListener(this);
        }
    }
}
