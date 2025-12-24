// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.IO.Compression;

string worldsPath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
    "My Games",
    "Terraria",
    "tModLoader",
    "Worlds"
);

string stagingPath = Path.Combine(Directory.GetCurrentDirectory(),"staging");

Directory.CreateDirectory(stagingPath); //in case it's gone better be safe than sorry I suppose

if (!Directory.Exists(worldsPath))
{
    Console.WriteLine("Worlds folder not found.");
    return;
}

var files = Directory.GetFiles(worldsPath);

var worlds = files
    .Select(f => Path.GetFileName(f))
    .GroupBy(name =>
    {
        var parts = name.Split(".");
        return parts[0];
    })
    .ToList();

if (worlds.Count == 0)
{
    Console.WriteLine("No worlds found.");
    return;
}

Console.WriteLine("Detected worlds:\n");
foreach (var world in worlds)
{
    Console.WriteLine($"World: {world.Key}");

    foreach(var file in world)
    {
        Console.WriteLine($"  - {file}");
    }
    Console.WriteLine();
}

var selectedWorld = worlds[0]; // testing, will change to wtv gui interaction
string worldName = selectedWorld.Key;
string worldStagingDir = Path.Combine(stagingPath, worldName);

Directory.CreateDirectory(worldStagingDir);

Console.WriteLine($"Copying world '{worldName}' to staging...\n");

File.SetAttributes(worldStagingDir, FileAttributes.Normal); //needed because seems like code made dirs are readonly
foreach(var file in selectedWorld)
{
    string source = Path.Combine(worldsPath, file);
    string destination = Path.Combine(worldStagingDir, file);

    File.Copy(source, destination, overwrite: true);
    File.SetAttributes(destination, FileAttributes.Normal); //jsut in case we get a dir like situation in the futurue

    Console.WriteLine($"Copied: {file}");
}

Console.WriteLine("\nDone");

string zipPath = Path.Combine(stagingPath, $"{worldName}.zip");
if (File.Exists(zipPath))
{
    File.Delete(zipPath); //gets deleted because old or wtv
}

Console.WriteLine($"Creating zip: {zipPath}");

ZipFile.CreateFromDirectory(
    worldStagingDir,
    zipPath,
    CompressionLevel.Optimal,
    includeBaseDirectory: false
);

Console.WriteLine("Zip created successfully.");

string oldWorldsPath = Path.Combine(
    Directory.GetCurrentDirectory(),
    "old_worlds",
    worldName
);

Directory.CreateDirectory(oldWorldsPath); //same stuff

string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

string archivedZipPath = Path.Combine(
    oldWorldsPath,
    $"{timestamp}.zip"
);

File.Move(zipPath, archivedZipPath);
Console.WriteLine($"Archived world to: {archivedZipPath}");
//TODO add the delete or disabling by moving paths in actual terraria fodlers. Dont wanna lose worlds tho (at least now).

Console.WriteLine($"Cleaning up staging folder: {worldStagingDir}");

if (Directory.Exists(worldStagingDir))
{
    try
    {
        //Small delay to ensure zip file handles are realesed and we can actually def parent folder
        System.Threading.Thread.Sleep(100);

        Directory.Delete(worldStagingDir,recursive: true);
        Console.WriteLine("Staging cleanup completed.");
    }
    catch(IOException ex)
    {
        Console.WriteLine("Cleanup failed (probably file lock).");
        Console.WriteLine(ex.Message);
        //TODO implement a retry system or find out if I can manually close handles
    }
}
else
{
    Console.WriteLine("Staging folder already clean.");
}