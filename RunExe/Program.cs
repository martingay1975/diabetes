// See https://aka.ms/new-console-template for more information
using Helper.Nightscout;
using System.Text.Json;

Console.WriteLine("Hello, World!");
const string API_SECRET_SHA1_HASH = "1fbf2b03609cc5c3cb08f9f3d6d2f907434f2dc2";

var nightscoutClient = new NightscoutClient(API_SECRET_SHA1_HASH, @"https://mgns.herokuapp.com");
var dto = TreatmentDtoFactory.CreateBolus(DateTime.Parse("2022-05-01 12:01:01"), 8.2, 99);
dto._id = null;

var results = await nightscoutClient.PutTreatmentsAsync(new List<TreatmentDto> { { dto } });
var json = JsonSerializer.Serialize<List<TreatmentDto>>(results.ToList());
Console.WriteLine(json);

File.WriteAllText(@"c:\git\treatments.json", json);
