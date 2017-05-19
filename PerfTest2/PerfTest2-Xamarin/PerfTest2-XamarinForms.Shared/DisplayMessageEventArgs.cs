using System;
#if __ANDROID__
using Android.Runtime;
#else
using Foundation;
#endif

namespace PerfTest2Xamarin
{
	[Serializable]
	[Preserve (AllMembers = true)]
	public delegate void DisplayMessageEventHandler (object sender, DisplayMessageEventArgs e);

	public class DisplayMessageEventArgs : EventArgs
	{
		private readonly string title = string.Empty;
		private readonly string message = string.Empty;

		public DisplayMessageEventArgs (string title, string message)
		{
			this.title = title;
			this.message = message;
		}

		public string Title => title;

		public string Message => message;
	}
}