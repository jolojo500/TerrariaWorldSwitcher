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
    });

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
