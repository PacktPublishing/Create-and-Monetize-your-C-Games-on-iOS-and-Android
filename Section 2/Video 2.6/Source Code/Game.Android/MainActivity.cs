using Android.App;
using Android.Widget;
using Android.OS;
using Engine.Android;
using Game.Shared;
using Android.Content.PM;
using Android.Content;
using Plugin.InAppBilling;
using Android.Gms.Common.Apis;
using System;
using Android.Gms.Common;
using Game.Shared.Competitive;
using Game.Android.Source.Competitive;
using Game.Shared.Base;

[assembly: MetaData("com.google.android.gms.games.APP_ID", Value = "@string/app_id")]

namespace Game.Android
{
    [Activity(Label = "Game.Android", MainLauncher = true, Theme = "@style/MainTheme", Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : GameActivity, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        /// <summary> Creates the activity </summary>
        public MainActivity() 
            : base(new ZippyGame())
        {

        }

        /// <summary> Called when the Google API has connected - passes the result to the PlayServicesController </summary>
        /// <param name="connectionHint"></param>
        public void OnConnected(Bundle connectionHint)
        {
            ((PlayServicesController)CompetitiveManager.Instance.PlatformController).OnConnected(connectionHint);
        }

        /// <summary> Called when the Google API connection fails - passes the result to the PlayServicesController </summary>
        /// <param name="result"></param>
        public void OnConnectionFailed(ConnectionResult result)
        {
            ((PlayServicesController)CompetitiveManager.Instance.PlatformController).OnConnectionFailed(result);
        }

        /// <summary> Called when the Google API connection has been suspended - tells the PlayServicesController </summary>
        /// <param name="cause"></param>
        public void OnConnectionSuspended(Int32 cause)
        {
            ((PlayServicesController)CompetitiveManager.Instance.PlatformController).OnConnectionSuspended(cause);
        }

        /// <summary> Called when a result from an external activity has been received - it passes the data to the purchase activity </summary>
        /// <param name="requestCode"></param>
        /// <param name="resultCode"></param>
        /// <param name="data"></param>
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            InAppBillingImplementation.HandleActivityResult(requestCode, resultCode, data);
            if (requestCode == Constants.SIGN_IN_REQUEST_CODE) ((PlayServicesController)CompetitiveManager.Instance.PlatformController).OnResultReceived(resultCode);
        }
    }
}

