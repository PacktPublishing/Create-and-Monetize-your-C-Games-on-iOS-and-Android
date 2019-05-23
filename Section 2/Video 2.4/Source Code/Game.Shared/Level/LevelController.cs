using Game.Shared.Base;
using System;
using System.Collections.Generic;

namespace Game.Shared.Level
{
    /// <summary> The controller for the levels </summary>
    public class LevelController
    {
        /// <summary> The current level the player is on </summary>
        private Int32 _CurrentLevel;
        /// <summary> The levels in the game </summary>
        private readonly List<Level> _Levels = new List<Level>();
        /// <summary> The instance of the level controller </summary>
        private static LevelController _Instance;

        /// <summary> The current level index </summary>
        public Int32 CurrentLevelIndex => _CurrentLevel;
        /// <summary> The current level </summary>
        public Level CurrentLevel => _Levels[_CurrentLevel];
        /// <summary> The number of levels in the game </summary>
        public Int32 NumberOfLevels => _Levels.Count;
        /// <summary> Whether or not the game is complete </summary>
        public Boolean GameComplete => _CurrentLevel >= _Levels.Count;
        /// <summary> The instance of the level controller </summary>
        public static LevelController Instance => _Instance ?? (_Instance = new LevelController());

        /// <summary> The level controller - loads all the levels </summary>
        private LevelController()
        {
            LoadLevelData();
        }

        /// <summary> Loads the data for the levels </summary>
        private void LoadLevelData()
        {
            for (Int32 i = 0; i < Constants.NUMBER_OF_LEVELS; i++)
            {
                _Levels.Add(new Level($"Content/Levels/Level{i}.csv"));
            }
        }

        /// <summary> Increase the current level for the player </summary>
        public void ChangeLevel()
        {
            _CurrentLevel++;
        }

        /// <summary> Resets the current level </summary>
        public void Reset()
        {
            _CurrentLevel = 0;
        }
    }
}
