using System.Diagnostics;
using System.Text.Json;
using Helper.Diasend;
using Helper.Nightscout;

namespace Helper
{
	/// <summary>
	/// A delegate which creates a NightscoutClient object
	/// </summary>
	public delegate NightscoutClient CreateNightscoutClientFn();

	public class GlookoToNightscoutTreatmentsManager
	{
		private readonly CreateNightscoutClientFn createNightscoutClientFn;

		public GlookoToNightscoutTreatmentsManager(CreateNightscoutClientFn createNightscoutClientFn)
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
				Debug.WriteLine($"{treatment.Created_at} deleted");
			}
		}

		/// <summary>
		/// Goes off to the Glooko's "Bolus" CSV and uploads the data to Nightscout (Treatments)
		/// </summary>
		/// <param name="csvPath">The path of Diasend's "Unsulin Use And Carbs" CSV file</param>
		/// <param name="lastTreatmentProcessDateTime">From when we are interested in querying and uploading data</param>
		/// <returns>The new lastTreatmentProcessDateTime, so it can be used in subsequent calls</returns>
		public async Task<(DateTime, ErrorInfo?)>
			TransferBolusAsync(string csvPath, DateTime lastTreatmentProcessDateTime, bool ignoreErrors, Action updateProgressFn, Action<int> updateMaxFn)
		{
			var glookoCsvReader = new GlookoCsvReader();
			var allData = glookoCsvReader.ReadBolus(csvPath);
			var gookoBolusSortedDataList = allData
				.Where(insulinAdministration => insulinAdministration.DateTimeUtc > lastTreatmentProcessDateTime)
				.OrderBy(insulinAdministration => insulinAdministration.DateTimeUtc).ToList();

			updateMaxFn(gookoBolusSortedDataList.Count);

			// loop around each of the glooko data
			var newLastTreatmentProcessDateTime = lastTreatmentProcessDateTime;
			var nightscoutClient = this.createNightscoutClientFn();

			TreatmentMapper.Reset();

			foreach (var insulinAdministration in gookoBolusSortedDataList)
			{
				try
				{
					var treatmentDto = TreatmentMapper.MapBolus(insulinAdministration);
					if (treatmentDto != null)
					{
						var content = new List<TreatmentDto> { { treatmentDto } };
						var val = JsonSerializer.Serialize(content, NightscoutClient.jsonSerializerOptions);

						await nightscoutClient.PostTreatmentsAsync(content);
						Debug.WriteLine($"{insulinAdministration.DateTimeUtc} bolus added");
					}
					else
					{
						Debug.WriteLine($"{insulinAdministration.DateTimeUtc} bolus ignored");
					}

					newLastTreatmentProcessDateTime = insulinAdministration.DateTimeUtc;
				}
				catch (Exception ex)
				{
					if (!ignoreErrors)
					{
						Debug.WriteLine(ex);
						return (newLastTreatmentProcessDateTime, new ErrorInfo { Exception = ex, GlookoBaseDataDto = insulinAdministration });
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




		/// <summary>
		/// Goes off to the Glooko's "Bolus" CSV and uploads the data to Nightscout (Treatments)
		/// </summary>
		/// <param name="csvPath">The path of Diasend's "Unsulin Use And Carbs" CSV file</param>
		/// <param name="lastTreatmentProcessDateTime">From when we are interested in querying and uploading data</param>
		/// <returns>The new lastTreatmentProcessDateTime, so it can be used in subsequent calls</returns>
		public async Task<(DateTime, ErrorInfo?)>
			TransferBasalAsync(string csvPath, DateTime lastTreatmentProcessDateTime, bool ignoreErrors, Action updateProgressFn, Action<int> updateMaxFn)
		{
			var glookoCsvReader = new GlookoCsvReader();
			var allData = glookoCsvReader.ReadBasal(csvPath);
			var gookoBasalSortedDataList = allData
				.Where(insulinAdministration => insulinAdministration.DateTimeUtc > lastTreatmentProcessDateTime)
				.OrderBy(insulinAdministration => insulinAdministration.DateTimeUtc).ToList();

			updateMaxFn(gookoBasalSortedDataList.Count);

			// loop around each of the glooko data
			var newLastTreatmentProcessDateTime = lastTreatmentProcessDateTime;
			var nightscoutClient = this.createNightscoutClientFn();

			TreatmentMapper.Reset();

			foreach (var insulinAdministration in gookoBasalSortedDataList)
			{
				try
				{
					var treatmentDto = TreatmentMapper.MapBasal(insulinAdministration);
					if (treatmentDto != null)
					{
						var content = new List<TreatmentDto> { { treatmentDto } };
						var val = JsonSerializer.Serialize(content, NightscoutClient.jsonSerializerOptions);

						await nightscoutClient.PostTreatmentsAsync(content);
						Console.WriteLine($"{insulinAdministration.DateTimeUtc} basal added");
					}
					else
					{
						Console.WriteLine($"{insulinAdministration.DateTimeUtc} basal ignored");
					}

					newLastTreatmentProcessDateTime = insulinAdministration.DateTimeUtc;
				}
				catch (Exception ex)
				{
					if (!ignoreErrors)
					{
						Console.WriteLine(ex);
						return (newLastTreatmentProcessDateTime, new ErrorInfo { Exception = ex, GlookoBaseDataDto = insulinAdministration });
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
