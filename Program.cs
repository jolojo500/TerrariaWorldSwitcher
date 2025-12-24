var worlds = WorldScanner.GetWorlds();
if (worlds.Count == 0)
{
    Console.WriteLine("No worlds found.");
    return;
}

Console.WriteLine("Detected worlds:\n");
for (int i = 0; i < worlds.Count; i++)
{
    Console.WriteLine($"{i + 1}. {worlds[i].Name}");
}


Console.Write("\nSelect world number: ");
int choice = int.Parse(Console.ReadLine()!) - 1;
var selectedWorld = worlds[choice];

Console.WriteLine($"\nProcessing world: {selectedWorld.Name}");

string stagingDir = WorldStager.StageWorld(selectedWorld);
Console.WriteLine($"World staged at: {stagingDir}");

string zipPath = WorldZipper.ZipWorld(stagingDir, selectedWorld.Name);
Console.WriteLine($"World zipped at: {zipPath}");

string archivedZipPath = WorldArchiver.Archive(zipPath, selectedWorld.Name);
Console.WriteLine($"World archived at: {archivedZipPath}");
/*
string disabledDir = WorldDisabler.Disable(selectedWorld);
Console.WriteLine($"World disabled at: {disabledDir}");
*/
WorldStager.CleanStaging(stagingDir);
Console.WriteLine("Staging folder cleaned.");


Console.WriteLine("\nProcess completed successfully.");
Console.WriteLine($"Archived: {archivedZipPath}");
//Console.WriteLine($"Disabled: {disabledDir}");
