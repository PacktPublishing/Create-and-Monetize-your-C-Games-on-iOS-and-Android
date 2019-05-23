In App Billing Plugin for Xamarin & Windows

Find the latest setup guides, documentation, and testing instructions at: 
https://github.com/jamesmontemagno/InAppBillingPlugin

## Additional Required Setup (Please Read!)

## Android 
This Plugin uses the CurrentActivity Plugin and will add a MainApplication.cs file to your Android application. This is extremely important and should not be deleted. Please see 
http://github.com/jamesmontemagno/CurrentActivityPlugin for more information.

In  your BaseActivity or MainActivity (for Xamarin.Forms) add this code:

protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
{
    base.OnActivityResult(requestCode, resultCode, data);
    InAppBillingImplementation.HandleActivityResult(requestCode, resultCode, data);
}


### Android Security
I recommend reading the Google Play services Security and Design that will walk you through your options on storing your public key. InAppBilling Pluging offers Android developers an additional interface, IInAppBillingVerifyPurchase to implement to verify the purchase with their public key and helper methods to encrypt and decrypt. It is recommended to atleast follow the XOR guidance if you do not want to setup a verification server.

IInAppBillingVerifyPurchase has 1 Method: Task VerifyPurchase(string signedData, string signature). It returns a boolean that validates that the signed data and signature match based on the public key. If you pass in null to the purchase or get purchases methods no verification will be done.

The simplest and easiest (not necessarily the most secure) way is to do the following:

Take your public key and break into 3 parts
Run each through the helper XOR method: Plugin.InAppBilling.InAppBillingImplementation.InAppBillingSecurity.TransformString
Save each value out and put them in your app
Implement the interface with this funcationality:

    public class Verify : IInAppBillingVerifyPurchase
    {
        const string key1 = @"XOR_key1";
        const string key2 = @"XOR_key2";
        const string key3 = @"XOR_key3";

        public Task<bool> VerifyPurchase(string signedData, string signature)
        {

#if __ANDROID__
            var key1Transform = Plugin.InAppBilling.InAppBillingImplementation.InAppBillingSecurity.TransformString(key1, 1);
            var key2Transform = Plugin.InAppBilling.InAppBillingImplementation.InAppBillingSecurity.TransformString(key2, 2);
            var key3Transform = Plugin.InAppBilling.InAppBillingImplementation.InAppBillingSecurity.TransformString(key3, 3);

            return Task.FromResult(Plugin.InAppBilling.InAppBillingImplementation.InAppBillingSecurity.VerifyPurchase(key1Transform + key2Transform + key3Transform, signedData, signature));
#else
            return Task.FromResult(true);
#endif
        }
    }

Plugin.InAppBilling.InAppBillingImplementation.InAppBillingSecurity.VerifyPurchase takes in your public key which you now have reversed back to standard and will do proper RSA validation on the signed data.