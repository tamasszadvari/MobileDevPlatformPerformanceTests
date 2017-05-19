using System;
using UIKit;
using Foundation;
using PerfTest2Xamarin.Enums;
using PerfTest2Xamarin.ViewSoruce;
using PerfTest2Xamarin.Utilities;

namespace PerfTest2Xamarin
{
	[Register ("MainViewController")]
	public class MainViewController : UITableViewController
	{
		private const int menuCleanUp = 0;
		private const int menuAddRecords = 1;
		private const int menuDisplayAll = 2;
		private const int menuDisplayWithWhere = 3;
		private const int menuSaveLargeFile = 4;
		private const int menuLoadAndDisplayFile = 5;

		private SqLiteDisplayType navigationQueryType;
		private string dbPath;

		public MainViewController ()
		{
		}

		public MainViewController (IntPtr p) : base (p)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();

			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			dbPath = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);

			var source = new MainMenuViewSource ();
			source.RowIsSelected += RowIsSelected;
			TableView.Source = source;
		}

		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue (segue, sender);

			if (segue.Identifier == "squToSqLiteTableView")
			{
				var destinationViewController = (SqLiteTableViewController)segue.DestinationViewController;
				destinationViewController.TableQueryType = navigationQueryType;
				destinationViewController.DbPath = dbPath;
			}
			else if (segue.Identifier == "sguToFileTableView")
			{
				var destinationViewController = (FileViewController)segue.DestinationViewController;
				destinationViewController.DbPath = dbPath;
			}
		}

		private void RowIsSelected (UITableView tableView, NSIndexPath indexPath)
		{
			switch (indexPath.Row)
			{
			case menuCleanUp:
				CleanUp ();
				break;
			case menuAddRecords:
				AddRecords ();
				break;
			case menuDisplayAll:
				ShowAllRecords ();
				break;
			case menuDisplayWithWhere:
				ShowRecordsWith ();
				break;
			case menuSaveLargeFile:
				SaveLargeFile ();
				break;
			case menuLoadAndDisplayFile:
				LoadAndDisplayFile ();
				break;
			}
		}

		private void CleanUp ()
		{
			var sqlUtilities = new SqLiteUtilities (dbPath);
			var alert = new UIAlertView ();
			try
			{
				sqlUtilities.DeleteFile ();
				sqlUtilities.CreateTable ();
				sqlUtilities.CloseConnection ();

				using (var fUtilities = new FileUtilities (dbPath))
				{
					fUtilities.DeleteFile ();
					fUtilities.CreateFile ();
					fUtilities.CloseFile ();
				}

				alert.Title = "Cleanup and Prepare for Tests Successful";
				alert.Message = string.Format ("Completed test setup");
			}
			catch (Exception ex)
			{
				alert.Title = "Error";
				alert.Message = string.Format ("An error has occurred: " + ex.Message);
			}
			finally
			{
				alert.Delegate = new UIAlertViewDelegate ();
				alert.AddButton ("OK");
				alert.Show ();
			}
		}

		private void AddRecords ()
		{
			var utilities = new SqLiteUtilities (dbPath);
			var alert = new UIAlertView ();
			try
			{
				for (int i = 0; i <= 999; i++)
				{
					utilities.AddRecord ("test", "person", i, "12345678901234567890123456789012345678901234567890");
				}
				utilities.CloseConnection ();

				alert.Title = "Success";
				alert.Message = string.Format ("All records written to database");
			}
			catch (Exception ex)
			{
				alert.Title = "Error";
				alert.Message = string.Format ("An error has occurred adding records: " + ex.Message);
			}
			finally
			{
				alert.Delegate = new UIAlertViewDelegate ();
				alert.AddButton ("OK");
				alert.Show ();
			}
		}

		private void ShowAllRecords ()
		{
			navigationQueryType = SqLiteDisplayType.ShowAll;
			PerformSegue ("squToSqLiteTableView", this);
		}

		private void ShowRecordsWith ()
		{
			navigationQueryType = SqLiteDisplayType.ShowContaining1;
			PerformSegue ("squToSqLiteTableView", this);
		}

		private void SaveLargeFile ()
		{
			var alert = new UIAlertView ();
			try
			{
				using (var utilities = new FileUtilities (dbPath))
				{
					for (int i = 0; i <= 999; i++)
					{
						utilities.WriteLineToFile ("Writing line to file at index: " + i);
					}
					utilities.CloseFile ();
				}

				alert.Title = "Success";
				alert.Message = string.Format ("All lines written to file");
			}
			catch (Exception ex)
			{
				alert.Title = "Error";
				alert.Message = string.Format ("An error has occurred adding lines to file: " + ex.Message);
			}
			finally
			{
				alert.Delegate = new UIAlertViewDelegate ();
				alert.AddButton ("OK");
				alert.Show ();
			}
		}

		private void LoadAndDisplayFile ()
		{
			PerformSegue ("sguToFileTableView", this);
		}
	}
}