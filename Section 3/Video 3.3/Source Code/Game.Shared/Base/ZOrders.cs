using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Shared.Base
{
    /// <summary> The drawing orders for the game </summary>
    public static class ZOrders
    {
        /// <summary> The background </summary>
        public const Int32 BACKGROUND = 0;
        /// <summary> The platforms the character stands on </summary>
        public const Int32 PLATFORM = 1;
        /// <summary> The Z order for the collectible </summary>
        public const Int32 COLLECTIBLE = 2;
        /// <summary> The player character </summary>
        public const Int32 PLAYER = 3;
        /// <summary> The goblin enemy </summary>
        public const Int32 GOBLIN = 4;

        /// <summary> The controls for the game </summary>
        public const Int32 CONTROLS = 0;
        /// <summary> The background for the transition </summary>
        public const Int32 TRANSITION_BG = 1;
        /// <summary> The text for the transition </summary>
        public const Int32 TRANSITION_TEXT = 2;
        /// <summary> The text for the score </summary>
        public const Int32 SCORE_TEXT = 3;
        /// <summary> The number of lives </summary>
        public const Int32 LIVES = 3;
        /// <summary> The background graphic for the health bar </summary>
        public const Int32 HEALTH_BG = 4;
        /// <summary> The foreground graphic for the health </summary>
        public const Int32 HEALTH_FG = 5;
    }
}
