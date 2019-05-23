#if __ANDROID__
using Android.Media;
#elif __IOS__
using AVFoundation;
using Foundation;
#endif
using System;

namespace Engine.Shared.Audio
{
    /// <summary> This class is used to play audio </summary>
    public class AudioPlayer
    {
        /// <summary> The data for the player </summary>
        private readonly AudioData _Data;
        /// <summary> Whether or not the audio is playing </summary>
        private Boolean _Playing;
        /// <summary> The volume of the player </summary>
        private Single _Volume;

        /// <summary> The volume of the sound </summary>
        public Single Volume
        {
            get { return _Volume; }
            set
            {
                _Volume = value;
                ShouldUpdateVolume = true;
            }
        }
        /// <summary> Whether or not the volume should be updated </summary>
        public Boolean ShouldUpdateVolume { get; set; }
        /// <summary> The category for the sound </summary>
        public AudioManager.Category Category { get; }

#if __ANDROID__
        /// <summary> The player for the sound </summary>
        private MediaPlayer _Player;
#elif __IOS__
        /// <summary> The player for the sound </summary>
        private AVAudioPlayer _Player;
#endif

        /// <summary> Creates the audio file </summary>
        /// <param name="file"></param>
        public AudioPlayer(String file, Boolean shouldLoop, AudioManager.Category category, Single volume)
        {
            _Data = AudioData.GetData(file);
            _Playing = true;
            Category = category;
            _Volume = volume;

#if __ANDROID__
            _Player = new MediaPlayer();
            _Player.SetDataSource(_Data.Descriptor, _Data.StartOffset, _Data.Length);
            UpdateVolume();
            _Player.Looping = shouldLoop;
            _Player.Prepare();
            _Player.Start();
            _Player.Completion += OnComplete;
#elif __IOS__
            _Player = new AVAudioPlayer(_Data.Data, "wav", out NSError error);
            _Player.NumberOfLoops = shouldLoop ? -1 : 0;
            UpdateVolume();
            _Player.FinishedPlaying += OnComplete;
            _Player.Play();
#endif
            AudioManager.Instance.AddAudioPlayer(this);
        }

#if __ANDROID__        
        /// <summary> Called when the Android sound is complete - it tells the AudioManager to remove the sound </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComplete(object sender, EventArgs e)
#elif __IOS__
        private void OnComplete(object sender, AVStatusEventArgs e)
#endif
        {
            Stop();
        }

        /// <summary> Checks for when the sound is complete and tells the AudioManager when done </summary>
        public void Update()
        {
            if (!_Playing) return;
            if (ShouldUpdateVolume) UpdateVolume();
        }

        /// <summary> Updates the volume of the AudioPlayer </summary>
        private void UpdateVolume()
        {
            Single categoryVolume = Category == AudioManager.Category.MUSIC ? AudioManager.Instance.MusicVolume : AudioManager.Instance.EffectVolume;
            Single volume = AudioManager.Instance.MasterVolume * categoryVolume * _Volume;
#if __ANDROID__
            _Player.SetVolume(volume, volume);
#elif __IOS__
            _Player.Volume = volume;
#endif
            ShouldUpdateVolume = false;
        }

        /// <summary> Pauses the audio </summary>
        public void Pause()
        {
            if (!_Playing) return;
#if __ANDROID__
            _Player.Pause();
#elif __IOS__
            _Player.Pause();
#endif
            _Playing = false;
        }

        /// <summary> Resumes the audio </summary>
        public void Resume()
        {
            if (_Playing) return;

#if __ANDROID__
            _Player.Start();
#elif __IOS__
            _Player.Play();
#endif

            _Playing = true;
        }

        /// <summary> Stops playing the audio </summary>
        public void Stop()
        {
            AudioManager.Instance.RemoveAudioPlayer(this);
#if __ANDROID__
            _Player.Stop();
#elif __IOS__
            _Player.Stop();
#endif
        }

        public void Dispose()
        {
#if __ANDROID__
            _Player.Dispose();
            _Player = null;
#elif __IOS__
            _Player.Dispose();
            _Player = null;
#endif
        }
    }
}
