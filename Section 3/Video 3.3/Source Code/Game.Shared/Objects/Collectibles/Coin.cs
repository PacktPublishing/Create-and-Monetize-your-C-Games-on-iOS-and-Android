using Engine.Shared.Graphics;
using Game.Shared.Base;
using Game.Shared.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Shared.Objects.Collectibles
{
    /// <summary> The coin collectible </summary>
    public class Coin : Collectible
    {
        public Coin()
            : base(ZippyGame.MainCanvas, ZOrders.COLLECTIBLE, Texture.GetTexture("Content/Graphics/Coin.png"), 64, 64, 4, 10, new OpenTK.Vector2(0.5f, 0.5f))
        {
        }

        /// <summary> Creates the coin from CSV </summary>
        /// <param name="values"></param>
        public Coin(String[] values)
            : base(ZippyGame.MainCanvas, values)
        {

        }

        /// <summary> Called when the object has been collected - it will increase the number of coins in the game </summary>
        protected override void OnCollect()
        {
            GameScene.Instance.IncrementCoins(this);
            Dispose();
        }
    }
}
