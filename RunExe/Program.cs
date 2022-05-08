// See https://aka.ms/new-console-template for more information
using Helper.Nightscout;
using Helper;
using RunExe;




const string diasendCsvPath = @"c:\git\diabetes\20AprTo3May.csv";
var settings = await SettingsModel.LoadAsync();
var diasendToNightscoutTreatmentsManager = new DiasendToNightscoutTreatmentsManager(() => new NightscoutClient(settings.ApiSecretSha1Hash, settings.Host));
await diasendToNightscoutTreatmentsManager.DeleteComboBolus(new DateTime(2022, 4, 19));
//settings.LastTreatmentProcessDateTime = await diasendToNightscoutTreatmentsManager.TransferAsync(diasendCsvPath, settings.LastTreatmentProcessDateTime);
//await SettingsModel.SaveAsync(settings);

