using System.Text.Json;

namespace Helper.Nightscout
{
    public class NightscoutClient
    {
        private const string baseUrl = @"https://mgns.herokuapp.com/api/v2/";
        private static HttpClient httpClient = new HttpClient();
        private static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        private static NightscoutUriBuilder nightscoutUriBuilder  = new NightscoutUriBuilder(baseUrl);


        /// <summary>
        /// Get Entries from Nightscout from the given date, till yesterday the whole of yesterday. Get complete days.
        /// </summary>
        /// <param name="dtLastDate">Get all entries equal or from the date</param>
        /// <returns></returns>
        public async Task<(List<EntryDto> entries, DateTime upToDate)> GetEntriesAsync(DateTime dtLastDate)
        {
            var nsParams = new NightscoutUriBuilder.NightscoutUriParams() {
                Path = "entries",
                FromDate = dtLastDate,
                DatePropertyName ="dateString"
            };
            var requestMessage = nightscoutUriBuilder.Build(nsParams);

            var httpResponse = await httpClient.SendAsync(requestMessage);
            var contentStream = await httpResponse.Content.ReadAsStreamAsync();

            var entries = await JsonSerializer.DeserializeAsync<EntryDto[]>(contentStream, jsonSerializerOptions);
            var sortedEntries = entries?.OrderBy(x => x.SysTime).ToList() ?? new List<EntryDto>();
            return (entries: sortedEntries, upToDate: nsParams.ToDate.Value);
        }

        public async Task<List<TreatmentDto>> GetTreatmentsAsync(DateTime dtLastDate, DateTime dtToDate)
        {
            var nsParams = new NightscoutUriBuilder.NightscoutUriParams()
            {
                Path = "treatments",
                FromDate = dtLastDate,
                ToDate = dtToDate
            };
            var requestMessage = nightscoutUriBuilder.Build(nsParams);

            var httpResponse = await httpClient.SendAsync(requestMessage);
            var contentStream = await httpResponse.Content.ReadAsStreamAsync();

            var ret = await JsonSerializer.DeserializeAsync<TreatmentDto[]>(contentStream, jsonSerializerOptions);
            return ret.ToList();
        }

        public async Task PutTreatmentsAsync(List<TreatmentDto> treatmentDto)
        {

        }
    }
}