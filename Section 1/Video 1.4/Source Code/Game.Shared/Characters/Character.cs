using Engine.Shared.Graphics.Drawables;
using System;
using System.Collections.Generic;
using Engine.Shared.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using Game.Shared.Base;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics;
using System.Globalization;

namespace Game.Shared.Characters
{
    /// <summary> The character will hold basic information for any player or enemy </summary>
    public abstract class Character : AnimatedSprite
    {
        public struct AnimationFrames
        {
            /// <summary> The start frame for the animation </summary>
            public Int32 StartFrame;
            /// <summary> The number of frames in the animation </summary>
            public Int32 EndFrame;

            public AnimationFrames(Int32 startFrame, Int32 endFrames)
            {
                StartFrame = startFrame;
                EndFrame = endFrames;
            }
        }

        /// <summary> The health of the player </summary>
        protected Single _Health;
        /// <summary> The animations on the character </summary>
        protected readonly Dictionary<String, AnimationFrames> _Animations = new Dictionary<String, AnimationFrames>();
        /// <summary> The current animation on the character </summary>
        protected AnimationFrames _CurrentAnimation;
        /// <summary> The main physics body of the character </summary>
        protected Body _MainBody;
        /// <summary> The wheel that moves the character </summary>
        protected Body _Wheel;
        /// <summary> The joint connectingthe body to the wheel </summary>
        protected RevoluteJoint _ConnectingJoint;
        /// <summary> The radius of the wheel </summary>
        protected Single _WheelRadius;
        /// <summary> The circumference of the wheel </summary>
        protected Single _WheelCircumference;

        /// <summary> The collision category for the character </summary>
        public abstract Category CollisionCategory { get; }
        /// <summary> Whether or not the character is active </summary>
        public Boolean Active { get; set; }

        /// <summary> Creates the character </summary>
        /// <param name="canvas"></param>
        /// <param name="zOrder"></param>
        /// <param name="texture"></param>
        /// <param name="imageWidth"></param>
        /// <param name="imageHeight"></param>
        /// <param name="numFrames"></param>
        /// <param name="fps"></param>
        public Character(Canvas canvas, Int32 zOrder, Texture texture, Int32 imageWidth, Int32 imageHeight, Int32 numFrames, Single fps, Vector2 bodyScale)
            : base(canvas, zOrder, texture, imageWidth, imageHeight, numFrames, fps)
        {
            InitialiseCharacter(bodyScale);
        }

        /// <summary> Creates the character from CSV </summary>
        /// <param name="canvas"></param>
        /// <param name="values"></param>
        public Character(Canvas canvas, String[] values)
            : base(canvas, values)
        {
            Vector2 bodyScale = Vector2.One;
            Vector2 startPosition = Vector2.Zero;

            foreach (String data in values)
            {
                String[] splitData = data.Split('|');

                switch (splitData[0])
                {
                    case "BodyScale":
                        bodyScale = new Vector2(Single.Parse(splitData[1], CultureInfo.InvariantCulture), Single.Parse(splitData[2], CultureInfo.InvariantCulture));
                        break;
                    case "StartPosition":
                        startPosition = new Vector2(Single.Parse(splitData[1], CultureInfo.InvariantCulture), Single.Parse(splitData[2], CultureInfo.InvariantCulture));
                        break;
                    case "Active":
                        Active = Boolean.Parse(splitData[1]);
                        break;
                }
            }

            InitialiseCharacter(bodyScale);
            SetStartPosition(startPosition);
        }
        
        /// <summary> Initialises the character </summary>
        private void InitialiseCharacter(Vector2 bodyScale)
        {
            AddAnimations();
            if (!_Animations.ContainsKey("Idle")) throw new ArgumentException("The list of animations for the character does not have the default Idle animation");
            _CurrentAnimation = _Animations["Idle"];

            Offset = new OpenTK.Vector2(_Width / 2, _Height / 2);

            Single boxHeight = (_Height - (_Width / 2)) * bodyScale.Y;
            Vector2 boxPosition = new Vector2(0, boxHeight / 2);
            _MainBody = PhysicsWorld.Instance.CreateRectangle(boxPosition, new Vector2(_Width * bodyScale.X, boxHeight), 5);
            _MainBody.BodyType = BodyType.Dynamic;
            _MainBody.CollisionCategories = CollisionCategory;
            _MainBody.Restitution = 0f;
            _MainBody.Friction = 0.5f;
            _MainBody.UserData = this;

            _WheelRadius = (_Width / 2) * bodyScale.X;
            _Wheel = PhysicsWorld.Instance.CreateCircle(boxPosition - new Vector2(0, boxHeight / 2), _WheelRadius, 5);
            _Wheel.BodyType = BodyType.Dynamic;
            _Wheel.CollisionCategories = CollisionCategory;
            _Wheel.Restitution = 0f;
            _Wheel.Friction = 1f;
            _Wheel.UserData = this;
            _WheelCircumference = (Single)(Math.PI * (_Width * bodyScale.X));

            _MainBody.IgnoreCollisionWith(_Wheel);
            _Wheel.IgnoreCollisionWith(_MainBody);
            _MainBody.FixedRotation = true;

            _MainBody.OnCollision += OnBodyCollision;
            _Wheel.OnCollision += OnWheelCollision;

            _ConnectingJoint = PhysicsWorld.Instance.CreateRevoluteJoint(_MainBody, _Wheel, Vector2.Zero);
            _ConnectingJoint.CollideConnected = false;
        }

        /// <summary> Called when the main body collides with an object - if its the floot it will stop movement </summary>
        /// <param name="fixtureA"></param>
        /// <param name="fixtureB"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        protected virtual Boolean OnBodyCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories == Constants.PLATFORM_CATEGORY)
            {
                return true;
            }
            return false;
        }

        /// <summary> Called when the character collides with the floor - if its the platform, it will stop movement </summary>
        /// <param name="fixtureA"></param>
        /// <param name="fixtureB"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        protected virtual Boolean OnWheelCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories == Constants.PLATFORM_CATEGORY)
            {
                return true;
            }
            return false;
        }

        /// <summary> Sets the start position of the character </summary>
        /// <param name="position"></param>
        public void SetStartPosition(Vector2 position)
        {
            Vector2 physicsStart = ConvertUnits.ToSimUnits(new Vector2(position.X, position.Y));
            _MainBody.Position = physicsStart;
            _Wheel.Position = physicsStart;
            UpdatePosition();
        }

        /// <summary> Updates the position of the sprite based on the physics body </summary>
        /// <param name="timeSinceUpdate"></param>
        public override void Update(TimeSpan timeSinceUpdate)
        {
            base.Update(timeSinceUpdate);
            UpdatePosition();
        }

        /// <summary> Updates the position of the sprite based on the physics object </summary>
        public virtual void UpdatePosition()
        {
            Vector2 position = ConvertUnits.ToDisplayUnits(_MainBody.Position);
            Position = new OpenTK.Vector2(position.X, position.Y);
            Rotation = _MainBody.Rotation;
        }

        /// <summary> Damages the character </summary>
        /// <param name="damage"></param>
        public virtual void Damage(Single damage)
        {
            _Health -= damage;
            if (_Health <= 0) OnDeath();
        }

        /// <summary> Changes the animation on the character </summary>
        /// <param name="anim"></param>
        public virtual void ChangeAnimation(String anim)
        {
            if (!_Animations.ContainsKey(anim)) throw new ArgumentException($"Given animation {anim} does not exist for the character");
            _CurrentAnimation = _Animations[anim];
            CurrentFrame = _CurrentAnimation.StartFrame;
        }

        /// <summary> Whether or not the character can be updated </summary>
        /// <returns></returns>
        public override bool CanUpdate()
        {
            return Active;
        }

        /// <summary> Checks the bounds of the animation </summary>
        protected override void CheckBounds()
        {
            if ((Fps > 0 && _CurrentFrame >= _CurrentAnimation.EndFrame) || (Fps < 0 && _CurrentFrame < _CurrentAnimation.StartFrame))
            {
                OnEndReached();
            }
        }

        /// <summary> Called when the end of the animation is reached - performs a different action based on the enum </summary>
        protected override void OnEndReached()
        {
            switch (_EndBehaviour)
            {
                case EndBehaviour.LOOP:
                    if (Fps > 0) _CurrentFrame = _CurrentAnimation.StartFrame;
                    else _CurrentFrame = _CurrentAnimation.EndFrame;
                    break;
                case EndBehaviour.REVERSE:
                    if (Fps > 0) _CurrentFrame = Math.Max(0, _CurrentAnimation.EndFrame - 1);
                    else _CurrentFrame = Math.Min(_CurrentAnimation.StartFrame + 1, _Uvs.Length - 1);
                    Fps = -Fps;
                    break;
                case EndBehaviour.STOP:
                    Playing = false;
                    if (Fps > 0) _CurrentFrame = _CurrentAnimation.EndFrame;
                    else _CurrentFrame = _CurrentAnimation.StartFrame;
                    break;
            }
            OnComplete?.Invoke(this);
        }

        /// <summary> Disposes of the character </summary>
        public override void Dispose()
        {
            base.Dispose();
            _Animations.Clear();
            if (_MainBody != null) PhysicsWorld.Instance.RemoveBody(_MainBody);
            if (_Wheel != null) PhysicsWorld.Instance.RemoveBody(_Wheel);
            if (_ConnectingJoint != null) PhysicsWorld.Instance.RemoveJoint(_ConnectingJoint);
        }

        /// <summary> Adds the animations to the character </summary>
        public abstract void AddAnimations();
        /// <summary> Performs the logic for when the character has been killed </summary>
        protected abstract void OnDeath();
    }
}
