namespace Helper.Nightscout
{
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
