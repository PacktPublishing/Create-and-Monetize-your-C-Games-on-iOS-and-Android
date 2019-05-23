using Engine.Shared.State;
using Engine.Shared.Touch;
using Game.Shared.Analytics;
using Game.Shared.Base;
using Game.Shared.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Shared.States
{
    public class StoreState : State
    {
        /// <summary> The scene with the store elements </summary>
        private StoreScene _Store;

        /// <summary> Called when the state is entered - connects the purchase manager </summary>
        public override void OnEnter()
        {
            AnalyticsManager.Instance.ChangeScreen("Store");
            _Store = new StoreScene() { Visible = true };
            _Store.StartFade(0, 1, () =>
            {
                _Store.SetStatus("loading");
                PurchaseManager.Instance.Connect(OnConnected);
                _Store.BackButton.TouchEnabled = true;
                _Store.BackButton.OnButtonRelease += OnBackRelease;
            });
        }

        /// <summary> Called when the store has connected - displays the products </summary>
        private void OnConnected()
        {
            if (_Store == null || _Store.Fading) return;
            if (PurchaseManager.Instance.Connected)
            {
                _Store.SetStatus(" ");
                _Store.ShowProducts();
                _Store.ToggleButtonStates(true);
            }
            else
            {
                _Store.SetStatus("not connected!");
            }
        }

        /// <summary> Called when the back button has been released - fades & returns to the menu state </summary>
        /// <param name="obj"></param>
        private void OnBackRelease(Button button)
        {
            _Store.StartFade(1, 0, () =>
            {
                StateManager.Instance.ChangeState(new MenuState());
            });
        }

        /// <summary> Updates the state </summary>
        /// <param name="timeSinceUpdate"></param>
        public override void Update(TimeSpan timeSinceUpdate)
        {

        }

        /// <summary> Called when the state exits - disconnects the store </summary>
        public override void OnExit()
        {
            PurchaseManager.Instance.Disconnect();
        }

        /// <summary> Disposes of the state </summary>
        public override void Dispose()
        {
            _Store.Dispose();
            _Store = null;
        }
    }
}
