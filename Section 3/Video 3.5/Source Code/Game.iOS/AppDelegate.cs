using Engine.iOS;
using Engine.Shared.Base;
using Foundation;
using Game.Shared;
using UIKit;

namespace Game.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : BaseAppDelegate
    {
        /// <summary> The instance of the game </summary>
        private BaseGame _GameInstance;

        /// <summary> The instance of the game </summary>
        public override BaseGame GameInstance => _GameInstance ?? (_GameInstance = new ZippyGame());

    }
}


