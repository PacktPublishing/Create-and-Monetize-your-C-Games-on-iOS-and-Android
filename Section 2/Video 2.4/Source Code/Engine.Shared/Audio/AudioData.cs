#if __ANDROID__
using Android.Content.Res;
using Engine.Android;
using Java.IO;
#elif __IOS__
using AVFoundation;
using Foundation;
#endif
using System;
using System.Collections.Generic;

namespace Engine.Shared.Audio
{
    /// <summary> The data that the AudioPlayer should play </summary>
    public class AudioData
    {
        /// <summary> All the currently registered audio data </summary>
        private static Dictionary<String, AudioData> _Data = new Dictionary<String, AudioData>();
        /// <summary> The filename for the data </summary>
        public String Filename { get; }

#if __ANDROID__
        /// <summary> The file for the AudioData </summary>
        private AssetFileDescriptor _AssetFileDescriptor;
        /// <summary> The descriptor for the Asset File </summary>
        public FileDescriptor Descriptor { get; }
        /// <summary> The start offset for the data </summary>
        public Int64 StartOffset { get; }
        /// <summary> The length of the data </summary>
        public Int64 Length { get; }
#elif __IOS__
        public NSData Data { get; }
#endif

        /// <summary> Creates the AudioData </summary>
        /// <param name="filename"></param>
        private AudioData(String filename)
        {
            Filename = filename;
#if __ANDROID__
            _AssetFileDescriptor = GameActivity.Instance.Assets.OpenFd(filename);
            Descriptor = _AssetFileDescriptor.FileDescriptor;
            StartOffset = _AssetFileDescriptor.StartOffset;
            Length = _AssetFileDescriptor.Length;
#elif __IOS__
            Data = NSData.FromFile(filename);
#endif
        }

        /// <summary> Gets the given audio data, or creates it if it doesn't exist </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static AudioData GetData(String filename)
        {
            if (_Data.ContainsKey(filename)) return _Data[filename];
            AudioData data = new AudioData(filename);
            _Data.Add(filename, data);
            return data;
        }

        /// <summary> Disposes of the data </summary>
        public void Dispose()
        {
#if __ANDROID__
            _AssetFileDescriptor.Dispose();
            Descriptor.Dispose();
#elif __IOS__
            Data.Dispose();
#endif
            _Data.Remove(Filename);
        }
    }
}
