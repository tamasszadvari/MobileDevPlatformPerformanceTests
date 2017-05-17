using System;
using System.Collections.Generic;
using System.IO;

namespace PerfTest2Xamarin.Utilities
{
	public class FileUtilities : IDisposable
	{
		private string filePath;
		private StreamWriter streamWriter;

		private StreamWriter Writer => streamWriter ?? (streamWriter = new StreamWriter (filePath));

		public FileUtilities (string filePath)
		{
			filePath = Path.Combine (filePath, "testFile.txt");
		}

		public void CloseFile ()
		{
			if (streamWriter != null)
			{
				streamWriter.Close ();
				streamWriter.Dispose ();
				streamWriter = null;
			}
		}

		public void DeleteFile ()
		{
			if (File.Exists (filePath))
			{
				File.Delete (filePath);
			}
		}

		public void CreateFile ()
		{
			if (!File.Exists (filePath))
			{
				using (var stream = File.Create (filePath)) { }
			}
		}

		public void WriteLineToFile (String line)
		{
			if (!File.Exists (filePath))
				CreateFile ();

			Writer.WriteLine (line);
		}

		public IList<string> ReadFileContents ()
		{
			if (!File.Exists (filePath))
				return new List<string> ();

			using (var streamReader = new StreamReader (filePath))
			{
				var returnValue = new List<String> ();

				while (!streamReader.EndOfStream)
				{
					var line = streamReader.ReadLine ();
					if (line != null)
					{
						returnValue.Add (line);
					}
				}

				return returnValue;
			}
		}

		public void Dispose ()
		{
			CloseFile ();
		}
	}
}
