using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System.Globalization;

namespace csv_reader
{
    public class InsulinAdministration
    {
        [Index(0)]
        public string Time {get; set;}
        [Index(1)]
        [Optional]
        public string BasalAmount { get; set; }
        [Index(3)]
        [Optional]
        public string BolusVolume { get; set; }
        [Index(7)]
        [Optional]
        public string Carbs { get; set; }
    }


    public class Class1
    {
        public void Read()
        {
            using (var reader = new StreamReader(@"C:\git\diabetes\diasend\insulin.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<InsulinAdministration>().ToList();

                foreach (var record in records)
                {
                    Console.WriteLine($"{record.Time}  {record.BasalAmount}   {record.BolusVolume}     {record.Carbs}");
                }
                Console.WriteLine($"records {records.Count}");
            }
        }
    }
}