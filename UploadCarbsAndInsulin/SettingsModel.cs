using System.Text.Json;

namespace UploadCarbsAndInsulin
{
	public class SettingsModel
	{
		public DateTime LastTreatmentProcessDateTime { get; set; }
		public string ApiSecretSha1Hash { get; set; }

		/// <summary>
		/// Working examples
		/// http://mgns.herokuapp.com
		/// http://mgnightscout.strangled.net
		/// </summary>
		public string Host { get; set; }
		public bool AllowNightscoutWrite { get; set; }

		private readonly static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

		private readonly static string Path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\UploadCarbsAndInsulin\runExe-config.json";

		private static SettingsModel Default = new SettingsModel()
		{
			LastTreatmentProcessDateTime = DateTime.MinValue, // // must fill in
			ApiSecretSha1Hash = "", // must fill in
			Host = "", // must fill in
			AllowNightscoutWrite = true
		};

		public static async Task<SettingsModel> LoadAsync()
		{
			var settings = Default;
			try
			{
				var settingsText = await File.ReadAllTextAsync(Path);
				settings = JsonSerializer.Deserialize<SettingsModel>(settingsText, jsonSerializerOptions) ?? Default;
			}
			catch
			{

			}

			Console.WriteLine(JsonSerializer.Serialize(settings));
			return settings;
		}

		public static async Task SaveAsync(SettingsModel settingsModel)
		{
			var settings = JsonSerializer.Serialize(settingsModel, jsonSerializerOptions);
			await File.WriteAllTextAsync(Path, settings);
		}

		//{"lastTreatmentProcessDateTime":"2022-11-23T06:27:00+00:00","apiSecretSha1Hash":"mg1234nightscout","host":"http://mgnightscout.strangled.net","allowNightscoutWrite":true}
	}
}
