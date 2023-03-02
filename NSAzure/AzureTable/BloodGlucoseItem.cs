using Azure;
using Azure.Data.Tables;

namespace NSAzure.AzureTable
{
	public class BloodGlucoseItem : ITableEntity
	{
		public string? Id { get; init; }
		public double MmolL { get; init; }
		public DateTime DateTime { get; init; }
		public long Date { get; init; }
		public string PartitionKey { get; set; } = default!;
		public string RowKey { get; set; } = default!;
		public DateTimeOffset? Timestamp { get; set; } = default!;
		public ETag ETag { get; set; } = default!;
	}
}
