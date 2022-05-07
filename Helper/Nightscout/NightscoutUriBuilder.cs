using System.Text;
using System.Text.Json;

namespace Helper.Nightscout
{
	public class NightscoutUriBuilder
    {
        public class NightscoutUriParams
        {
            public string Path { get; set; }
			public HttpMethod HttpMethod { get; set; }
			public DateTime? ToDate { get; set; }
            public DateTime? FromDate { get; set; }
            public int? Count { get; set; }
            public string? DatePropertyName { get; set; }
			public string? Content { get; set; }

			public static NightscoutUriParams CreatePut<T>(string path, T? content)
			{
				var nsParams = new NightscoutUriParams
				{
					Path = path,
					HttpMethod = HttpMethod.Put,
					ToDate = null,
					FromDate = null,
					Count = null,
					DatePropertyName = null,
					Content = content == null ? null : JsonSerializer.Serialize<T>(value: content, options: NightscoutClient.jsonSerializerOptions)
				};

				return nsParams;
			}

			public static NightscoutUriParams CreateGetQuery(string path, string? datePropertyName, DateTime? fromDate)
			{
				return new NightscoutUriParams
				{
					Path = path,
					HttpMethod = HttpMethod.Get,
					ToDate = DateTime.UtcNow,
					FromDate = fromDate,
					Count = 10000,
					DatePropertyName = datePropertyName
				};
			}
		}

        private readonly string baseUrl;
		private readonly string apiSecretSha1Hash;

        public NightscoutUriBuilder(string apiSecretSha1Hash, string baseUrl)
        {
			this.apiSecretSha1Hash = apiSecretSha1Hash.ToLower() ?? throw new ArgumentNullException(nameof(apiSecretSha1Hash));
			this.baseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
        }

        public HttpRequestMessage Build(NightscoutUriParams nightscoutUriParams)
        {
            var uriBuilder = new UriBuilder($@"{baseUrl}/{nightscoutUriParams.Path}");
            
            if (nightscoutUriParams.FromDate != null)
            {
                uriBuilder.AddToQueryString($"find[{nightscoutUriParams.DatePropertyName}][$gte]={FormatDate(nightscoutUriParams.FromDate.Value)}");
            }

            if (nightscoutUriParams.ToDate != null)
            {
                uriBuilder.AddToQueryString($"find[{nightscoutUriParams.DatePropertyName}][$lt]={FormatDate(nightscoutUriParams.ToDate.Value)}");
            }

            if (nightscoutUriParams.Count != null)
            {
                uriBuilder.AddToQueryString($"count={nightscoutUriParams.Count.Value}");
            }

            var requestMessage  = new HttpRequestMessage
            {
                Method = nightscoutUriParams.HttpMethod,
                RequestUri = uriBuilder.Uri
            };

			if (nightscoutUriParams.Content != null)
			{
				requestMessage.Content = new StringContent(nightscoutUriParams.Content, Encoding.UTF8, "application/json");
			}

			// request to say we want the response as json
			requestMessage.Headers.Add("Accept", "application/json");

			// SHA1 hash (lowercase) - Used https://passwordsgenerator.net/sha1-hash-generator/
			requestMessage.Headers.Add("API-SECRET", this.apiSecretSha1Hash);
            return requestMessage;
        }

		private static string FormatDate(DateTime dt)
		{
			return dt.ToString("yyyy-MM-dd");
		}
	}
}
