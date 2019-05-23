using Game.Shared.Characters.Enemies;
using Game.Shared.Objects;
using Game.Shared.Objects.Collectibles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Shared.Base
{
    public static class SceneTypeMap
    {
        /// <summary> The type map for each type in the scene </summary>
        private static readonly Dictionary<String, Type> _TypeMap = new Dictionary<String, Type>
        {
            { "Goblin", typeof(Goblin) },
            { "Coin", typeof(Coin) },
            { "Platform", typeof(Platform) },
            { "Treasure", typeof(Treasure) }
        };

        /// <summary> Checks whether or not the type exists </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean TypeExists(String value)
        {
            return _TypeMap.ContainsKey(value);
        }

        /// <summary> Gets the type given the reference name </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Type GetType(String value)
        {
            return _TypeMap[value];
        }
    }
}
