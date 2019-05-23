using Android.App;
using Android.Widget;
using Android.OS;
using Engine.Android;
using Game.Shared;
using Android.Content.PM;
using Android.Content;
using Plugin.InAppBilling;

namespace Game.Android
{
    [Activity(Label = "Game.Android", MainLauncher = true, Theme = "@style/MainTheme", Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : GameActivity
    {
        /// <summary> Creates the activity </summary>
        public MainActivity() 
            : base(new ZippyGame())
        {

        }

        /// <summary> Called when a result from an external activity has been received - it passes the data to the purchase activity </summary>
        /// <param name="requestCode"></param>
        /// <param name="resultCode"></param>
        /// <param name="data"></param>
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            InAppBillingImplementation.HandleActivityResult(requestCode, resultCode, data);
        }
    }
}

