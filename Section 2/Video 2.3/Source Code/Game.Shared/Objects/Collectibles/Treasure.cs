using Engine.Shared.Graphics;
using Game.Shared.Base;
using Game.Shared.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Shared.Objects.Collectibles
{
    /// <summary> The treasure that is the main goal for the game </summary>
    public class Treasure : Collectible
    {
        public Treasure()
            : base(ZippyGame.MainCanvas, ZOrders.COLLECTIBLE, Texture.GetTexture("Content/Graphics/Treasure.png"), 64, 64, 1, 1, new OpenTK.Vector2(0.875f, 0.53f))
        {

        }

        /// <summary> Creates the treasure from CSV </summary>
        /// <param name="values"></param>
        public Treasure(String[] values)
            : base(ZippyGame.MainCanvas, values)
        {

        }

        /// <summary> Called when collected - ends the level </summary>
        protected override void OnCollect()
        {
            GameScene.Instance.OnComplete();
        }
    }
}
