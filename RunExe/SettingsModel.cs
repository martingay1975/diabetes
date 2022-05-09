using System.Text.Json;

namespace RunExe
{
	public class SettingsModel
	{
		public DateTime LastTreatmentProcessDateTime { get; set; }
		public string ApiSecretSha1Hash { get; set; }
		public string Host { get; set; }
		public bool AllowNightscoutWrite { get; set; }

		private readonly static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

		private readonly static string Path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\runExe-config.json";

		private static SettingsModel Default = new SettingsModel()
		{
			LastTreatmentProcessDateTime = DateTime.MinValue, // // must fill in
			ApiSecretSha1Hash = "",	// must fill in
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
				Console.WriteLine($"Load settings from {Path}");
			}
			catch
			{
				Console.WriteLine($"Load default settings");
			}

			Console.WriteLine(JsonSerializer.Serialize(settings));
			return settings;
		}

		public static async Task SaveAsync(SettingsModel settingsModel)
		{
			var settings = JsonSerializer.Serialize<SettingsModel>(settingsModel, jsonSerializerOptions);
			await File.WriteAllTextAsync(Path, settings);
			Console.WriteLine($"Saved settings to {Path}");
		}
	}
}
