#if __ANDROID__
using Android.Content;
using Engine.Android;
using Android.Preferences;
#elif __IOS__
using Foundation;
#endif
using Engine.Shared.Base;
using Engine.Shared.Graphics;
using Engine.Shared.Graphics.Drawables;
using Engine.Shared.Touch;
using Game.Shared.Base;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Shared.Scenes
{
    /// <summary> The scene which displays the store - shows the icons & purchases </summary>
    public class StoreScene : FadeScene
    {
        /// <summary> The button to return to the menu </summary>
        private readonly Button _BackButton;
        /// <summary> The button to purchase the item </summary>
        private readonly Button _PurchaseButton;
        /// <summary> The icon for the element to purchase </summary>
        private readonly Sprite _Icon;
        /// <summary> The text on the element </summary>
        private readonly TextDisplay _ElementText;
        /// <summary> The text which shows the status of the purchase </summary>
        private readonly TextDisplay _StatusText;
        /// <summary> The title of the scene </summary>
        private readonly TextDisplay _Title;
        /// <summary> Whether or not the store is connected </summary>
        private Boolean _Connected;

        /// <summary> The button to go back to the main menu </summary>
        public Button BackButton => _BackButton;

        public StoreScene()
        {
            _BackButton = new Button(ZOrders.SCORE_TEXT, new Sprite(ZippyGame.UICanvas, ZOrders.SCORE_TEXT, Texture.GetTexture("Content/Graphics/Buttons/BackButton.png")))
            {
                Position = new Vector2(-910, (Renderer.Instance.TargetDimensions.Y / 2) - 306),
                Visible = true,
                TouchEnabled = false
            };

            _PurchaseButton = new Button(ZOrders.SCORE_TEXT, new Sprite(ZippyGame.UICanvas, ZOrders.SCORE_TEXT, Texture.GetTexture("Content/Graphics/Buttons/StoreButton.png")) { Colour = new Vector4(1, 1, 1, 0) })
            {
                Position = new Vector2(-128, -400),
                Visible = true,
                TouchEnabled = false,
            };

            _Icon = new Sprite(ZippyGame.UICanvas, ZOrders.SCORE_TEXT, Texture.GetTexture("Content/Graphics/UI/LivesIcon.png"))
            {
                Visible = true,
                Position = new Vector2(-300, -100),
                Colour = new Vector4(1, 1, 1, 0)
            };
            AddDrawable(_Icon);

            _ElementText = new TextDisplay(ZippyGame.UICanvas, ZOrders.SCORE_TEXT, Texture.GetTexture("Content/Graphics/UI/kromasky.png"), Constants.KROMASKY_CHARACTERS, 32, 32)
            {
                Text = "start lives x3",
                Position = new Vector2(-200, -100),
                Visible = true,
                Colour = new Vector4(1, 1, 1, 0)
            };
            AddDrawable(_ElementText);

            _StatusText = new TextDisplay(ZippyGame.UICanvas, ZOrders.SCORE_TEXT, Texture.GetTexture("Content/Graphics/UI/kromasky.png"), Constants.KROMASKY_CHARACTERS, 32, 32)
            {
                Position = new Vector2(0, 150),
                Visible = true
            };
            AddDrawable(_StatusText);

            _Title = new TextDisplay(ZippyGame.UICanvas, ZOrders.SCORE_TEXT, Texture.GetTexture("Content/Graphics/UI/kromasky.png"), Constants.KROMASKY_CHARACTERS, 32, 32)
            {
                Position = new Vector2(0, 450),
                Text = "store",
                Visible = true
            };
            _Title.Offset = new Vector2(_Title.Width / 2, _Title.Height / 2);
            AddDrawable(_Title);
            
            SetElementAlpha(0);
        }

        /// <summary> Toggles the Enabled state of the buttons </summary>
        /// <param name="toEnable"></param>
        public override void ToggleButtonStates(Boolean toEnable)
        {
            _PurchaseButton.TouchEnabled = toEnable;
            _BackButton.TouchEnabled = toEnable;
        }

        /// <summary> Sets the status of the store </summary>
        /// <param name="text"></param>
        public void SetStatus(String text)
        {
            _StatusText.Text = text;
            _StatusText.Offset = new Vector2(_StatusText.Width / 2, _StatusText.Height / 2);
        }

        /// <summary> Sets the alpha on all elements in the scene </summary>
        /// <param name="alpha"></param>
        protected override void SetElementAlpha(Single alpha)
        {
            _Title.Colour = new Vector4(1, 1, 1, alpha);
            _BackButton.Sprite.Colour = new Vector4(1, 1, 1, alpha);
            _StatusText.Colour = new Vector4(1, 1, 1, alpha);
            if (!_Connected) return;
            _PurchaseButton.Sprite.Colour = new Vector4(1, 1, 1, alpha);
            _Icon.Colour = new Vector4(1, 1, 1, alpha);
            _ElementText.Colour = new Vector4(1, 1, 1, alpha);
        }

        /// <summary> Shows the store's products </summary>
        public void ShowProducts()
        {
            _Connected = true;
            _PurchaseButton.OnButtonRelease += OnPurchase;
            if (!_Fading) SetElementAlpha(_TargetAlpha);
        }

        /// <summary> Called when the purchase button has been pressed - it starts the purchase </summary>
        /// <param name="button"></param>
        private void OnPurchase(Button button)
        {
            SetStatus("purchasing...");
            ToggleButtonStates(false);
            PurchaseManager.Instance.Purchase(Constants.LIVESX3_ID, OnPurchaseComplete, "test");
        }

        /// <summary> Called when the purchase has completed - awards the extra lives if successful </summary>
        /// <param name="toPurchase"></param>
        private void OnPurchaseComplete(Boolean success)
        {
            if (success)
            {

#if __ANDROID__
                ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(GameActivity.Instance);
                Int32 lives = preferences.GetInt(Constants.LIVES_SAVE_ID, 1);
                ISharedPreferencesEditor editor = preferences.Edit();
                editor.PutInt(Constants.LIVES_SAVE_ID, lives += 3);
                editor.Apply();
#elif __IOS__
                Int32 lives = (Int32)NSUserDefaults.StandardUserDefaults.IntForKey(Constants.LIVES_SAVE_ID);
                if (lives == 0) lives = 1;
                lives += 3;
                NSUserDefaults.StandardUserDefaults.SetInt(lives, Constants.LIVES_SAVE_ID);
                NSUserDefaults.StandardUserDefaults.Synchronize();
#endif
                SetStatus("purchase complete!");
            }
            else
            {
                SetStatus("purchase failed!");
            }
            ToggleButtonStates(true);
        }

        /// <summary> Disposes of the scene </summary>
        public override void Dispose()
        {
            base.Dispose();
            _PurchaseButton.Dispose();
            _BackButton.Dispose();
        }
    }
}
