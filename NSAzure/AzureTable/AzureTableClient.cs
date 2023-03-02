using Azure.Data.Tables;

namespace NSAzure.AzureTable
{
	public class AzureTableClient
	{
		private TableClient tableClient;

		public AzureTableClient()
		{
			//var connectionStringAzureite = @"AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";
			var connectionStringAzure = @"DefaultEndpointsProtocol=https;AccountName=diabetesstorage1;AccountKey=+T2Ir09O6QGMbG2CZu2at8Hk4Clj4WzYcPzp0pS0UgpvJqE2YZoA8mrvadLjcGanoTbVU7wUNRB5+AStjpD0eQ==;EndpointSuffix=core.windows.net";
			var serviceClient = new TableServiceClient(connectionStringAzure);

			this.tableClient = serviceClient.GetTableClient("bloodGlucose");
			this.tableClient.CreateIfNotExists();
		}

		public async Task AddAsync(BloodGlucoseItem bloodGlucoseItem)
		{
			await this.tableClient.AddEntityAsync<BloodGlucoseItem>(bloodGlucoseItem);
		}

		//public async Task Query()
		//{
		//	await this.tableClient.QueryAsync<BloodGlucoseItem>(bloodGlucoseItem => bloodGlucoseItem.DateTime)
		//}
	}
}
