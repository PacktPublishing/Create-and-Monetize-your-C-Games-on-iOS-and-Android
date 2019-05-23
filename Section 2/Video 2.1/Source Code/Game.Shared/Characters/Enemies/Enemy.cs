using Engine.Shared.Graphics;
using FarseerPhysics.Dynamics;
using Game.Shared.Base;
using Game.Shared.Scenes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Shared.Characters.Enemies
{
    /// <summary> The main enemy class </summary>
    public abstract class Enemy : Character
    {
        /// <summary> The strength of the attack </summary>
        protected Int32 _AttackPower;

        /// <summary> The strength of the attack </summary>
        public Int32 AttackPower => _AttackPower;
        /// <summary> The collision category for the enemy </summary>
        public override Category CollisionCategory => Constants.ENEMY_CATEGORY;

        protected Enemy(Canvas canvas, int zOrder, Texture texture, int imageWidth, int imageHeight, int numFrames, float fps, Vector2 bodyScale)
            : base(canvas, zOrder, texture, imageWidth, imageHeight, numFrames, fps, bodyScale)
        {
        }

        /// <summary> Creates the enemy </summary>
        /// <param name="canvas"></param>
        /// <param name="values"></param>
        protected Enemy(Canvas canvas, String[] values)
            : base(canvas, values)
        {
            foreach (String data in values)
            {
                String[] splitData = data.Split('|');
                switch (splitData[0])
                {
                    case "AttackPower":
                        _AttackPower = Int32.Parse(splitData[1]);
                        break;
                }
            }
        }

        /// <summary> Called when the enemy is dead - increases the player's score </summary>
        protected override void OnDeath()
        {
            GameScene.Instance.OnEnemyDefeated(this);
        }
    }
}
