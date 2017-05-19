using System;
using Foundation;
using PerfTest2Xamarin.Enums;
using PerfTest2Xamarin.ViewSoruce;
using UIKit;

namespace PerfTest2Xamarin
{
	[Register ("SqLiteTableViewController")]
	public class SqLiteTableViewController : UITableViewController
	{
		public SqLiteDisplayType TableQueryType { get; set; }
		public string DbPath { get; set; }

		public SqLiteTableViewController (IntPtr p) : base (p)
		{
		}

		public override void ViewDidLoad ()
		{
			TableView.Source = new SqLiteViewSource (DbPath, TableQueryType);
		}
	}
}