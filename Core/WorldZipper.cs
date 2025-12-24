using System.IO.Compression;
public static class WorldZipper
{
    public static string ZipWorld(string worldStagingDir, string worldName)
    {
        string zipPath = Path.Combine(Paths.Staging, $"{worldName}.zip");

        if (File.Exists(zipPath))
        {
            File.Delete(zipPath); //gets deleted because old or wtv
        }

        //Console.WriteLine($"Creating zip: {zipPath}");
        ZipFile.CreateFromDirectory(
            worldStagingDir,
            zipPath,
            CompressionLevel.Optimal,
            includeBaseDirectory: false
        );

        //Console.WriteLine("Zip created successfully.");
        return zipPath;
    }
}
