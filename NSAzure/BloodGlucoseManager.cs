using Helper.Nightscout;
using NSAzure.AzureTable;

namespace NSAzure
{
	public class BloodGlucoseManager
	{
		private NightscoutClient nightscoutClient;
		private AzureTableClient azureTableClient;

		public BloodGlucoseManager()
		{
			nightscoutClient = new NightscoutClient("mg1234nightscout", "https://mgnightscout.strangled.net", true);
			azureTableClient = new AzureTableClient();
		}

		public async Task<int> ProcessAsync()
		{
			var counter = 0;
			var (nightscoutDataList, lastUpdateDate) = await nightscoutClient.GetEntriesAsync(300);

			try
			{
				foreach (var nightscoutData in nightscoutDataList)
				{
					var bloodGlucoseItem = BloodGlucoseItemMapper.Map(nightscoutData);
					await azureTableClient.AddAsync(bloodGlucoseItem);
					counter++;
				}
			}
			catch (Exception)
			{
			}

			return counter;
		}
	}
}
