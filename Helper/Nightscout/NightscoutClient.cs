using System.Diagnostics;
using System.Text.Json;
using static Helper.Nightscout.NightscoutUriBuilder;

namespace Helper.Nightscout
{
	public class NightscoutClient
	{
		private static HttpClient httpClient = new HttpClient();
		public readonly static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
		private readonly NightscoutUriBuilder nightscoutUriBuilder;
		private readonly bool allowNightscoutWrite;

		public NightscoutClient(string apiSecretSha1Hash, string host, bool allowNightscoutWrite)
		{
			if (string.IsNullOrEmpty(apiSecretSha1Hash))
			{
				throw new ArgumentException($"'{nameof(apiSecretSha1Hash)}' cannot be null or empty.", nameof(apiSecretSha1Hash));
			}

			if (string.IsNullOrEmpty(host))
			{
				throw new ArgumentException($"'{nameof(host)}' cannot be null or empty.", nameof(host));
			}

			var baseUrl = $@"{host}/api/v2/";
			nightscoutUriBuilder = new NightscoutUriBuilder(apiSecretSha1Hash, baseUrl);
			this.allowNightscoutWrite = allowNightscoutWrite;
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
			var sortedEntries = ret?.OrderByDescending(x => x.SysTime).ToList() ?? new List<EntryDto>();
			return (entries: sortedEntries, upToDate: nsParams.ToDate.Value);
		}

		public async Task<(List<EntryDto> entries, DateTime upToDate)> GetEntriesAsync(int count)
		{
			var nsParams = NightscoutUriParams.CreateGetQuery(path: "entries", count: 300);
			var ret = await SendAsync<List<EntryDto>>(nsParams);
			var sortedEntries = ret?.OrderByDescending(x => x.SysTime).ToList() ?? new List<EntryDto>();
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

		public async Task DeleteTreatmentAsync(string id)
		{
			var nsParams = NightscoutUriParams.CreateDelete(path: "treatments", id: id);
			await SendAsync(nsParams, this.allowNightscoutWrite);
		}

		/// <summary>
		/// Add Treatments to Nightscout
		/// </summary>
		/// <param name="treatmentDto"></param>
		/// <returns></returns>
		public async Task<List<TreatmentDto>> PostTreatmentsAsync(List<TreatmentDto> treatmentListDto)
		{
			var nsParams = NightscoutUriParams.CreatePost(path: "treatments", content: treatmentListDto);
			var ret = await SendAsync<List<TreatmentDto>>(nsParams, this.allowNightscoutWrite);
			Debug.WriteLine($"Sent: {treatmentListDto.FirstOrDefault()?.Created_at}");
			return ret;
		}


		private async Task<HttpResponseMessage> SendAsync(NightscoutUriParams nightscoutUriParams, bool sendIt = true)
		{
			var requestMessage = nightscoutUriBuilder.Build(nightscoutUriParams);

			if (!sendIt)
			{
				var content = requestMessage?.Content?.ReadAsStringAsync();
				var content1 = content == null ? null : await content;
				Debug.WriteLine($"Fake send {requestMessage?.Method} {requestMessage?.RequestUri} `with content: {content1}");
			}

			try
			{
				if (requestMessage.Content != null)
				{
					Debug.WriteLine($"Fake send {requestMessage?.Method} {requestMessage?.RequestUri} `with content: {await requestMessage.Content.ReadAsStringAsync()}");
				}
				var httpResponse = await httpClient.SendAsync(requestMessage ?? throw new Exception("No request specified"));
				try
				{
					httpResponse.EnsureSuccessStatusCode();
				}
				catch (Exception ex)
				{
					var response = await httpResponse.Content.ReadAsStringAsync();
					Debug.WriteLine($"ERROR response: {response}");
					throw;
				}

				return httpResponse;
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Exception: {ex}.");
				Debug.WriteLine($"Uri: {requestMessage?.RequestUri}");
				Debug.WriteLine($"Request: {JsonSerializer.Serialize(requestMessage)}");
				throw;
			}
		}

		/// <summary>
		/// Send a Http Request built from the information in <paramref name="nightscoutUriParams"/>. Checks it's successful and reads a JSON deserialized result back
		/// </summary>
		/// <typeparam name="TResponseType"></typeparam>
		/// <param name="nightscoutUriParams"></param>
		/// <returns>The JSON deserialized of the response from the http call</returns>
		private async Task<TResponseType> SendAsync<TResponseType>(NightscoutUriParams nightscoutUriParams, bool sendIt = true)
		{
			var httpResponse = await SendAsync(nightscoutUriParams, sendIt);
			var contentStream = await httpResponse.Content.ReadAsStreamAsync();
			var ret = await JsonSerializer.DeserializeAsync<TResponseType>(contentStream, jsonSerializerOptions);
			return ret ?? throw new Exception("Failed");
		}
	}
}
