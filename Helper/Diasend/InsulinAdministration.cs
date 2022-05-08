using System.Globalization;
using CsvHelper.Configuration.Attributes;

namespace Helper.Diasend
{
	public class InsulinAdministration
	{
		[Index(0)]
		public string Time { get; set; }
		[Index(1)]
		[Optional]
		public string BasalAmount { get; set; }
		[Index(3)]
		[Optional]
		public string BolusVolume { get; set; }
		[Index(7)]
		[Optional]
		public string Carbs { get; set; }

		/// <summary>
		/// Parses the local time from the CSV file and creates a UTC DateTime structure.
		/// </summary>
		[Ignore]
		public DateTime DateTimeUtc {
			get
			{
				try
				{
					var dt = DateTime.ParseExact(Time, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
					DateTime res = DateTime.SpecifyKind(dt, DateTimeKind.Local);
					var ret = res.ToUniversalTime();
					return ret;
				}
				catch (Exception ex)
				{
					throw;
				}
			}
		}
    }
}
