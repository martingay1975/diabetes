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
			var downloadsFolder = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();
			openFileDialog1.Filter = "Diasend CSV|*.csv";
			openFileDialog1.InitialDirectory = downloadsFolder;
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				var diasendCsvPath = openFileDialog1.FileName;

				var settings = await SettingsModel.LoadAsync();

				this.txtBefore.Text = JsonSerializer.Serialize(settings);

				var dateFrom = chkOldData.Checked ? DateTime.MinValue : settings.LastTreatmentProcessDateTime;
				var ignoreErrors = chkOldData.Checked;

				var diasendToNightscoutTreatmentsManager = new DiasendToNightscoutTreatmentsManager(() => new NightscoutClient(settings.ApiSecretSha1Hash, settings.Host, settings.AllowNightscoutWrite));
				(settings.LastTreatmentProcessDateTime, var errorInfo) = await diasendToNightscoutTreatmentsManager.TransferAsync(diasendCsvPath, dateFrom, ignoreErrors);

				if (errorInfo != null)
				{
					MessageBox.Show($"Error: {JsonSerializer.Serialize(errorInfo.InsulinAdministration)} Exception:{errorInfo.Exception.Message}");
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
