using System.Text.Json.Serialization;

namespace azure.CosmosDb
{
    public class BloodGlucoseItem
    {
        [JsonPropertyName("id")]
        public string id { get; set; }
        [JsonPropertyName("mmoll")]
        public double mmolL { get; set; }
        [JsonPropertyName("datetime")]
        public DateTime dateTime { get; set; }
        [JsonPropertyName("date")]
        public long date { get; set; }
    }
}