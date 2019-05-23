﻿using Engine.Shared.Base;
using UIKit;

namespace Engine.iOS
{
    public partial class GLViewController : UIViewController
    {
        /// <summary> The instance of the game </summary>
        protected readonly BaseGame _GameInstance;

        public GLViewController(BaseGame gameInstance) : base()
        {
            _GameInstance = gameInstance;
        }
        /// <summary> Loads & creates our view and sets the game instance </summary>
        public override void LoadView()
        {
            View = new GameView(UIScreen.MainScreen.Bounds);
            ((GameView)View).SetGameInstance(_GameInstance);
        }

        /// <summary> Called when the view is loaded </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ((GameView)View).Run(60f);
            // Perform any additional setup after loading the view, typically from a nib.
        }
    }
}
