public static class WorldStager
{
    public static string StageWorld(WorldInfo world)
    {
        string worldStagingDir = Path.Combine(Paths.Staging, world.Name);

        Directory.CreateDirectory(worldStagingDir);

        File.SetAttributes(worldStagingDir, FileAttributes.Normal);
        foreach (var file in world.Files)
        {
            string source = Path.Combine(Paths.Worlds, file);
            string destination = Path.Combine(worldStagingDir, file);

            File.Copy(source, destination, overwrite: true);
            File.SetAttributes(destination, FileAttributes.Normal);

            //Console.WriteLine($"Copied: {file}");
        }

        return worldStagingDir; //for zipping etc
    }

    public static void CleanStaging(string worldStagingDir)
    {
        if (!Directory.Exists(worldStagingDir))
        {
            //Console.WriteLine("Staging folder already clean.");
            return;
        }

        try
        {
            //Small delay to ensure zip file handles are realesed and we can actually def parent folder
            System.Threading.Thread.Sleep(100);

            Directory.Delete(worldStagingDir,recursive: true);
            //Console.WriteLine("Staging cleanup completed.");
        }
        catch(IOException ex)
        {
            Console.WriteLine("Cleanup failed (probably file lock).");
            Console.WriteLine(ex.Message);
            //TODO implement a retry system or find out if I can manually close handles
        }
    }
}
