using Engine.Shared.Graphics;
using Engine.Shared.Graphics.Drawables;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Game.Shared.Base;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using XVector2 = Microsoft.Xna.Framework.Vector2;

namespace Game.Shared.Objects
{
    /// <summary> The platforms that the player can stand on </summary>
    public class Platform : IDisposable
    {
        /// <summary> The sprites that make up the platform </summary>
        protected readonly List<Sprite> _Sprites = new List<Sprite>();
        /// <summary> The body for the physics object of the platform </summary>
        protected Body _PhysicsBox;
        /// <summary> The position of the platform </summary>
        protected Vector2 _Position;
        /// <summary> Whether or not the platform is visible </summary>
        protected Boolean _Visible;
        /// <summary> Whether or not the physics is enabled </summary>
        protected Boolean _Enabled;
        /// <summary> The body that is used to change the direction on the Goblin </summary>
        protected Body _LeftCollider;
        /// <summary> The body that is used to change the direction on the Goblin </summary>
        protected Body _RightCollider;
        /// <summary> The width of the platform </summary>
        protected Single _Width;
        /// <summary> The height of the platform </summary>
        protected Single _Height;

        /// <summary> The position of the platform </summary>
        public Vector2 Position
        {
            get { return _Position; }
            set
            {
                _Position = value;
                Vector2 currentPosition = value;
                for (Int32 i = 0; i < _Sprites.Count; i++)
                {
                    _Sprites[i].Position = currentPosition;
                    currentPosition.X += _Sprites[i].Width;
                }
                _LeftCollider.Position = ConvertUnits.ToSimUnits(new XVector2(value.X + 25, (value.Y + _Height / 2) + 100));
                _RightCollider.Position = ConvertUnits.ToSimUnits(new XVector2(value.X + _Width - 25, (value.Y + _Height / 2) + 100));
                _PhysicsBox.Position = ConvertUnits.ToSimUnits(new XVector2(value.X + (_Width / 2), value.Y + (_Height / 2)));
            }
        }
        /// <summary> Whether or not the platform is visible </summary>
        public Boolean Visible
        {
            get { return _Visible; }
            set
            {
                _Visible = value;
                foreach (Sprite sprite in _Sprites) sprite.Visible = value;
            }
        }
        /// <summary> Whether or not the physics is enabled </summary>
        public Boolean Enabled
        {
            get { return _Enabled; }
            set
            {
                _Enabled = value;
                _PhysicsBox.Enabled = value;
                _LeftCollider.Enabled = value;
                _RightCollider.Enabled = value;
            }
        }

        /// <summary> Creates the platform </summary>
        /// <param name="leftTexture"></param>
        /// <param name="midTexture"></param>
        /// <param name="rightTexture"></param>
        /// <param name="position"></param>
        /// <param name="numPieces"></param>
        public Platform(String leftTexture, String midTexture, String rightTexture, Vector2 position, Int32 numPieces)
        {
            InitialisePlatform(leftTexture, midTexture, rightTexture, numPieces, position);
        }

        /// <summary> Creates the platform from CSV </summary>
        /// <param name="data"></param>
        public Platform(String[] data)
        {
            String leftTexture = "";
            String midTexture = "";
            String rightTexture = "";
            Int32 numPieces = -1;
            Vector2 position = Vector2.Zero;
            Boolean visible = false;
            Boolean enabled = false;

            foreach (String value in data)
            {
                String[] splitString = value.Split('|');
                switch (splitString[0])
                {
                    case "LeftTexture":
                        leftTexture = splitString[1];
                        break;
                    case "MidTexture":
                        midTexture = splitString[1];
                        break;
                    case "RightTexture":
                        rightTexture = splitString[1];
                        break;
                    case "NumberOfPieces":
                        numPieces = Int32.Parse(splitString[1]);
                        break;
                    case "Position":
                        position = new Vector2(Single.Parse(splitString[1], CultureInfo.InvariantCulture), Single.Parse(splitString[2], CultureInfo.InvariantCulture));
                        break;
                    case "Visible":
                        visible = Boolean.Parse(splitString[1]);
                        break;
                    case "Enabled":
                        enabled = Boolean.Parse(splitString[1]);
                        break;
                }
            }

            if (String.IsNullOrEmpty(leftTexture)) throw new ArgumentNullException(nameof(leftTexture), "A left texture needs to be defined for the platform");
            if (String.IsNullOrEmpty(midTexture)) throw new ArgumentNullException(nameof(midTexture), "A middle texture needs to be defined for the platform");
            if (String.IsNullOrEmpty(rightTexture)) throw new ArgumentNullException(nameof(rightTexture), "A right texture needs to be defined for the platform");
            if (numPieces <= 0) throw new ArgumentOutOfRangeException(nameof(numPieces), "The platform needs to have more than 0 pieces defined");

            InitialisePlatform(leftTexture, midTexture, rightTexture, numPieces, position);
            Visible = visible;
            Enabled = enabled;
        }

        /// <summary> Initialises the platform </summary>
        /// <param name="leftTexture"></param>
        /// <param name="midTexture"></param>
        /// <param name="rightTexture"></param>
        private void InitialisePlatform(String leftTexture, String midTexture, String rightTexture, Int32 numPieces, Vector2 position)
        {
            _Sprites.Add(new Sprite(ZippyGame.MainCanvas, ZOrders.PLATFORM, Texture.GetTexture(leftTexture))
            {
                Visible = false
            });
            for (Int32 i = 1; i < numPieces - 1; i++)
            {
                _Sprites.Add(new Sprite(ZippyGame.MainCanvas, ZOrders.PLATFORM, Texture.GetTexture(midTexture))
                {
                    Visible = false
                });
            }
            _Sprites.Add(new Sprite(ZippyGame.MainCanvas, ZOrders.PLATFORM, Texture.GetTexture(rightTexture))
            {
                Visible = false
            });
            _Width = _Sprites.Sum(s => s.Width);
            _Height = _Sprites.Max(s => s.Height);

            _LeftCollider = PhysicsWorld.Instance.CreateRectangle(new XVector2(position.X + 25, (position.Y + _Height / 2) + 100), new XVector2(50, 100), 0);
            _LeftCollider.CollisionCategories = Constants.PLATFORM_TURN_CATEGORY;
            _LeftCollider.BodyType = BodyType.Static;

            _RightCollider = PhysicsWorld.Instance.CreateRectangle(new XVector2(position.X + _Width - 25, (position.Y + _Height / 2) + 100), new XVector2(50, 100), 0);
            _RightCollider.CollisionCategories = Constants.PLATFORM_TURN_CATEGORY;
            _RightCollider.BodyType = BodyType.Static;

            _PhysicsBox = PhysicsWorld.Instance.CreateRectangle(new XVector2(position.X + _Width / 2, position.Y + _Height / 2), new XVector2(_Width, _Height), 10000);
            _PhysicsBox.BodyType = BodyType.Static;
            _PhysicsBox.CollisionCategories = Constants.PLATFORM_CATEGORY;
            _PhysicsBox.Enabled = false;
            Position = position;
        }

        /// <summary> Disposes of the platform </summary>
        public void Dispose()
        {
            foreach (Sprite sprite in _Sprites) sprite.Dispose();
            _Sprites.Clear();

            PhysicsWorld.Instance.RemoveBody(_LeftCollider);
            PhysicsWorld.Instance.RemoveBody(_RightCollider);
            PhysicsWorld.Instance.RemoveBody(_PhysicsBox);
        }
    }
}
