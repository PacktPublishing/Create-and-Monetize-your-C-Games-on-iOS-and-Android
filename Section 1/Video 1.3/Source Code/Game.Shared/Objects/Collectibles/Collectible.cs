using Engine.Shared.Graphics;
using Engine.Shared.Graphics.Drawables;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Game.Shared.Base;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using XVector2 = Microsoft.Xna.Framework.Vector2;

namespace Game.Shared.Objects.Collectibles
{
    /// <summary> The collectible that the player can collect </summary>
    public abstract class Collectible : AnimatedSprite
    {
        /// <summary> The physics body for the collectible </summary>
        protected Body _Box;
        /// <summary> Whether or not the collectible has been collected </summary>
        protected Boolean _Collected;

        /// <summary> The position of the collectible </summary>
        public override Vector2 Position
        {
            get => base.Position;
            set
            {
                base.Position = value;
                if (_Box != null) _Box.Position = ConvertUnits.ToSimUnits(new XVector2(value.X, value.Y));
            }
        }

        /// <summary> Creates the collectible </summary>
        /// <param name="canvas"></param>
        /// <param name="zOrder"></param>
        /// <param name="texture"></param>
        /// <param name="imageWidth"></param>
        /// <param name="imageHeight"></param>
        /// <param name="numFrames"></param>
        /// <param name="fps"></param>
        protected Collectible(Canvas canvas, int zOrder, Texture texture, int imageWidth, int imageHeight, int numFrames, float fps, Vector2 bodyScale)
            : base(canvas, zOrder, texture, imageWidth, imageHeight, numFrames, fps)
        {
            InitialiseCollectible(bodyScale);
        }

        /// <summary> Creates the collectible from CSV </summary>
        /// <param name="canvas"></param>
        /// <param name="values"></param>
        protected Collectible(Canvas canvas, String[] values)
            : base(canvas, values)
        {
            Vector2 bodyScale = Vector2.One;

            foreach (String data in values)
            {
                String[] splitData = data.Split('|');

                switch (splitData[0])
                {
                    case "BodyScale":
                        bodyScale = new Vector2(Single.Parse(splitData[1], CultureInfo.InvariantCulture), Single.Parse(splitData[2], CultureInfo.InvariantCulture));
                        break;
                }
            }

            InitialiseCollectible(bodyScale);
        }

        /// <summary> Initialises the collectible </summary>
        /// <param name="bodyScale"></param>
        private void InitialiseCollectible(Vector2 bodyScale)
        {
            _Box = PhysicsWorld.Instance.CreateRectangle(new XVector2(_Position.X, _Position.Y), new XVector2(_Width * bodyScale.X, _Height * bodyScale.Y), 8);
            _Box.BodyType = BodyType.Static;
            _Box.CollisionCategories = Constants.COLLECTIBLE_CATEGORY;
            _Box.CollidesWith = Constants.PLAYER_CATEGORY;
            _Box.OnCollision += OnCollision;

            Playing = true;
            Offset = new Vector2(_Width / 2, _Height / 2);
        }

        /// <summary> Called when the collectible has collided with an object - if it is the player it will be collected </summary>
        /// <param name="fixtureA"></param>
        /// <param name="fixtureB"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        protected bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories == Constants.PLAYER_CATEGORY && !_Collected)
            {
                _Collected = true;
                OnCollect();
            }
            return false;
        }

        /// <summary> Called when the object has been collected </summary>
        protected abstract void OnCollect();

        /// <summary> Disposes of the collectible </summary>
        public override void Dispose()
        {
            base.Dispose();
            PhysicsWorld.Instance.RemoveBody(_Box);
        }
    }
}
