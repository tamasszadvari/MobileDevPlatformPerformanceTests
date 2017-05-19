using System;
using Foundation;
using PerfTest2Xamarin.ViewSoruce;
using UIKit;

namespace PerfTest2Xamarin
{
	[Register ("FileViewController")]
	public class FileViewController : UITableViewController
	{
		public string DbPath { get; set; }

		public FileViewController (IntPtr p) : base (p)
		{
		}

		public override void ViewDidLoad ()
		{
			TableView.Source = new FileViewSource (DbPath);
		}
	}
}