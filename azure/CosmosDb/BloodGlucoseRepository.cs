using Microsoft.Azure.Cosmos;
using System.Net;

namespace azure.CosmosDb
{
    public class BloodGlucoseRepository
    {
        // The Cosmos client instance
        private CosmosClient cosmosClient;

        // The database we will create
        private Database? database;

        // The container we will create.
        private Container? container;

        public BloodGlucoseRepository()
        {
            cosmosClient = new CosmosClient(Constants.EndpointUri, Constants.PrimaryKey);
        }

        public async Task CreateDatabaseAsync()
        {
            database = await cosmosClient.CreateDatabaseIfNotExistsAsync(Constants.databaseId);
            container = await database.CreateContainerIfNotExistsAsync(Constants.containerId, "/date") ?? throw new Exception("No container retrieved");
        }

        public async Task<BloodGlucoseItem> AddAsync(BloodGlucoseItem bloodGlucoseItem)
        {
            if (container == null)
            {
                await CreateDatabaseAsync();
            }

            ItemResponse<BloodGlucoseItem> item;
            try
            {
                // Read the item to see if it exists.  
                item = await container?.ReadItemAsync<BloodGlucoseItem>(bloodGlucoseItem.id, new PartitionKey(bloodGlucoseItem.date));
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                try
                {
                    // Create an item in the container representing the Andersen family. Note we provide the value of the partition key for this item, which is "Andersen"
                    item = await container?.CreateItemAsync(bloodGlucoseItem, new PartitionKey(bloodGlucoseItem.date));
                }
                catch (CosmosException exCreate)
                {
                    Console.WriteLine(exCreate.ToString());
                    throw;
                }
            }

            return item.Resource;
        }
    }
}
