using Helper.Diasend;
using Helper.Nightscout;

namespace Helper
{
	public static class TreatmentMapper
	{
		public static TreatmentDto? Map(InsulinAdministration insulinAdministration)
		{
			double? bolusTotal = null;
			if (double.TryParse(insulinAdministration.BolusVolume, out var bolus))
			{
				bolusTotal = bolus;
			}

			int? carbsTotal = null;
			if (int.TryParse(insulinAdministration.Carbs, out var carbsG))
			{
				carbsTotal = carbsG;
			}

			if (bolusTotal == null && carbsTotal == null)
			{
				return null;
			}

			return TreatmentDtoFactory.CreateBolus(
				dateTimeUtc: insulinAdministration.DateTimeUtc,
				bolusMmoll: bolusTotal,
				carbsG: carbsTotal);
		}
	}
}
