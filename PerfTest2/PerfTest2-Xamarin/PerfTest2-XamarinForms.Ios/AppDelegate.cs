using Foundation;
using UIKit;
using Xamarin.Forms;

namespace PerfTest2Xamarin
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;

		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			Xamarin.Forms.Forms.Init ();

			window = new UIWindow (UIScreen.MainScreen.Bounds);

			var app = new App ();

			window.RootViewController = app.MainPage.CreateViewController ();
			window.MakeKeyAndVisible ();

			return true;
		}
	}
}