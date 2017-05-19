using SQLite;

namespace PerfTest2Xamarin.Utilities
{
	[Table ("TestTable")]
	public class TestTable
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Misc { get; set; }
	}
}
