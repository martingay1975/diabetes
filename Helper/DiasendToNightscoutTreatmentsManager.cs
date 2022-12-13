using System.Text.Json;
using Helper.Diasend;
using Helper.Nightscout;

namespace Helper
{
	/// <summary>
	/// A delegate which creates a NightscoutClient object
	/// </summary>
	public delegate NightscoutClient CreateNightscoutClientFn();

	public class DiasendToNightscoutTreatmentsManager
	{
		private readonly CreateNightscoutClientFn createNightscoutClientFn;

		public DiasendToNightscoutTreatmentsManager(CreateNightscoutClientFn createNightscoutClientFn)
		{
			this.createNightscoutClientFn = createNightscoutClientFn ?? throw new ArgumentNullException(nameof(createNightscoutClientFn));
		}

		/// <summary>
		/// Goes to Nightscout and queries for all <paramref name="treatmentType"/> and removes their records from the date <paramref name="fromDate"/>
		/// </summary>
		/// <param name="fromDate">Filter from date</param>
		/// <param name="treatmentType">The treatmentType value must match</param>
		public async Task DeleteTreatmentType(DateTime fromDate, string treatmentType)
		{
			var nightscoutClient = this.createNightscoutClientFn();
			var treatments = await nightscoutClient.GetTreatmentsAsync(fromDate, DateTime.UtcNow);
			var comboBolusTreatments = treatments.Where(treatmentDto => treatmentDto.EventType == treatmentType);
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
		public async Task<(DateTime, ErrorInfo?)>
			TransferAsync(string csvPath, DateTime lastTreatmentProcessDateTime, bool ignoreErrors, Action updateProgressFn, Action<int> updateMaxFn)
		{
			var diasendCsvReader = new DiasendCsvReader();
			var allData = diasendCsvReader.Read(csvPath);
			var diasendSortedDataList = allData
				.Where(insulinAdministration => insulinAdministration.DateTimeUtc > lastTreatmentProcessDateTime)
				.OrderBy(insulinAdministration => insulinAdministration.DateTimeUtc).ToList();

			updateMaxFn(diasendSortedDataList.Count);

			// loop around each of the diasend data
			var newLastTreatmentProcessDateTime = lastTreatmentProcessDateTime;
			var nightscoutClient = this.createNightscoutClientFn();

			TreatmentMapper.Reset();

			var boluses = diasendSortedDataList.Where(d => !string.IsNullOrEmpty(d.BasalAmount)).ToList();

			foreach (var insulinAdministration in diasendSortedDataList)
			{
				try
				{
					var treatmentDto = TreatmentMapper.Map(insulinAdministration);
					if (treatmentDto != null)
					{
						var content = new List<TreatmentDto> { { treatmentDto } };
						var val = JsonSerializer.Serialize(content, NightscoutClient.jsonSerializerOptions);

						await nightscoutClient.PostTreatmentsAsync(content);
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
					if (!ignoreErrors)
					{
						Console.WriteLine(ex);
						return (newLastTreatmentProcessDateTime, new ErrorInfo { Exception = ex, InsulinAdministration = insulinAdministration });
					}
				}
				finally
				{
					updateProgressFn();
				}

				// process the last basal - looking at the data the last record is a basal one.


			}

			return (newLastTreatmentProcessDateTime, null);
		}
	}
}
