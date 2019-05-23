package mono.android.app;

public class ApplicationRegistration {

	public static void registerApplications ()
	{
				// Application and Instrumentation ACWs must be registered first.
		mono.android.Runtime.register ("Game.Android.MainApplication, Game.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", md55c722e3bc780651b2c50c68294a9c6c1.MainApplication.class, md55c722e3bc780651b2c50c68294a9c6c1.MainApplication.__md_methods);
		
	}
}
