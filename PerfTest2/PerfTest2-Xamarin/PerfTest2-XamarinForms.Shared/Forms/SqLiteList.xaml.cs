using PerfTest2Xamarin.Enums;
using PerfTest2Xamarin.ViewModels;
using Xamarin.Forms;
#if __ANDROID__
using Android.Runtime;
#else
using Foundation;
#endif

namespace PerfTest2Xamarin.Forms
{
	[Preserve (AllMembers = true)]
	public partial class SqLiteList : ContentPage
	{
		public SqLiteList ()
		{
			InitializeComponent ();

			if (Device.RuntimePlatform == Device.Android)
			{
				NavigationPage.SetHasNavigationBar (this, false);
			}
		}

		public void SetSqLiteDisplayType (SqLiteDisplayType displayType)
		{
			var viewModel = BindingContext as SqLiteListViewModel;
			if (viewModel != null)
			{
				viewModel.SetDisplayType (displayType);
			}
		}
	}
}