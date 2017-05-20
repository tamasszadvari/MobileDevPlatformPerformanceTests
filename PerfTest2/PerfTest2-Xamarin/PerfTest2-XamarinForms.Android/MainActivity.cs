using Android.App;
using Android.OS;
using Xamarin.Forms.Platform.Android;

namespace PerfTest2Xamarin
{
	[Activity (Label = "PerfTest2_XamarinForms.Android", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/AppTheme")]
	public class MainActivity : FormsAppCompatActivity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			Xamarin.Forms.Forms.Init (this, savedInstanceState);

			LoadApplication (new App ());
		}
	}
}

