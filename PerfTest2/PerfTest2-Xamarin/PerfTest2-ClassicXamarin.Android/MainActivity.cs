
using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using PerfTest2Xamarin.Adapters;
using PerfTest2Xamarin.Enums;
using PerfTest2Xamarin.Fragments;
using PerfTest2Xamarin.Utilities;

namespace PerfTest2Xamarin
{
	[Activity (Label = "PerfTest2_ClassicXamarin.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity, AdapterView.IOnItemClickListener
	{
		Fragment currentFragment;
		private string directory;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.activity_main);

			currentFragment = new MainMenuFragment ();
			FragmentTransaction trans = this.FragmentManager.BeginTransaction ();
			trans.Add (Resource.Id.main_area, currentFragment);
			trans.AddToBackStack (null);
			trans.Commit ();
		}

		protected override void OnResume ()
		{
			base.OnResume ();

			directory = this.GetExternalFilesDir (Android.OS.Environment.DirectoryDownloads).Path + "/";
		}

		public void OnItemClick (AdapterView parent, View view, int position, long id)
		{
			var lstMainMenu = (ListView)this.FindViewById (Resource.Id.lstMainMenu);

			switch (position)
			{
			case MainMenuAdapter.CLEAN_UP_TEST:
				CleanUp ();
				break;
			case MainMenuAdapter.ADD_SQL_RECORDS:
				AddRecords ();
				break;
			case MainMenuAdapter.DISPLAY_ALL_RECORDS:
				ShowAllRecords ();
				break;
			case MainMenuAdapter.DISPLAY_RECORDS_WITH_WHERE:
				ShowRecordsWith ();
				break;
			case MainMenuAdapter.SAVE_LARGE_FILE:
				SaveLargeFile ();
				break;
			case MainMenuAdapter.DISPLAY_LARGE_FILE:
				LoadAndDisplayFile ();
				break;
			}
		}

		private void CleanUp ()
		{
			var sqlUtilities = new SqLiteUtilitiesAlt (this);
			var alertDialog = new AlertDialog.Builder (this);
			try
			{
				sqlUtilities.CreateTable ();
				sqlUtilities.CloseConnection ();

				using (var fUtilities = new FileUtilities (directory))
				{
					fUtilities.DeleteFile ();
					fUtilities.CreateFile ();
					fUtilities.CloseFile ();
				}

				alertDialog.SetMessage ("Completed test setup");
				alertDialog.SetTitle ("Cleanup and Prepare for Tests Successful");
			}
			catch (Exception ex)
			{
				alertDialog.SetMessage ("An error has occurred: " + ex.Message);
				alertDialog.SetTitle ("Error");
			}
			finally
			{
				alertDialog.SetPositiveButton ("OK", (sender, args) => { });
				alertDialog.SetCancelable (true);
				alertDialog.Create ().Show ();
			}
		}

		private void AddRecords ()
		{
			var utilities = new SqLiteUtilitiesAlt (this);
			var alertDialog = new AlertDialog.Builder (this);
			try
			{
				for (int i = 0; i <= 999; i++)
				{
					utilities.AddRecord ("test", "person", i, "12345678901234567890123456789012345678901234567890");
				}
				utilities.CloseConnection ();

				alertDialog.SetMessage ("All records written to database");
				alertDialog.SetTitle ("Success");
			}
			catch (Exception ex)
			{
				alertDialog.SetMessage ("An error has occurred adding records: " + ex.Message);
				alertDialog.SetTitle ("Error");
			}
			finally
			{
				alertDialog.SetPositiveButton ("OK", (sender, args) => { });
				alertDialog.SetCancelable (true);
				alertDialog.Create ().Show ();
			}
		}

		private void ShowAllRecords ()
		{
			var bundle = new Bundle ();
			bundle.PutInt ("displayType", (int)SqLiteDisplayType.ShowAll);
			var fragment = new SqLiteTableFragment (directory) {
				Arguments = bundle
			};
			var fragmentTransaction = FragmentManager.BeginTransaction ();
			fragmentTransaction.Replace (Resource.Id.main_area, fragment);
			fragmentTransaction.AddToBackStack (null);
			fragmentTransaction.Commit ();
		}

		private void ShowRecordsWith ()
		{
			var bundle = new Bundle ();
			bundle.PutInt ("displayType", (int)SqLiteDisplayType.ShowContaining1);
			var fragment = new SqLiteTableFragment (directory) {
				Arguments = bundle
			};
			var fragmentTransaction = FragmentManager.BeginTransaction ();
			fragmentTransaction.Replace (Resource.Id.main_area, fragment);
			fragmentTransaction.AddToBackStack (null);
			fragmentTransaction.Commit ();
		}

		private void SaveLargeFile ()
		{
			var alertDialog = new AlertDialog.Builder (this);
			try
			{
				using (var utilities = new FileUtilities (directory))
				{
					for (int i = 0; i <= 999; i++)
					{
						utilities.WriteLineToFile ("Writing line to file at index: " + i);
					}
					utilities.CloseFile ();
				}

				alertDialog.SetMessage ("All lines written to file");
				alertDialog.SetTitle ("Success");
			}
			catch (Exception ex)
			{
				alertDialog.SetMessage ("An error has occurred adding lines to file: " + ex.Message);
				alertDialog.SetTitle ("Error");
			}
			finally
			{
				alertDialog.SetPositiveButton ("OK", (sender, args) => { });
				alertDialog.SetCancelable (true);
				alertDialog.Create ().Show ();
			}
		}

		private void LoadAndDisplayFile ()
		{
			var fragmentTransaction = FragmentManager.BeginTransaction ();
			var fragment = new DisplayTextFileFragment (directory);
			fragmentTransaction.Replace (Resource.Id.main_area, fragment);
			fragmentTransaction.AddToBackStack (null);
			fragmentTransaction.Commit ();
		}
	}
}

