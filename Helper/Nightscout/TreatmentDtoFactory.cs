namespace Helper.Nightscout
{
	public static class TreatmentDtoFactory
	{
		public static TreatmentDto CreateBolus(DateTime dateTimeUtc, double? bolusMmoll, int? carbsG = null)
		{
			return new TreatmentDto()
			{
				_id = Guid.NewGuid().ToString("N"),
				Created_at = $"{dateTimeUtc.ToString("yyyy-MM-dd")}T{dateTimeUtc.ToString("HH:mm:ss")}.000Z",
				EventType = "Combo Bolus",
				Insulin = bolusMmoll,
				Carbs = carbsG,
				EnteredBy = "diasend",
				Units = "mmol"
			};
		}
	}
}
