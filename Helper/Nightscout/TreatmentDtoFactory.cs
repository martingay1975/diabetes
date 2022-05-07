namespace Helper.Nightscout
{
	public static class TreatmentDtoFactory
	{
		public static TreatmentDto CreateBolus(DateTime dateTime, double bolusMmoll, int? carbsG = null)
		{
			return new TreatmentDto()
			{
				_id = Guid.NewGuid().ToString("N"),
				Created_at = $"{dateTime.ToString("yyyy-MM-dd")}T{dateTime.ToString("HH:mm:ss")}.000Z",
				EventType = "Combo Bolus",
				Insulin = bolusMmoll,
				Carbs = carbsG,
				EnteredBy = "diasend",
				Units = "mmol"
			};
		}
	}
}
