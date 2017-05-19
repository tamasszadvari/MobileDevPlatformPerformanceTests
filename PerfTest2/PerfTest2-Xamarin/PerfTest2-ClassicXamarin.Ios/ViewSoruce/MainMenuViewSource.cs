using System;
using Foundation;
using UIKit;

namespace PerfTest2Xamarin.ViewSoruce
{
	public delegate void RowIsSelectedHandler (UITableView tableView, NSIndexPath indexPath);

	public class MainMenuViewSource : UITableViewSource
	{
		public event RowIsSelectedHandler RowIsSelected;

		private readonly string[] menuItems =
		{
			"Clean up and Prepare for Tests",
			"Add 1,000 records to SqLite",
			"Display all records",
			"Display all records that contain 1",
			"Save large file",
			"Load and display large file"
		};

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return 6;
		}

		public override nint NumberOfSections (UITableView tableView)
		{
			return 1;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			RowIsSelected?.Invoke (tableView, indexPath);
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			const string simpleTableViewIdentifier = "SimpleTableItem";

			// if there are no cells to reuse, create a new one
			var cell = tableView.DequeueReusableCell (simpleTableViewIdentifier) ?? new UITableViewCell (UITableViewCellStyle.Default, simpleTableViewIdentifier);

			cell.TextLabel.Text = menuItems[indexPath.Row];

			return cell;
		}
	}
}