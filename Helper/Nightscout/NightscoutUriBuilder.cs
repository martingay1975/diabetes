namespace Helper.Nightscout
{
    public class NightscoutUriBuilder
    {
        public class NightscoutUriParams
        {
            public string Path { get; set; }
            public DateTime? ToDate { get; set; } = DateTime.UtcNow;
            public DateTime? FromDate { get; set; } = null;
            public int? Count { get; set; } = 10000;
            public HttpMethod HttpMethod { get; set; } = HttpMethod.Get;
            public string DatePropertyName { get; set; } = "created_at";  
        }
        private string host;
        private static string FormatDate(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        public NightscoutUriBuilder(string host)
        {
            this.host = host ?? throw new ArgumentNullException(nameof(host));
        }

        public HttpRequestMessage Build(NightscoutUriParams nightscoutUriParams)
        {
            var uriBuilder = new UriBuilder($@"{host}/{nightscoutUriParams.Path}");
            
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

            requestMessage.Headers.Add("Accept", "application/json");
            return requestMessage;
        }


    }

    public static class UriBuilderExtenstion
    {
        public static void AddToQueryString(this UriBuilder uriBuilder, string queryPart)
        {
            if (string.IsNullOrEmpty(uriBuilder.Query))
            {
                uriBuilder.Query = $"?{queryPart}";
            }
            else
            {
                uriBuilder.Query += $"&{queryPart}";
            }
        }
    }

}