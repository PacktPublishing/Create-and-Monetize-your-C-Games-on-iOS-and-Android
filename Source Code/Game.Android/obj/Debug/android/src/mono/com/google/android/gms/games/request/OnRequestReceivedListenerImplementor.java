package mono.com.google.android.gms.games.request;


public class OnRequestReceivedListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.android.gms.games.request.OnRequestReceivedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onRequestReceived:(Lcom/google/android/gms/games/request/GameRequest;)V:GetOnRequestReceived_Lcom_google_android_gms_games_request_GameRequest_Handler:Android.Gms.Games.Request.IOnRequestReceivedListenerInvoker, Xamarin.GooglePlayServices.Games\n" +
			"n_onRequestRemoved:(Ljava/lang/String;)V:GetOnRequestRemoved_Ljava_lang_String_Handler:Android.Gms.Games.Request.IOnRequestReceivedListenerInvoker, Xamarin.GooglePlayServices.Games\n" +
			"";
		mono.android.Runtime.register ("Android.Gms.Games.Request.IOnRequestReceivedListenerImplementor, Xamarin.GooglePlayServices.Games, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", OnRequestReceivedListenerImplementor.class, __md_methods);
	}


	public OnRequestReceivedListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == OnRequestReceivedListenerImplementor.class)
			mono.android.TypeManager.Activate ("Android.Gms.Games.Request.IOnRequestReceivedListenerImplementor, Xamarin.GooglePlayServices.Games, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onRequestReceived (com.google.android.gms.games.request.GameRequest p0)
	{
		n_onRequestReceived (p0);
	}

	private native void n_onRequestReceived (com.google.android.gms.games.request.GameRequest p0);


	public void onRequestRemoved (java.lang.String p0)
	{
		n_onRequestRemoved (p0);
	}

	private native void n_onRequestRemoved (java.lang.String p0);

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
