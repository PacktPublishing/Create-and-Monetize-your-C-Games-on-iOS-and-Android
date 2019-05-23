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
        /// <summary> The custom epsilon value so that comparisons work correctly on iOS </summary>
        public const Double EPSILON = 1.175494351E-38;

        /// <summary> The IDs for the achievements on android </summary>
        public static readonly String[] ANDROID_ACHIEVEMENT_IDS = new[]
        {
            "CggIlKb83HYQAhAC",
            "CggIlKb83HYQAhAD",
        };
        /// <summary> The IDs for the leaderboards on Android </summary>
        public static readonly String[] ANDROID_LEADERBOARD_IDS = new[]
        {
            "",
        };
        /// <summary> The IDs for the achievements on iOS </summary>
        public static readonly String[] IOS_ACHIEVEMENT_IDS = new[]
        {
            "com.xamarintutorials.zippysadventure.finishlevel",
            "com.xamarintutorials.zippysadventure.finishalllevels",
        };
        /// <summary> The IDs for the achievements on iOS </summary>
        public static readonly String[] IOS_LEADERBOARD_IDS = new[]
        {
            "com.xamarintutorials.zippysadventure.totalscores",
        };

        /// <summary> The index of the finish level achievement </summary>
        public const Int32 ACHIEVEMENT_FINISH_LEVEL = 0;
        /// <summary> The index of the finish all levels achievement </summary>
        public const Int32 ACHIEVEMENT_FINISH_ALL_LEVELS = 1;
        /// <summary> The index of the score leaderboard </summary>
        public const Int32 LEADERBOARD_SCORE = 0;
        /// <summary> The request code for signing in Google Play Services </summary>
        public const Int32 SIGN_IN_REQUEST_CODE = 4000;
        /// <summary> The request code for viewing achievements on Google Play Services </summary>
        public const Int32 ACHIEVEMENT_REQUEST_CODE = 4001;
    }
}
