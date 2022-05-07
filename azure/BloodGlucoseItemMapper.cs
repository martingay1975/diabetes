using azure.CosmosDb;
using Helper.Nightscout;

namespace azure
{
    public static class BloodGlucoseItemMapper
    {
        public static BloodGlucoseItem Map(EntryDto entryDto)
        {
            return new BloodGlucoseItem
            {
                id = entryDto.Id,
                mmolL = entryDto.Sgv * 0.0555,
                dateTime = entryDto.SysTime,
                date = new DateTime(entryDto.SysTime.Year, entryDto.SysTime.Month, entryDto.SysTime.Day).Ticks
            };
        }
    }
}