// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var csvFiles = Directory.GetFiles(@"C:\git\diabetes", "*.csv");
int fileCounter = 0;

var outputFilePath = @"C:\git\diabetes\all.csv";

using (var outputStream = new StreamWriter(outputFilePath, true))
foreach (var csvFile in csvFiles)
{
	fileCounter++;
	var fileLines = File.ReadAllLines(csvFile);
	var lineCounter = 0;
	foreach (var line in fileLines)
	{
		lineCounter++;
		if (fileCounter > 1 && lineCounter == 1)
		{
			continue;
		}

		outputStream.WriteLine(line);
	}
	
}
