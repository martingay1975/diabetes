using CsvHelper;
using System.Globalization;

namespace Helper.Diasend
{
    public class DiasendCsvReader
    {
        public IEnumerable<InsulinAdministration> Read(string csvpath)
        {
            var output = new Dictionary<string, InsulinAdministration>();

            using (var reader = new StreamReader(csvpath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<InsulinAdministration>().ToList();

                foreach (var record in records)
                {
                    Console.WriteLine($"Time:{record.Time}  Basal:{record.BasalAmount}   Bolus:{record.BolusVolume}     Carbs:{record.Carbs}");
                    if (output.TryGetValue(record.Time, out var insulinAdministration))
                    {
						Console.WriteLine($"Update existing Time:{record.Time}");
						if (!string.IsNullOrWhiteSpace(record.BasalAmount))
                        {
                            insulinAdministration.BasalAmount = record.BasalAmount;
                        }

                        if (!string.IsNullOrWhiteSpace(record.BolusVolume))
                        {
                            insulinAdministration.BolusVolume = record.BolusVolume;
                        }

                        if (!string.IsNullOrWhiteSpace(record.Carbs))
                        {
                            insulinAdministration.Carbs = record.Carbs;
                        }
                    } else
                    {
                        output.Add(record.Time, record);
                    }
                }

				return output.Values;
			}
        }
    }
}
