package md55c722e3bc780651b2c50c68294a9c6c1;


public class MainActivity
	extends md59f74b8cd51118fb9fa991aac847334a2.GameActivity
	implements
		mono.android.IGCUserPeer,
		com.google.android.gms.common.api.GoogleApiClient.ConnectionCallbacks,
		com.google.android.gms.common.api.GoogleApiClient.OnConnectionFailedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onActivityResult:(IILandroid/content/Intent;)V:GetOnActivityResult_IILandroid_content_Intent_Handler\n" +
			"n_onConnected:(Landroid/os/Bundle;)V:GetOnConnected_Landroid_os_Bundle_Handler:Android.Gms.Common.Apis.GoogleApiClient/IConnectionCallbacksInvoker, Xamarin.GooglePlayServices.Base\n" +
			"n_onConnectionSuspended:(I)V:GetOnConnectionSuspended_IHandler:Android.Gms.Common.Apis.GoogleApiClient/IConnectionCallbacksInvoker, Xamarin.GooglePlayServices.Base\n" +
			"n_onConnectionFailed:(Lcom/google/android/gms/common/ConnectionResult;)V:GetOnConnectionFailed_Lcom_google_android_gms_common_ConnectionResult_Handler:Android.Gms.Common.Apis.GoogleApiClient/IOnConnectionFailedListenerInvoker, Xamarin.GooglePlayServices.Base\n" +
			"";
		mono.android.Runtime.register ("Game.Android.MainActivity, Game.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", MainActivity.class, __md_methods);
	}


	public MainActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == MainActivity.class)
			mono.android.TypeManager.Activate ("Game.Android.MainActivity, Game.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onActivityResult (int p0, int p1, android.content.Intent p2)
	{
		n_onActivityResult (p0, p1, p2);
	}

	private native void n_onActivityResult (int p0, int p1, android.content.Intent p2);


	public void onConnected (android.os.Bundle p0)
	{
		n_onConnected (p0);
	}

	private native void n_onConnected (android.os.Bundle p0);


	public void onConnectionSuspended (int p0)
	{
		n_onConnectionSuspended (p0);
	}

	private native void n_onConnectionSuspended (int p0);


	public void onConnectionFailed (com.google.android.gms.common.ConnectionResult p0)
	{
		n_onConnectionFailed (p0);
	}

	private native void n_onConnectionFailed (com.google.android.gms.common.ConnectionResult p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
