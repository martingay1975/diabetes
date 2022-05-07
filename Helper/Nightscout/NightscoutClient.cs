using System.Text.Json;
using static Helper.Nightscout.NightscoutUriBuilder;

namespace Helper.Nightscout
{
    public class NightscoutClient
    {
        private static HttpClient httpClient = new HttpClient();
        public readonly static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        private readonly NightscoutUriBuilder nightscoutUriBuilder;

		public NightscoutClient(string apiSecretSha1Hash, string host)
		{
			var baseUrl = $@"{host}/api/v2/";
			nightscoutUriBuilder = new NightscoutUriBuilder(apiSecretSha1Hash, baseUrl);
		}

		/// <summary>
		/// Get Blood Glucose Entries from Nightscout from the given date, till yesterday the whole of yesterday. Get complete days.
		/// </summary>
		/// <param name="dtLastDate">Get all entries equal or from the date</param>
		/// <returns></returns>
		public async Task<(List<EntryDto> entries, DateTime upToDate)> GetEntriesAsync(DateTime fromDate)
        {
			var nsParams = NightscoutUriParams.CreateGetQuery(path: "entries", datePropertyName: "dateString", fromDate: fromDate);
			var ret = await SendAsync<List<EntryDto>>(nsParams);
            var sortedEntries = ret?.OrderBy(x => x.SysTime).ToList() ?? new List<EntryDto>();
            return (entries: sortedEntries, upToDate: nsParams.ToDate.Value);
        }

		/// <summary>
		/// Get Treatments from Nightscout from the given date range
		/// </summary>
		/// <param name="dtLastDate"></param>
		/// <param name="dtToDate"></param>
		/// <returns></returns>
		public async Task<List<TreatmentDto>> GetTreatmentsAsync(DateTime fromDate, DateTime? toDate = null)
		{
			var nsParams = NightscoutUriParams.CreateGetQuery(path: "treatments", datePropertyName: "created_at", fromDate: fromDate);
			nsParams.ToDate = toDate;
			var ret = await SendAsync<List<TreatmentDto>>(nsParams);
			return ret;
		}

		/// <summary>
		/// Add Treatments to Nightscout
		/// </summary>
		/// <param name="treatmentDto"></param>
		/// <returns></returns>
		public async Task<List<TreatmentDto>> PutTreatmentsAsync(List<TreatmentDto> treatmentListDto)
		{
			var nsParams = NightscoutUriParams.CreatePut(path: "treatments", content: treatmentListDto);
			var ret = await SendAsync<List<TreatmentDto>>(nsParams);
			return ret;
		}

		private async Task<T> SendAsync<T>(NightscoutUriParams nightscoutUriParams)
		{
			var requestMessage = nightscoutUriBuilder.Build(nightscoutUriParams);

			var httpResponse = await httpClient.SendAsync(requestMessage);
			try
			{
				httpResponse.EnsureSuccessStatusCode();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				throw;
			}

			var contentStream = await httpResponse.Content.ReadAsStreamAsync();
			var ret = await JsonSerializer.DeserializeAsync<T>(contentStream, jsonSerializerOptions);
			return ret ?? throw new Exception("Failed");
		}
    }
}
