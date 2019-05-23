using FarseerPhysics.Dynamics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Shared.Base
{
    /// <summary> The constants in the game </summary>
    public static class Constants
    {
        /// <summary> The category for the platform </summary>
        public const Category PLATFORM_CATEGORY = Category.Cat2;
        /// <summary> The category for the player </summary>
        public const Category PLAYER_CATEGORY = Category.Cat1;
        /// <summary> The category for the enemy </summary>
        public const Category ENEMY_CATEGORY = Category.Cat3;
        /// <summary> The category for the platform for enemies to turn around </summary>
        public const Category PLATFORM_TURN_CATEGORY = Category.Cat4;
        /// <summary> The category for the collectibles </summary>
        public const Category COLLECTIBLE_CATEGORY = Category.Cat5;
        /// <summary> The category for the limits of the screen </summary>
        public const Category LIMIT_CATEGORY = Category.Cat6;

        /// <summary> The max health for the player </summary>
        public const Single PLAYER_MAX_HEALTH = 100;
        /// <summary> The speed of the player </summary>
        public const Single PLAYER_SPEED = 300f;
        /// <summary> The speed of the enemy </summary>
        public const Single ENEMY_SPEED = 150f;

        /// <summary> The number of levels in the game </summary>
        public const Int32 NUMBER_OF_LEVELS = 2;

        /// <summary> The characters for the kromasky font </summary>
        public const String KROMASKY_CHARACTERS = " !\"©❤%_'[]¬+,-./0123456789:;{|}?¦abcdefghijklmnopqrstuvwxyz";

        /// <summary> The ID for the lives x3 in app purchase </summary>
        public const String LIVESX3_ID = "com.xamarintutorials.zippysadventure.lifepack1";
        /// <summary> The ID for retrieving the initial number of lives for the player </summary>
        public const String LIVES_SAVE_ID = "ZippyStartLives";
    }
}
