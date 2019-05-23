using Engine.Shared.Graphics;
using Engine.Shared.Graphics.Drawables;
using Game.Shared.Base;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Shared.Objects.UI
{
    /// <summary> The main health bar for the player </summary>
    public class HealthBar : Sprite
    {
        /// <summary> The percentage that the bar is full </summary>
        private Single _PercentageFull;

        public HealthBar(Vector2 healthBgPosition)
            : base(ZippyGame.UICanvas, ZOrders.HEALTH_FG, Texture.GetTexture("Content/Graphics/UI/HealthFg.png"))
        {
            Position = healthBgPosition + new Vector2(6, 6);
            Visible = true;
            _PercentageFull = 1;
        }

        /// <summary> Updates the bar's display percentage </summary>
        public void UpdateBar(Single health)
        {
            _PercentageFull = health / Constants.PLAYER_MAX_HEALTH;
            _VerticesShouldUpdate = true;
        }

        /// <summary> Generates the vertices for the health bar </summary>
        /// <returns></returns>
        public override List<Vertex> GenerateVertices()
        {
            return new List<Vertex>
            {
                new Vertex(new Vector3(0, 0, 0f), new Vector2(0, 1), _Colour),
                new Vertex(new Vector3(_Width * _PercentageFull, 0, 0f), new Vector2(_PercentageFull, 1), _Colour),
                new Vertex(new Vector3(_Width * _PercentageFull, _Height, 0f), new Vector2(_PercentageFull, 0), _Colour),
                new Vertex(new Vector3(0, _Height, 0f), new Vector2(0, 0), _Colour),
            };
        }
    }
}
