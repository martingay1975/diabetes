namespace Helper.Nightscout
{
	public static class TreatmentDtoFactory
	{
		public static TreatmentDto CreateBolus(DateTime dateTimeUtc, double? bolusMmoll, int? carbsG = null)
		{
			return new TreatmentDto()
			{
				_id = Guid.NewGuid().ToString("N").Substring(0, 24),
				EventType = "",
				EnteredBy = "diasend",
				Insulin = bolusMmoll,
				Units = "mmol",
				Created_at = $"{dateTimeUtc.ToString("yyyy-MM-dd")}T{dateTimeUtc.ToString("HH:mm:ss")}.000Z",
				Carbs = carbsG,
			};
		}

		public static TreatmentDto CreateBasal(DateTime dateTimeUtc, double? basal, int? durationMins)
		{
			return new TreatmentDto
			{
				_id = Guid.NewGuid().ToString("N"),
				Created_at = $"{dateTimeUtc.ToString("yyyy-MM-dd")}T{dateTimeUtc.ToString("HH:mm:ss")}.000Z",
				EventType = "Temp Basal",
				Duration = durationMins,
				EnteredBy = "diasend",
				Units = "mmol",
				Absolute = basal,
				GlucoseType = "Finger"
				// eventTime = "Sat Oct 29 2022 00:00:00 GMT + 0100(British Summer Time)"
			};
		}
	}
}
