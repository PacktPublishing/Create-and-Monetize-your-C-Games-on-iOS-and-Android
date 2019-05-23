using Engine.Shared.Base;
using Engine.Shared.Graphics;
using Engine.Shared.Interfaces;
using Game.Shared.Scenes;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Game.Shared.Base
{
    /// <summary> The camera that looks at an object </summary>
    public class LookAtCamera : Camera, IUpdatable
    {
        /// <summary> The left limit of the camera </summary>
        private Vector2 _TopLeft;
        /// <summary> The right limit of the camera </summary>
        private Vector2 _BottomRight;
        /// <summary> The start position of the camera </summary>
        private Vector2 _StartPosition;

        /// <summary> The top left limit of the camera </summary>
        public Vector2 TopLeft => _TopLeft;
        /// <summary> The bottom right limit of the camera </summary>
        public Vector2 BottomRight => _BottomRight;
        /// <summary> The dimensions of the camera </summary>
        public Vector2 Dimensions => _Dimensions;
        /// <summary> Whether or not the camera is actively chasing the player </summary>
        public Boolean Active { get; set; }

        public LookAtCamera(Vector2 position, Vector2 dimensions) : base(position, dimensions)
        {
            UpdateManager.Instance.AddUpdatable(this);
        }

        /// <summary> Sets the limits of the camera movement </summary>
        public void SetLimits(Vector2 topLeft, Vector2 bottomRight, Vector2 startPosition)
        {
            _TopLeft = topLeft;
            _BottomRight = bottomRight;
            _StartPosition = startPosition;
        }

        /// <summary> Sets the limits of the camera movement </summary>
        /// <param name="data"></param>
        public void SetLimits(String[] data)
        {
            foreach (String value in data)
            {
                String[] splitString = value.Split('|');
                switch (splitString[0])
                {
                    case "TopLeft":
                        _TopLeft = new Vector2(Single.Parse(splitString[1], CultureInfo.InvariantCulture), Single.Parse(splitString[2], CultureInfo.InvariantCulture));
                        break;

                    case "BottomRight":
                        _BottomRight = new Vector2(Single.Parse(splitString[1], CultureInfo.InvariantCulture), Single.Parse(splitString[2], CultureInfo.InvariantCulture));
                        break;

                    case "StartPosition":
                        _StartPosition = new Vector2(Single.Parse(splitString[1], CultureInfo.InvariantCulture), Single.Parse(splitString[2], CultureInfo.InvariantCulture));
                        break;
                }
            }

        }

        /// <summary> Updates the camera's position </summary>
        /// <param name="timeSinceUpdate"></param>
        public void Update(TimeSpan timeSinceUpdate)
        {
            Single x = Math.Max(GameScene.Instance.Zippy.Position.X, _TopLeft.X);
            x = Math.Min(x, _BottomRight.X);

            Single y = Math.Max(GameScene.Instance.Zippy.Position.Y, _BottomRight.Y);
            y = Math.Min(y, _TopLeft.Y);

            Position = new Vector2(x, y);
        }

        /// <summary> Whether or not the camera can be updated </summary>
        /// <returns></returns>
        public Boolean CanUpdate()
        {
            return Active;
        }

        /// <summary> Resets the camera's position </summary>
        public void Reset()
        {
            Position = _StartPosition;
        }
    }
}
