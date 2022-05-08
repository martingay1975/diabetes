using System.Text.Json.Serialization;

namespace azure.CosmosDb
{
    public class BloodGlucoseItem
    {
        [JsonPropertyName("id")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public string id { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		[JsonPropertyName("mmoll")]
        public double mmolL { get; set; }
        [JsonPropertyName("datetime")]
        public DateTime dateTime { get; set; }
        [JsonPropertyName("date")]
        public long date { get; set; }
    }
}
