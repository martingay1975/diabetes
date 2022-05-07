// See https://aka.ms/new-console-template for more information
using Helper.Nightscout;
using System.Text.Json;

Console.WriteLine("Hello, World!");


var nightscoutClient = new NightscoutClient();
var treatments = await nightscoutClient.GetTreatmentsAsync(new DateTime(2022, 1, 1), DateTime.UtcNow);

var results = treatments.Where(treatment => treatment.EventType.Contains("Combo"));

Console.WriteLine(JsonSerializer.Serialize<List<TreatmentDto>>(results.ToList()));

