using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Game.Shared.Competitive;
using Android.Gms.Common.Apis;
using Engine.Android;
using Android.Gms.Games;
using Game.Shared.Base;
using Android.Gms.Common;

namespace Game.Android.Source.Competitive
{
    /// <summary> The controller that reports scores and achievements to Google Play Services </summary>
    public class PlayServicesController : CompetitiveController
    {
        /// <summary> The client that communicates to the Google APIs </summary>
        private GoogleApiClient _GoogleApiClient;
        /// <summary> The action to fire when the controller has connected </summary>
        private Action<Boolean> _OnConnected;

        /// <summary> Creates the controller & api client </summary>
        public PlayServicesController()
        {
            _GoogleApiClient = new GoogleApiClient.Builder(GameActivity.Instance)
                .AddConnectionCallbacks((MainActivity)GameActivity.Instance)
                .AddOnConnectionFailedListener((MainActivity)GameActivity.Instance)
                .AddApi(GamesClass.API).AddScope(GamesClass.ScopeGames)
                .Build();
        }

        /// <summary> Connects the player to Google Play Services </summary>
        /// <param name="onComplete"></param>
        public override void Connect(Action<Boolean> onComplete)
        {
            if (_GoogleApiClient.IsConnected)
            {
                onComplete?.Invoke(true);
                return;
            }

            _OnConnected = onComplete;
            _GoogleApiClient.Connect();
        }

        /// <summary> Updates the progress of the achievement </summary>
        /// <param name="achievement"></param>
        public override void UpdateAchievement(Achievement achievement)
        {
            if (!_GoogleApiClient.IsConnected) return;
            GamesClass.Achievements.SetSteps(_GoogleApiClient, achievement.Id, (Int32)(achievement.PercentageComplete * 100));
            if (achievement.Achieved) GamesClass.Achievements.Unlock(_GoogleApiClient, achievement.Id);
        }

        /// <summary> Views the achievements on Google Play Services </summary>
        public override void ViewAchievements()
        {
            GameActivity.Instance.StartActivityForResult(GamesClass.Achievements.GetAchievementsIntent(_GoogleApiClient), Constants.ACHIEVEMENT_REQUEST_CODE);
        }

        /// <summary> Called when the Google API has connected - passes the result to the PlayServicesController </summary>
        /// <param name="connectionHint"></param>
        public void OnConnected(Bundle connectionHint)
        {
            _OnConnected?.Invoke(true);
            _OnConnected = null;
        }

        /// <summary> Called when the Google API connection fails - passes the result to the PlayServicesController </summary>
        /// <param name="result"></param>
        public void OnConnectionFailed(ConnectionResult result)
        {
            if (result.HasResolution)
            {
                try
                {
                    result.StartResolutionForResult(GameActivity.Instance, Constants.SIGN_IN_REQUEST_CODE);
                }
                catch (IntentSender.SendIntentException e)
                {
                    _GoogleApiClient.Connect();
                }
            }
            else
            {
                _OnConnected?.Invoke(false);
                _OnConnected = null;
            }
        }

        /// <summary> Called when the Google API connection has been suspended - tells the PlayServicesController </summary>
        /// <param name="cause"></param>
        public void OnConnectionSuspended(Int32 cause)
        {
            _GoogleApiClient.Connect();
        }

        /// <summary> Called when the result has returned from signing in - will connect the API client </summary>
        /// <param name="resultCode"></param>
        public void OnResultReceived(Result resultCode)
        {
            if (resultCode == Result.Ok) _GoogleApiClient.Connect();
            else
            {
                _OnConnected?.Invoke(false);
                _OnConnected = null;
            }
        }
    }
}