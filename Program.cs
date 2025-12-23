// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

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

Console.WriteLine($"Copying world '{worldName} to staging...\n");

foreach(var file in selectedWorld)
{
    string source = Path.Combine(worldsPath, file);
    string destination = Path.Combine(worldStagingDir, file);

    File.Copy(source, destination, overwrite: true);
    Console.WriteLine($"Copied: {file}");
}

Console.WriteLine("\nDone");