using System.Globalization;
using CsvHelper.Configuration.Attributes;

namespace Helper.Diasend
{
	public class GlookoData
	{
		public DateTime Time { get; set; }

		public decimal? BasalAmount { get; set; }

		public int? Carbs { get; set; }

		public decimal? BolusVolume { get; set; }
	}

	public abstract class GlookoBaseDataDto
	{
		[Index(0)]
		public string Time { get; set; }

		/// <summary>
		/// Parses the local time from the CSV file and creates a UTC DateTime structure.
		/// </summary>
		[Ignore]
		public DateTime DateTimeUtc
		{
			get
			{
				try
				{
					var dt = DateTime.ParseExact(Time, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
					DateTime res = DateTime.SpecifyKind(dt, DateTimeKind.Local);
					//var ret = res.ToUniversalTime();
					return res;
				}
				catch (Exception)
				{
					throw;
				}
			}
		}
	}

	public class GlookoBasalDataDto : GlookoBaseDataDto
	{

		[Index(4)]
		public string BasalAmount { get; set; }
	}

	public class GlookoBolusDataDto : GlookoBaseDataDto
	{
		[Index(3)]
		[Optional]
		public string Carbs { get; set; }

		[Index(5)]
		[Optional]
		public string BolusVolume { get; set; }
	}
}
