using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PerfTest2Xamarin.Utilities;

namespace PerfTest2Xamarin.Adapters
{
	public class FileTableAdapter : BaseAdapter<string>
	{
		private Context context;
		private string directory;
		private IList<string> records;

		public FileTableAdapter (Context context, string directory)
		{
			this.context = context;
			this.directory = directory;
			LoadRecords ();
		}

		private void LoadRecords ()
		{
			var utilities = new FileUtilities (this.directory);
			try
			{
				records = utilities.ReadFileContents ();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine (ex.Message);
			}
		}

		public override string this[int position] => records[position];

		public override int Count => records.Count;

		public override long GetItemId (int position) => position;

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			if (convertView == null)
			{
				var inflater = (LayoutInflater)context.GetSystemService (Context.LayoutInflaterService);
				convertView = inflater.Inflate (Android.Resource.Layout.SimpleListItem1, null);
			}

			var txtItem = (TextView)convertView.FindViewById (Android.Resource.Id.Text1);
			txtItem.Text = records[position];

			return convertView;
		}
	}
}