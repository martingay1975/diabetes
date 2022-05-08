using Helper.Diasend;
using Helper.Nightscout;

namespace Helper
{
	public delegate NightscoutClient CreateNightscoutClientFn();

	public class DiasendToNightscoutTreatmentsManager
	{
		private readonly CreateNightscoutClientFn createNightscoutClientFn;

		public DiasendToNightscoutTreatmentsManager(CreateNightscoutClientFn createNightscoutClientFn)
		{
			this.createNightscoutClientFn = createNightscoutClientFn ?? throw new ArgumentNullException(nameof(createNightscoutClientFn));
		}


		public async Task DeleteComboBolus(DateTime fromDate)
		{
			var nightscoutClient = this.createNightscoutClientFn();
			var treatments = await nightscoutClient.GetTreatmentsAsync(fromDate, DateTime.UtcNow);
			var comboBolusTreatments = treatments.Where(treatmentDto => treatmentDto.EventType == "Combo Bolus");
			foreach (var treatment in comboBolusTreatments)
			{
				await nightscoutClient.DeleteTreatmentAsync(treatment._id);
				Console.WriteLine($"{treatment.Created_at} deleted");
			}
		}

		/// <summary>
		/// Goes off to the Diasend's "Unsulin Use And Carbs" CSV and uploads the data to Nightscout (Treatments)
		/// </summary>
		/// <param name="csvPath">The path of Diasend's "Unsulin Use And Carbs" CSV file</param>
		/// <param name="lastTreatmentProcessDateTime">From when we are interested in querying and uploading data</param>
		/// <returns>The new lastTreatmentProcessDateTime, so it can be used in subsequent calls</returns>
		public async Task<DateTime> TransferAsync(string csvPath, DateTime lastTreatmentProcessDateTime)
		{

			var diasendCsvReader = new DiasendCsvReader();
			var allData = diasendCsvReader.Read(csvPath);
			var diasendSortedDataList = allData
				.Where(insulinAdministration => lastTreatmentProcessDateTime < insulinAdministration.DateTimeUtc)
				.OrderBy(insulinAdministration => insulinAdministration.DateTimeUtc).ToList();

			// loop around each of the diasend data
			var newLastTreatmentProcessDateTime = lastTreatmentProcessDateTime;
			var nightscoutClient = this.createNightscoutClientFn();

			foreach (var insulinAdministration in diasendSortedDataList)
			{
				try
				{
					var treatmentDto = TreatmentMapper.Map(insulinAdministration);
					if (treatmentDto != null)
					{
						await nightscoutClient.PostTreatmentsAsync(new List<TreatmentDto> { { treatmentDto } });
						Console.WriteLine($"{insulinAdministration.DateTimeUtc} added");
					}
					else
					{
						Console.WriteLine($"{insulinAdministration.DateTimeUtc} ignored");
					}

					newLastTreatmentProcessDateTime = insulinAdministration.DateTimeUtc;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
					return newLastTreatmentProcessDateTime;
				}
			}

			return newLastTreatmentProcessDateTime;
		}
	}
}
