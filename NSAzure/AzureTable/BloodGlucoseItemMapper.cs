using Helper.Nightscout;

namespace NSAzure.AzureTable
{
	public static class BloodGlucoseItemMapper
	{
		public static BloodGlucoseItem Map(EntryDto entryDto)
		{
			return new BloodGlucoseItem
			{
				ETag = new Azure.ETag(),
				PartitionKey = entryDto.SysTime.Year.ToString(),
				RowKey = entryDto.SysTime.Ticks.ToString(),
				Timestamp = entryDto.SysTime,
				Id = entryDto.Id,
				MmolL = entryDto.Sgv * 0.0555,
				DateTime = entryDto.SysTime,
				Date = new DateTime(entryDto.SysTime.Year, entryDto.SysTime.Month, entryDto.SysTime.Day).Ticks
			};
		}
	}
}
