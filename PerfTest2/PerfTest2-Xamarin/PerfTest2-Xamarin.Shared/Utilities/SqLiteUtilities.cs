using System;
using System.Collections.Generic;
using SQLite;
using System.IO;
using System.Linq;

namespace PerfTest2Xamarin.Utilities
{
	public class SqLiteUtilities
	{
		private const int DATABASE_VERSION = 1;
		private const string DATABASE_NAME = "testDB.sql3";
		private const string TABLE_NAME = "testTable";

		private string databasePath;
		private SQLiteConnection database;

		private SQLiteConnection Database => database ?? (database = new SQLiteConnection (Path.Combine (databasePath, DATABASE_NAME)));

		public SqLiteUtilities (string path)
		{
			databasePath = path;
		}

		public void CloseConnection ()
		{
			if (database == null)
			{
				throw new InvalidOperationException ("Connection not open to close");
			}

			database.Close ();
			database = null;
		}

		public void DeleteFile ()
		{
			if (database != null)
			{
				CloseConnection ();
			}

			if (File.Exists (Path.Combine (databasePath, DATABASE_NAME)))
			{
				File.Delete (Path.Combine (databasePath, DATABASE_NAME));
			}
		}

		public void CreateTable ()
		{
			Database.CreateTable<TestTable> ();
		}

		public void AddRecord (string fName, string lName, int i, string m)
		{
			Database.Insert (new TestTable { FirstName = fName, Id = 0, LastName = lName + i, Misc = m });
		}

		public IList<string> GetAllRecords ()
		{
			return Database.Table<TestTable> ()
						   .Select (record => string.Format ("{0} {1}", record.FirstName, record.LastName))
						   .ToList ();
		}

		public IList<string> GetRecordsWith1 ()
		{
			return Database.Table<TestTable> ()
						   .Where (record => record.LastName.Contains ("1"))
						   .Select (record => string.Format ("{0} {1}", record.FirstName, record.LastName))
						   .ToList ();
		}
	}
}