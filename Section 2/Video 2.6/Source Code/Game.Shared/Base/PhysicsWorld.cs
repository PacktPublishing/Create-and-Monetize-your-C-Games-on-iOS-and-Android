using Engine.Shared.Base;
using Engine.Shared.Interfaces;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Shared.Base
{
    /// <summary> The physics world that will control all physics objects </summary>
    public class PhysicsWorld : IUpdatable, IDisposable
    {
        /// <summary> The list of bodies in the physics world </summary>
        private readonly List<Body> _BodyList = new List<Body>();
        /// <summary> The list of joints in the scene </summary>
        private readonly List<Joint> _JointList = new List<Joint>();

        /// <summary> The instance of the physics world </summary>
        private World _World;
        /// <summary> The gravity for the world </summary>
        private Vector2 _Gravity;
        /// <summary> The instance of the physics world </summary>
        private static PhysicsWorld _Instance;

        /// <summary> The instance of the world </summary>
        public static PhysicsWorld Instance => _Instance ?? (_Instance = new PhysicsWorld());
        /// <summary> The instance of the physics world </summary>
        public World World
        {
            get
            {
                return _World;
            }
        }
        /// <summary> The gravity of the world </summary>
        public Vector2 Gravity
        {
            get { return _Gravity; }
            set
            {
                _Gravity = value;
                _World.Gravity = value;
            }
        }
        /// <summary> Whether or not physics are enabled for the world </summary>
        public Boolean Enabled { get; set; }

        public PhysicsWorld()
        {
            ConvertUnits.SetDisplayUnitToSimUnitRatio(350);
            _Gravity = new Vector2(0, -9.81f);
            _World = new World(_Gravity);
            UpdateManager.Instance.AddUpdatable(this);
        }

        /// <summary> Creates a rectangle in the position given with the size given </summary>
        /// <param name="centerPosition"></param>
        /// <param name="size"></param>
        /// <param name="density"></param>
        /// <returns></returns>
        public Body CreateRectangle(Vector2 centerPosition, Vector2 size, Single density)
        {
            Body rectangle = BodyFactory.CreateRectangle(_World, ConvertUnits.ToSimUnits(size.X), ConvertUnits.ToSimUnits(size.Y), density);
            rectangle.Position = ConvertUnits.ToSimUnits(centerPosition);
            _BodyList.Add(rectangle);
            return rectangle;
        }

        /// <summary> Creates a circle with the given center position and radius </summary>
        /// <param name="centerPosition"></param>
        /// <param name="radius"></param>
        /// <param name="density"></param>
        /// <returns></returns>
        public Body CreateCircle(Vector2 centerPosition, Single radius, Single density)
        {
            Body circle = BodyFactory.CreateCircle(_World, ConvertUnits.ToSimUnits(radius), density);
            circle.Position = ConvertUnits.ToSimUnits(centerPosition);
            _BodyList.Add(circle);
            return circle;
        }

        /// <summary> Creates a revolute joint between 2 given bodies - this joint ensures that the 2 bodies are always in the same place </summary>
        /// <param name="bodyA"></param>
        /// <param name="bodyB"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public RevoluteJoint CreateRevoluteJoint(Body bodyA, Body bodyB, Vector2 position)
        {
            RevoluteJoint joint = JointFactory.CreateRevoluteJoint(_World, bodyA, bodyB, ConvertUnits.ToSimUnits(position));
            _JointList.Add(joint);
            return joint;
        }

        /// <summary> Removes the body from the world </summary>
        /// <param name="body"></param>
        public void RemoveBody(Body body)
        {
            body.Dispose();
            body.UserData = null;
            body.CollidesWith = Category.None;
            _BodyList.Remove(body);
        }

        /// <summary> Removes the joint </summary>
        /// <param name="joint"></param>
        public void RemoveJoint(Joint joint)
        {
            _World.RemoveJoint(joint);
            _JointList.Remove(joint);
        }

        /// <summary> Updates the physics world </summary>
        /// <param name="timeSinceUpdate"></param>
        public void Update(TimeSpan timeSinceUpdate)
        {
            _World.Step((Single)timeSinceUpdate.TotalSeconds);
        }

        /// <summary> Disposes of the physics world </summary>
        public void Dispose()
        {
            foreach (Body body in _BodyList) body.Dispose();
            _BodyList.Clear();

            foreach (Joint joint in _JointList.ToList()) RemoveJoint(joint);
            _JointList.Clear();

            UpdateManager.Instance.RemoveUpdatable(this);
            _Instance = null;
        }

        /// <summary> Whether or not the physics can be updated </summary>
        /// <returns></returns>
        public bool CanUpdate()
        {
            return Enabled;
        }
    }
}
