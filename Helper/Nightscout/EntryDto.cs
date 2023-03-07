using System.Text.Json.Serialization;

namespace Helper.Nightscout
{
	public class EntryDto
	{
		[JsonPropertyName("_id")]
		public string Id { get; set; }
		public int Sgv { get; set; }
		public DateTime SysTime { get; set; }
		public double Delta { get; set; }
		// public string Device { get; set; }
		// public object Date { get; set; }
		// public DateTime DateString { get; set; }
		public string Direction { get; set; }
		// public string Type { get; set; }
		// public int Filtered { get; set; }
		// public int Unfiltered { get; set; }
		// public int Rssi { get; set; }
		// public int Noise { get; set; }
		// public int UtcOffset { get; set; }
	}
}
