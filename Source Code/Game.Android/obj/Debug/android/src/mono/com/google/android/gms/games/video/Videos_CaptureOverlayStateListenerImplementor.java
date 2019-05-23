package mono.com.google.android.gms.games.video;


public class Videos_CaptureOverlayStateListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.android.gms.games.video.Videos.CaptureOverlayStateListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCaptureOverlayStateChanged:(I)V:GetOnCaptureOverlayStateChanged_IHandler:Android.Gms.Games.Video.IVideosCaptureOverlayStateListenerInvoker, Xamarin.GooglePlayServices.Games\n" +
			"";
		mono.android.Runtime.register ("Android.Gms.Games.Video.IVideosCaptureOverlayStateListenerImplementor, Xamarin.GooglePlayServices.Games, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Videos_CaptureOverlayStateListenerImplementor.class, __md_methods);
	}


	public Videos_CaptureOverlayStateListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Videos_CaptureOverlayStateListenerImplementor.class)
			mono.android.TypeManager.Activate ("Android.Gms.Games.Video.IVideosCaptureOverlayStateListenerImplementor, Xamarin.GooglePlayServices.Games, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCaptureOverlayStateChanged (int p0)
	{
		n_onCaptureOverlayStateChanged (p0);
	}

	private native void n_onCaptureOverlayStateChanged (int p0);

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
