using PerfTest2Xamarin.Forms;
using Xamarin.Forms;
#if __ANDROID__
using Android.Runtime;
#else
using Foundation;
#endif

namespace PerfTest2Xamarin
{
	[Preserve (AllMembers = true)]
	public class App : Application
	{
		public App ()
		{
			var mainPage = new MainPage ();
			MainPage = new NavigationPage (mainPage);
		}
	}
}
