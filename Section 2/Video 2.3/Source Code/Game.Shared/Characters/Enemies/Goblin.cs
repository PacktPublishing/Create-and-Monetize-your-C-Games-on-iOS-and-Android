using Engine.Shared.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Game.Shared.Base;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Shared.Characters.Enemies
{
    /// <summary> The enemy Goblin </summary>
    public class Goblin : Enemy
    {
        /// <summary> Creates the goblin </summary>
        public Goblin()
            : base(ZippyGame.MainCanvas, ZOrders.GOBLIN, Texture.GetTexture("Content/Graphics/Goblin.png"), 128, 128, 9, 9, new Vector2(0.21f, 0.43f))
        {
            InitialiseGoblin();
        }

        /// <summary> Creates the Goblin from CSV </summary>
        /// <param name="values"></param>
        public Goblin(String[] values)
            : base(ZippyGame.MainCanvas, values)
        {
            InitialiseGoblin();
        }

        /// <summary> Initialises all elements of the Goblin </summary>
        private void InitialiseGoblin()
        {
            _Health = 20;
            _AttackPower = 30;
            _ConnectingJoint.MaxMotorTorque = 10000;
            _ConnectingJoint.MotorEnabled = true;
            _ConnectingJoint.MotorSpeed = (Single)(Math.PI * 2) * (Constants.ENEMY_SPEED / -_WheelCircumference);
            _Scale.X = 1f;
            ScaleOrigin = new OpenTK.Vector2(64, 64);
            ChangeAnimation("Run");
            Playing = true;
            AnimEndBehaviour = EndBehaviour.LOOP;
        }

        /// <summary> Called when the goblin collides with an object - if its the player it damages the player </summary>
        /// <param name="fixtureA"></param>
        /// <param name="fixtureB"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        protected override Boolean OnBodyCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories == Constants.PLATFORM_TURN_CATEGORY)
            {
                _ConnectingJoint.MotorSpeed *= -1f;
                Scale = new OpenTK.Vector2(-Scale.X, Scale.Y);
                return true;
            }
            else if (fixtureB.CollisionCategories == Constants.PLATFORM_CATEGORY) return true;

            return false;
        }

        /// <summary> Adds the animations for the goblin </summary>
        public override void AddAnimations()
        {
            _Animations.Add("Idle", new AnimationFrames(0, 1));
            _Animations.Add("Run", new AnimationFrames(1, 9));
        }

        /// <summary> Called to kill the goblin </summary>
        protected override void OnDeath()
        {
            base.OnDeath();
            Dispose();
        }
    }
}
