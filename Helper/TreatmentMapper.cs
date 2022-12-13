using Helper.Diasend;
using Helper.Nightscout;

namespace Helper
{
	public static class TreatmentMapper
	{
		private static InsulinAdministration? LatestBasal;

		public static void Reset()
		{
			LatestBasal = null;
		}

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

			double? basalTotal = null;
			if (double.TryParse(insulinAdministration.BasalAmount, out var basal))
			{
				if (LatestBasal == null)
				{
					LatestBasal = insulinAdministration;
					return null;
				}
				basalTotal = basal;
			}

			if (bolusTotal.HasValue || carbsTotal.HasValue)
			{
				return TreatmentDtoFactory.CreateBolus(
					dateTimeUtc: insulinAdministration.DateTimeUtc,
					bolusMmoll: bolusTotal,
					carbsG: carbsTotal);
			}

			if (basalTotal.HasValue)
			{
				var ret = TreatmentDtoFactory.CreateBasal(
										dateTimeUtc: LatestBasal.DateTimeUtc,
										basal: double.Parse(LatestBasal.BasalAmount),
										durationMins: (int)((insulinAdministration.DateTimeUtc - LatestBasal.DateTimeUtc).TotalMinutes)
				);

				LatestBasal = insulinAdministration;
				return ret;
			}

			return null;
		}
	}
}
