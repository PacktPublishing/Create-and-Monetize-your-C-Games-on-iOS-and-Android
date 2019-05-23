using System;
using Plugin.InAppBilling;
using System.Collections.Generic;
using System.Text;
using Plugin.InAppBilling.Abstractions;
using System.Threading.Tasks;

namespace Game.Shared.Base
{
    /// <summary> This class is used to purchase an item & controls the outcome </summary>
    public class PurchaseManager
    {
        /// <summary> The instance of the purchase manager </summary>
        private static PurchaseManager _Instance;
        /// <summary> Whether or not the manager is connected </summary>
        private Boolean _Connected;

        /// <summary> Whether or not the store is connected </summary>
        public Boolean Connected => _Connected;
        /// <summary> Creates the instance of the purchase manager </summary>
        public static PurchaseManager Instance => _Instance ?? (_Instance = new PurchaseManager());

        /// <summary> The purchase manager is used to control the purchases in the game </summary>
        private PurchaseManager()
        {

        }

        /// <summary> Connects the purchase manager to the relevant service </summary>
        /// <returns></returns>
        public async void Connect(Action onComplete)
        {
            try
            {
                _Connected = await CrossInAppBilling.Current.ConnectAsync();
            }
            catch (Exception e)
            {

            }
            onComplete?.Invoke();
        }

        /// <summary> Purchases an item - returns true if successful </summary>
        /// <param name="productId"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public async void Purchase(String productId, Action<Boolean> onComplete, String payload = "")
        {
            Boolean success = false;

            if (!_Connected) Connect(null);
            if (_Connected)
            {
                try
                {
                    InAppBillingPurchase purchase = await CrossInAppBilling.Current.PurchaseAsync(productId, ItemType.InAppPurchase, payload);
                    if (purchase != null)
                    {
#if __IOS__
                        success = true;
#else
                        success = await Consume(purchase);
#endif
                    }
                }
                catch (Exception e)
                {

                }
            }
            onComplete?.Invoke(success);
        }

        /// <summary> Consumes the given purchase </summary>
        /// <param name="purchase"></param>
        /// <returns></returns>
        public async Task<Boolean> Consume(InAppBillingPurchase purchase)
        {
            InAppBillingPurchase consumedProduct = null;
            try
            {
                consumedProduct = await CrossInAppBilling.Current.ConsumePurchaseAsync(purchase.ProductId, purchase.PurchaseToken);
            }
            catch (Exception e)
            {

            }
            return consumedProduct != null;
        }

        /// <summary> Gets a list of all the available products </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        public async void GetProducts(String[] productIds, Action<List<InAppBillingProduct>> onComplete)
        {
            List<InAppBillingProduct> products = new List<InAppBillingProduct>();

            if (!_Connected) Connect(null);
            if (_Connected)
            {
                try
                {
                    IEnumerable<InAppBillingProduct> obtainedProducts = await CrossInAppBilling.Current.GetProductInfoAsync(ItemType.InAppPurchase, productIds);
                    if (obtainedProducts != null) products.AddRange(obtainedProducts);
                }
                catch (Exception e)
                {

                }
            }
            onComplete(products);
        }

        /// <summary> Gets all items that have not been consumed </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        public async void GetPurchases(Action<List<InAppBillingPurchase>> onComplete)
        {
            List<InAppBillingPurchase> purchases = new List<InAppBillingPurchase>();

            if (!_Connected) Connect(null);
            if (_Connected)
            {
                try
                {
                    IEnumerable<InAppBillingPurchase> obtainedPurchases = await CrossInAppBilling.Current.GetPurchasesAsync(ItemType.InAppPurchase);
                    if (obtainedPurchases != null) purchases.AddRange(obtainedPurchases);
                }
                catch (Exception e)
                {

                }
            }
            onComplete(purchases);
        }

        /// <summary> Disconnects the purchase manager </summary>
        public async void Disconnect()
        {
            try
            {
                await CrossInAppBilling.Current.DisconnectAsync();
            }
            catch (Exception e)
            {

            }
            _Connected = false;
        }
    }
}
