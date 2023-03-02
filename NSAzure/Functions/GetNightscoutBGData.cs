using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace NSAzure.Functions
{
	public class GetNightscoutBGData
	{
		[Function("GetNightscoutBGData")]
		public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData request,
			ILogger log)
		{
			var bloodGlucoseManager = new BloodGlucoseManager();
			var count = await bloodGlucoseManager.ProcessAsync();

			var response = request.CreateResponse(HttpStatusCode.OK);
			await response.WriteStringAsync(count.ToString());

			return response;
		}
	}
}
