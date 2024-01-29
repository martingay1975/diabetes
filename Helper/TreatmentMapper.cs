using Helper.Diasend;
using Helper.Nightscout;

namespace Helper
{
	public static class TreatmentMapper
	{
		private static GlookoBasalDataDto? LatestBasal;

		public static void Reset()
		{
			LatestBasal = null;
		}

		public static TreatmentDto? MapBasal(GlookoBasalDataDto insulinAdministration)
		{
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

		public static TreatmentDto? MapBolus(GlookoBolusDataDto insulinAdministration)
		{
			double? bolusTotal = null;
			if (double.TryParse(insulinAdministration.BolusVolume, out var bolus))
			{
				bolusTotal = bolus;
			}

			int? carbsTotal = null;
			if (double.TryParse(insulinAdministration.Carbs, out var carbsG))
			{
				carbsTotal = (int)carbsG;
			}

			if (bolusTotal.HasValue || carbsTotal.HasValue)
			{
				return TreatmentDtoFactory.CreateBolus(
					dateTimeUtc: insulinAdministration.DateTimeUtc,
					bolusMmoll: bolusTotal,
					carbsG: carbsTotal);
			}

			return null;
		}
	}
}
