using System;
using Engine.Shared.Graphics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Game.Shared.Base;
using FarseerPhysics;
using FarseerPhysics.Dynamics.Contacts;
using Game.Shared.Characters.Enemies;
using Game.Shared.Scenes;
using Game.Shared.Objects.UI;

namespace Game.Shared.Characters
{
    /// <summary> The Zippy character </summary>
    public class Zippy : Character
    {
        /// <summary> The health bar for the player </summary>
        private readonly HealthBar _HealthBar;
        /// <summary> Whether or not Zippy is jumping </summary>
        private Boolean _Jumping;
        /// <summary> The amount of time to be invulnerable after damage </summary>
        private TimeSpan _RechargeTime;
        /// <summary> Whether or not the player is dead </summary>
        private Boolean _Dead;
        /// <summary> How many lives the player has </summary>
        private Int32 _Lives;

        /// <summary> The category for the player </summary>
        public override Category CollisionCategory => Constants.PLAYER_CATEGORY;
        /// <summary> The number of lives remaining </summary>
        public Int32 Lives => _Lives;

        /// <summary> The Zippy character </summary>
        public Zippy(HealthBar healthBar)
            : base(ZippyGame.MainCanvas, ZOrders.PLAYER, Texture.GetTexture("Content/Graphics/Soldier.png"), 144, 144, 13, 15, new Vector2(0.181f, 0.5f))
        {
            _Health = Constants.PLAYER_MAX_HEALTH;
            _HealthBar = healthBar;
            Playing = true;
            _EndBehaviour = EndBehaviour.LOOP;
            _ConnectingJoint.MaxMotorTorque = 10000;
            _ConnectingJoint.MotorEnabled = true;
            ScaleOrigin = new OpenTK.Vector2(72, 72);
            _Lives = 1;
        }

        /// <summary> Resets the player to the initial level </summary>
        public void Reset()
        {
            _Lives = 1;
            _RechargeTime = TimeSpan.Zero;
            Initialise();
        }

        /// <summary> Initialises the player </summary>
        public void Initialise()
        {
            _Dead = false;
            _Health = Constants.PLAYER_MAX_HEALTH;
            _HealthBar.UpdateBar(_Health);
        }

        /// <summary> Adds the animations to the character </summary>
        public override void AddAnimations()
        {
            _Animations.Add("Idle", new AnimationFrames(0, 0));
            _Animations.Add("Jump", new AnimationFrames(1, 4));
            _Animations.Add("Run", new AnimationFrames(5, 13));
        }

        /// <summary> Tells the player to jump </summary>
        public void Jump()
        {
            if (_Jumping) return;
            _Wheel.ApplyLinearImpulse(ConvertUnits.ToSimUnits(new Vector2(0, 70)));
            ChangeAnimation("Jump");
            _Jumping = true;
            _EndBehaviour = EndBehaviour.STOP;
        }

        /// <summary> Starts moving Zippy </summary>
        /// <param name="speed"></param>
        private void StartMove(Single speed)
        {
            if (_Jumping)
            {
                _MainBody.LinearVelocity = new Vector2(-speed / 4f, _MainBody.LinearVelocity.Y);
                _Wheel.LinearVelocity = new Vector2(-speed / 4f, _Wheel.LinearVelocity.Y);
            }
            else
            {
                ChangeAnimation("Run");
            }

            _ConnectingJoint.MotorSpeed = (Single)(Math.PI * 2) * speed;
        }

        /// <summary> Moves the player left </summary>
        public void MoveLeft()
        {
            StartMove(Constants.PLAYER_SPEED / _WheelCircumference);
            Scale = new OpenTK.Vector2(-1f, 1f);
        }

        /// <summary> Moves Zippy to the right </summary>
        public void MoveRight()
        {
            StartMove(Constants.PLAYER_SPEED / -_WheelCircumference);
            Scale = new OpenTK.Vector2(1f, 1f);
        }

        /// <summary> Stops moving the player </summary>
        public void Stop()
        {
            _ConnectingJoint.MotorSpeed = 0;
            if (_Jumping)
            {
                _Wheel.LinearVelocity = new Vector2(0, _Wheel.LinearVelocity.Y);
                _MainBody.LinearVelocity = new Vector2(0, _MainBody.LinearVelocity.Y);
            }
            else ChangeAnimation("Idle");
        }

        /// <summary> Updates the character </summary>
        /// <param name="timeSinceUpdate"></param>
        public override void Update(TimeSpan timeSinceUpdate)
        {
            if (_Dead) return;
            base.Update(timeSinceUpdate);
            if (_RechargeTime > TimeSpan.Zero)
            {
                _RechargeTime -= timeSinceUpdate;
                Double value = _RechargeTime.TotalSeconds % 0.2 - 0.1;
                if (value > 0) Colour = new OpenTK.Vector4(1, 1, 1, 1);
                else Colour = new OpenTK.Vector4(1, 1, 1, 0);
            }
            else Colour = new OpenTK.Vector4(1, 1, 1, 1);

            if (Position.Y - Height < ZippyGame.Camera.BottomRight.Y - ZippyGame.Camera.Dimensions.Y) Damage(_Health);
        }

        /// <summary> Damages the player only if the recharge time has depleted</summary>
        /// <param name="damage"></param>
        public override void Damage(Single damage)
        {
            if (_RechargeTime <= TimeSpan.Zero)
            {
                _RechargeTime = TimeSpan.FromSeconds(1);
                base.Damage(damage);
                _HealthBar.UpdateBar(_Health);
            }
        }

        /// <summary> Called when colliding with an object - if its the floor, it will play the idle animation </summary>
        /// <param name="fixtureA"></param>
        /// <param name="fixtureB"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        protected override bool OnWheelCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (_Jumping)
            {
                if (fixtureB.CollisionCategories == Constants.PLATFORM_CATEGORY)
                {
                    ChangeAnimation(_ConnectingJoint.MotorSpeed != 0 ? "Run" : "Idle");
                    _Jumping = false;
                    Playing = true;
                    _EndBehaviour = EndBehaviour.LOOP;
                }
                else if (fixtureB.CollisionCategories == Constants.ENEMY_CATEGORY)
                {
                    Enemy enemy = (Enemy)fixtureB.Body.UserData;
                    Single enemyMid = enemy.Position.Y + (enemy.Height / 2);
                    if (_Wheel.LinearVelocity.Y < 0 && Position.Y + Height / 2 > enemyMid)
                    {
                        enemy.Damage(30);
                        _Wheel.LinearVelocity = new Vector2(_Wheel.LinearVelocity.X, 0);
                        _MainBody.LinearVelocity = new Vector2(_MainBody.LinearVelocity.X, 0);
                        _Wheel.ApplyLinearImpulse(ConvertUnits.ToSimUnits(0, 20));
                    }
                    else
                    {
                        Damage(enemy.AttackPower);
                    }
                }
            }
            else
            {
                if (fixtureB.CollisionCategories == Constants.ENEMY_CATEGORY)
                {
                    Enemy enemy = (Enemy)fixtureB.Body.UserData;
                    Damage(enemy.AttackPower);
                }
            }

            if (fixtureB.CollisionCategories == Constants.LIMIT_CATEGORY) return true;
            return base.OnWheelCollision(fixtureA, fixtureB, contact);
        }

        /// <summary> Called when the body collides with an object - if its the limit it will return true </summary>
        /// <param name="fixtureA"></param>
        /// <param name="fixtureB"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        protected override bool OnBodyCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories == Constants.LIMIT_CATEGORY) return true;
            return base.OnBodyCollision(fixtureA, fixtureB, contact);
        }

        /// <summary> Performs the death logic for the player </summary>
        protected override void OnDeath()
        {
            _Dead = true;
            _Lives--;
            GameScene.Instance.OnDeath?.Invoke();
        }
    }
}
