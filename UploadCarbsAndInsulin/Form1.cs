using System.Text.Json;
using Helper;
using Helper.Nightscout;
using Microsoft.Win32;

namespace UploadCarbsAndInsulin
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private async void button1_Click(object sender, EventArgs e)
		{
			var updateProgressBarFn = () =>
			{
				progressBar1.Increment(1);
			};
			var updateProgressBarSetMax = (int maxVal) =>
			{
				progressBar1.Maximum = maxVal;
			};

			var downloadsFolder = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();
			this.folderBrowserDialog1.InitialDirectory = downloadsFolder;
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				var glookoUnzippedFolderPath = folderBrowserDialog1.SelectedPath;

				var settings = await SettingsModel.LoadAsync();

				this.txtBefore.Text = JsonSerializer.Serialize(settings);

				var dateFrom = chkOldData.Checked ? DateTime.MinValue : settings.LastTreatmentProcessDateTime;
				var ignoreErrors = chkOldData.Checked;
				ErrorInfo? errorInfo;
				var glookoToNightscoutTreatmentsManager = new GlookoToNightscoutTreatmentsManager(() => new NightscoutClient(settings.ApiSecretSha1Hash, settings.Host, settings.AllowNightscoutWrite));
				try
				{
					var bolusCsvPath = Path.Combine(glookoUnzippedFolderPath, "Insulin Data", "bolus_data.csv");
					var basalCsvPath = Path.Combine(glookoUnzippedFolderPath, "Insulin Data", "basal_data.csv");
					(var lastBolusTime, errorInfo) = await glookoToNightscoutTreatmentsManager.TransferBolusAsync(bolusCsvPath, dateFrom, ignoreErrors, updateProgressBarFn, updateProgressBarSetMax);
					(var lastBasalTime, errorInfo) = await glookoToNightscoutTreatmentsManager.TransferBasalAsync(basalCsvPath, dateFrom, ignoreErrors, updateProgressBarFn, updateProgressBarSetMax);
					settings.LastTreatmentProcessDateTime = new DateTime(Math.Max(lastBasalTime.Ticks, lastBolusTime.Ticks));
				}
				catch (Exception ex)
				{
					errorInfo = new ErrorInfo { Exception = ex };
				}

				if (errorInfo != null)
				{
					MessageBox.Show($"Error: {JsonSerializer.Serialize(errorInfo?.GlookoBaseDataDto)} Exception:{errorInfo.Exception.Message}");
				}

				if (settings.AllowNightscoutWrite && chkOldData.Checked == false)
				{
					await SettingsModel.SaveAsync(settings);
					this.txtAfter.Text = JsonSerializer.Serialize(settings);
				}
				else
				{
					this.txtAfter.Text = "Finished";
				}
			}

		}
	}
}
