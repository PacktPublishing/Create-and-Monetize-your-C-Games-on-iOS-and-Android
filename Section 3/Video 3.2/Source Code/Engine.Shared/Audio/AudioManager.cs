#if __IOS__
using AVFoundation;
#endif
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Shared.Audio
{
    /// <summary> The audio manager will be used to play the audio </summary>
    public class AudioManager
    {
        /// <summary> The different categories of audio </summary>
        public enum Category
        {
            MUSIC,
            EFFECT
        }

        /// <summary> The instance of the audio manager </summary>
        private static AudioManager _Instance;
        /// <summary> The volume for all sounds </summary>
        private Single _MasterVolume = 1f;
        /// <summary> The volume for the Music category </summary>
        private Single _MusicVolume = 1f;
        /// <summary> The volume for the effect category </summary>
        private Single _EffectVolume = 1f;
        /// <summary> The currently active audio players </summary>
        private readonly List<AudioPlayer> _ActiveAudio = new List<AudioPlayer>();
        /// <summary> The list of audio players to be removed </summary>
        private readonly List<AudioPlayer> _AudioToRemove = new List<AudioPlayer>();

        /// <summary> The instance of the audio manager </summary>
        public static AudioManager Instance => _Instance ?? (_Instance = new AudioManager());

        /// <summary> The volume of the AudioManager </summary>
        public Single MasterVolume
        {
            get { return _MasterVolume; }
            set
            {
                _MasterVolume = value;
                foreach (AudioPlayer player in _ActiveAudio) player.ShouldUpdateVolume = true;
            }
        }
        /// <summary> The volume for the Music category </summary>
        public Single MusicVolume
        {
            get { return _MusicVolume; }
            set
            {
                _MusicVolume = value;
                foreach (AudioPlayer player in _ActiveAudio.Where(s => s.Category == Category.MUSIC)) player.ShouldUpdateVolume = true;
            }
        }
        /// <summary> The volume for the Effect category </summary>
        public Single EffectVolume
        {
            get { return _EffectVolume; }
            set
            {
                _EffectVolume = value;
                foreach (AudioPlayer player in _ActiveAudio.Where(s => s.Category == Category.EFFECT)) player.ShouldUpdateVolume = true;
            }
        }

        /// <summary> Creates the manager </summary>
        private AudioManager()
        {
        }

        /// <summary> Updates all playing audio files </summary>
        internal void Update()
        {
            foreach (AudioPlayer player in _ActiveAudio)
            {
                player.Update();
            }

            foreach (AudioPlayer player in _AudioToRemove)
            {
                player.Dispose();
                _ActiveAudio.Remove(player);
            }
            _AudioToRemove.Clear();
        }

        /// <summary> Gets the ID for a channel for the AudioPlayer </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        internal void AddAudioPlayer(AudioPlayer player)
        {
            _ActiveAudio.Add(player);
        }

        /// <summary> Removes the given audioplayer from the Dictionary </summary>
        /// <param name="player"></param>
        internal void RemoveAudioPlayer(AudioPlayer player)
        {
            if (!_AudioToRemove.Contains(player)) _AudioToRemove.Add(player);
        }

        /// <summary> Pauses all the currently playing audio files </summary>
        internal void Pause()
        {
            foreach (AudioPlayer player in _ActiveAudio)
            {
                player.Pause();
            }
        }

        /// <summary> Resumes all currently playing audio files </summary>
        internal void Resume()
        {
            foreach (AudioPlayer player in _ActiveAudio)
            {
                player.Resume();
            }
        }

        /// <summary> Disposes of all the audio players </summary>
        internal void Dispose()
        {
            foreach (AudioPlayer player in _ActiveAudio)
            {
                player.Stop();
            }

            foreach (AudioPlayer player in _AudioToRemove)
            {
                player.Dispose();
            }
            _ActiveAudio.Clear();
            _AudioToRemove.Clear();
        }
    }
}
