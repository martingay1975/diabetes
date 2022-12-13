using System.Security.Cryptography;
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
			public string? Id { get; set; }
			public int? Count { get; set; }
			public string? DatePropertyName { get; set; }
			public string? Content { get; set; }

			public static NightscoutUriParams CreatePost<T>(string path, T? content)
			{
				var nsParams = new NightscoutUriParams
				{
					Path = path,
					HttpMethod = HttpMethod.Post,
					ToDate = null,
					FromDate = null,
					Count = null,
					DatePropertyName = null,
					Content = content == null ? null : JsonSerializer.Serialize<T>(value: content, options: NightscoutClient.jsonSerializerOptions),
					Id = null
				};

				return nsParams;
			}

			public static NightscoutUriParams CreateDelete(string path, string id)
			{
				var nsParams = new NightscoutUriParams
				{
					Path = path,
					HttpMethod = HttpMethod.Delete,
					ToDate = null,
					FromDate = null,
					Count = 1,
					DatePropertyName = null,
					Id = id
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
					DatePropertyName = datePropertyName,
					Id = null
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

			if (nightscoutUriParams.Id != null)
			{
				uriBuilder.AddToQueryString($"find[_id][$eq]={nightscoutUriParams.Id}");
			}

			var requestMessage = new HttpRequestMessage
			{
				Method = nightscoutUriParams.HttpMethod,
				RequestUri = uriBuilder.Uri
			};

			if (nightscoutUriParams.Content != null)
			{
				requestMessage.Content = new StringContent(nightscoutUriParams.Content, Encoding.UTF8, "application/json");
			}
			else
			{
				// request to say we want the response as json
				requestMessage.Headers.Add("Accept", "application/json");
			}


			requestMessage.Headers.Add("API-SECRET", Hash(this.apiSecretSha1Hash));
			return requestMessage;
		}

		private static string Hash(string input)
		{
			using var sha1 = SHA1.Create();
			return Convert.ToHexString(sha1.ComputeHash(Encoding.UTF8.GetBytes(input))).ToLower();
		}

		private static string FormatDate(DateTime dt)
		{
			return dt.ToString("yyyy-MM-dd");
		}
	}
}
