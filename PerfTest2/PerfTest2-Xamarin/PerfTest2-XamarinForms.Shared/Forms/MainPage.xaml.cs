using System;
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
	public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
			InitializeComponent ();
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			var viewModel = BindingContext as MainMenuViewModel;
			if (viewModel != null)
			{
				viewModel.DisplayMessage += ViewModelOnDisplayMessage;
				viewModel.NavigatePage += ViewModelOnNavigatePage;
			}
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();

			var viewModel = BindingContext as MainMenuViewModel;
			if (viewModel != null)
			{
				viewModel.DisplayMessage -= ViewModelOnDisplayMessage;
				viewModel.NavigatePage -= ViewModelOnNavigatePage;
			}
		}

		private void ViewModelOnDisplayMessage (object sender, DisplayMessageEventArgs eventArgs)
		{
			DisplayAlert (eventArgs.Title, eventArgs.Message, "OK", "Cancel");
		}

		private void ViewModelOnNavigatePage (object sender, NavigatePageEventArgs e)
		{
			Page targetPage;
			switch (e.Target)
			{
			case NavigationTarget.SqLiteDisplayAll:
				targetPage = new SqLiteList ();
				((SqLiteList)targetPage).SetSqLiteDisplayType (SqLiteDisplayType.ShowAll);
				break;
			case NavigationTarget.SqLiteDisplayWhere:
				targetPage = new SqLiteList ();
				((SqLiteList)targetPage).SetSqLiteDisplayType (SqLiteDisplayType.ShowContaining1);
				break;
			case NavigationTarget.FileList:
				targetPage = new FileList ();
				break;
			default:
				throw new NotImplementedException ("Not a valid navigation target.");
			}

			Navigation.PushAsync (targetPage, true);
		}
	}
}
