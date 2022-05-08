

using System.Text.Json;

namespace RunExe
{
	public class SettingsModel
	{
		public DateTime LastTreatmentProcessDateTime { get; set; }
		public string ApiSecretSha1Hash { get; set; }
		public string Host { get; set; }

		private readonly static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

		private readonly static string Path = $@"{Environment.SpecialFolder.CommonApplicationData}\runExe-config.json";

		public static async Task<SettingsModel> LoadAsync()
		{
			var settings = new SettingsModel();
			try
			{
				var settingsText = await File.ReadAllTextAsync(Path);
				settings = JsonSerializer.Deserialize<SettingsModel>(settingsText, jsonSerializerOptions);
			}
			catch
			{
				settings = new SettingsModel()
				{
					LastTreatmentProcessDateTime = DateTime.Parse("2022-04-20 08:00:00"),
					ApiSecretSha1Hash = "1fbf2b03609cc5c3cb08f9f3d6d2f907434f2dc2",
					Host = "https://mgns.herokuapp.com"
				};
			}
			return settings;
		}

		public static async Task SaveAsync(SettingsModel settingsModel)
		{
			var settings = JsonSerializer.Serialize<SettingsModel>(settingsModel, jsonSerializerOptions);
			await File.WriteAllTextAsync(Path, settings);
		}
	}
}
