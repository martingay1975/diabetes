﻿using CsvHelper;
using System.Globalization;
// mmol/L
// mg/dl
// 1 mg/dL = 0.0555 mmol/L
namespace Helper.Diasend
{
    public class DiasendCsvReader
    {
        public void Read()
        {
            var output = new Dictionary<string, InsulinAdministration>();

            using (var reader = new StreamReader(@"C:\git\diabetes\diasend\insulin.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<InsulinAdministration>().ToList();

                foreach (var record in records)
                {
                    Console.WriteLine($"{record.Time}  {record.BasalAmount}   {record.BolusVolume}     {record.Carbs}");
                    if (output.TryGetValue(record.Time, out var insulinAdministration))
                    {
                        if (record.BasalAmount != null)
                        {
                            insulinAdministration.BasalAmount = record.BasalAmount;
                        }

                        if (record.BolusVolume != null)
                        {
                            insulinAdministration.BolusVolume = record.BolusVolume;
                        }

                        if (record.Carbs != null)
                        {
                            insulinAdministration.Carbs = record.Carbs;
                        }
                    } else
                    {
                        output.Add(record.Time, record);
                    }

                }
                Console.WriteLine($"records {output.Count}");
            }
        }
    }
}