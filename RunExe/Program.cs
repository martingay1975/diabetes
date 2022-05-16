using Helper.Nightscout;
using Helper;
using RunExe;

const string diasendCsvPath = @"c:\git\diabetes\08MayTo15May.csv";
var settings = await SettingsModel.LoadAsync();
var diasendToNightscoutTreatmentsManager = new DiasendToNightscoutTreatmentsManager(() => new NightscoutClient(settings.ApiSecretSha1Hash, settings.Host, settings.AllowNightscoutWrite));
settings.LastTreatmentProcessDateTime = await diasendToNightscoutTreatmentsManager.TransferAsync(diasendCsvPath, settings.LastTreatmentProcessDateTime);
if (settings.AllowNightscoutWrite)
{
	await SettingsModel.SaveAsync(settings);
}

