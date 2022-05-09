using azure.CosmosDb;
using Helper.Nightscout;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace azure
{
    public class GetNightscoutBGData
    {
        private readonly ILogger _logger;

        public GetNightscoutBGData(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetNightscoutBGData>();
        }

        [Function("GetNightscoutBGData")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var bloodGlucoseManager = new BloodGlucoseManager();
            var count = await bloodGlucoseManager.ProcessAsync();
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString($"Processed {count} records");

            return response;
        }
    }

    public class BloodGlucoseManager
    {
        public async Task<int> ProcessAsync()
        {
            var counter = 0;
            var nightscoutClient = new NightscoutClient(null, null, true);
            var (nightscoutDataList, lastUpdateDate) = await nightscoutClient.GetEntriesAsync(new DateTime(2022,1,1));
            var bloodGlucoseItemList = nightscoutDataList.Select(BloodGlucoseItemMapper.Map);
            var bloodGlucoseRepository = new BloodGlucoseRepository();

            foreach (var item in bloodGlucoseItemList)
            {
                await bloodGlucoseRepository.AddAsync(item);
                counter++;
            }

            return counter;
        }
    }
}
