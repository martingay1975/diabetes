using System.Diagnostics;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Helper.Diasend
{
	public class GlookoCsvReader
	{
		public IEnumerable<GlookoBolusDataDto> ReadBolus(string csvpath)
		{
			var output = new Dictionary<string, GlookoBolusDataDto>();
			var csvReaderConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				HasHeaderRecord = false,
			};

			using (var reader = new StreamReader(csvpath))
			using (var csv = new CsvReader(reader, csvReaderConfig))
			{
				reader.ReadLine();
				reader.ReadLine();

				var records = csv.GetRecords<GlookoBolusDataDto>().ToList();

				foreach (var record in records)
				{
					Debug.WriteLine($"Time:{record.Time}  Bolus:{record.BolusVolume}     Carbs:{record.Carbs}");
					if (output.TryGetValue(record.Time, out var insulinAdministration))
					{
						Debug.WriteLine($"Update existing Time:{record.Time}");
						if (!string.IsNullOrWhiteSpace(record.BolusVolume))
						{
							insulinAdministration.BolusVolume = record.BolusVolume;
						}

						if (!string.IsNullOrWhiteSpace(record.Carbs))
						{
							insulinAdministration.Carbs = record.Carbs;
						}
					}
					else
					{
						output.Add(record.Time, record);
					}
				}

				return output.Values;
			}
		}

		public IEnumerable<GlookoBasalDataDto> ReadBasal(string csvpath)
		{
			var output = new Dictionary<string, GlookoBasalDataDto>();
			var csvReaderConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				HasHeaderRecord = false,
			};

			using (var reader = new StreamReader(csvpath))
			using (var csv = new CsvReader(reader, csvReaderConfig))
			{
				reader.ReadLine();
				reader.ReadLine();

				var records = csv.GetRecords<GlookoBasalDataDto>().ToList();

				foreach (var record in records)
				{
					Debug.WriteLine($"Time:{record.Time}  Basal:{record.BasalAmount}");
					if (output.TryGetValue(record.Time, out var insulinAdministration))
					{
						Debug.WriteLine($"Update existing Time:{record.Time}");
						if (!string.IsNullOrWhiteSpace(record.BasalAmount))
						{
							insulinAdministration.BasalAmount = record.BasalAmount;
						}
					}
					else
					{
						output.Add(record.Time, record);
					}
				}

				return output.Values;
			}
		}
	}
}
