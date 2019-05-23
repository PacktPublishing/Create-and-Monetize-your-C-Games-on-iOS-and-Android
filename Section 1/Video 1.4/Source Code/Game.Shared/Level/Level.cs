using System;
using System.IO;
#if __ANDROID__
using Android.Content.Res;
using Engine.Android;
#endif

namespace Game.Shared.Level
{
    /// <summary> The data for each level </summary>
    public class Level
    {
        /// <summary> The data for the level </summary>
        private String[] _Data;

        /// <summary> The data for the level </summary>
        public String[] Data => _Data;

        /// <summary> The name of the file </summary>
        /// <param name="fileName"></param>
        public Level(String fileName)
        {
            String data = "";
#if __ANDROID__
            AssetManager assets = GameActivity.Instance.Assets;
            Boolean fileFound = false;
            try
            {
                using (StreamReader reader = new StreamReader(assets.Open(fileName)))
                {
                    data = reader.ReadToEnd();
                    fileFound = true;
                }
            }
            catch (Exception e)
            {

            }
            if (!fileFound) throw new FileNotFoundException($"Given level {fileName} does not exist!");
#elif __IOS__
            if (!File.Exists(fileName)) throw new FileNotFoundException($"Given level {fileName} does not exist!");
            data = File.ReadAllText(fileName);
#endif

            _Data = data.Split('\n');
        }
    }
}
