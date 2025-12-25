using System.IO.Compression;

public static class WorldRestorer
{
    //restore a world from an archived zip (old_worlds folder) into tModLoader Worlds folder
    public static void RestoreFromArchive(string zipPath)
    {
        if (!File.Exists(zipPath))
            throw new FileNotFoundException("Archive not found", zipPath);

        //Directory.CreateDirectory(Paths.Worlds);//prolly unnecessary

        ZipFile.ExtractToDirectory(
            zipPath,
            Paths.Worlds,
            overwriteFiles: true
        );
    }

    //restore a world from disabled_worlds back into tModLoader Worlds
    public static void RestoreDisabledWorld(string worldName)
    {
        string disabledDir = Path.Combine(Paths.DisabledWorlds, worldName);

        if (!Directory.Exists(disabledDir))
            throw new DirectoryNotFoundException("Disabled world not found");

        foreach (var file in Directory.GetFiles(disabledDir))
        {
            string fileName = Path.GetFileName(file);
            string destination = Path.Combine(Paths.Worlds, fileName);

            File.Move(file, destination, overwrite: true);
        }

        //cleanup of empty folder
        if (Directory.GetFiles(disabledDir).Length == 0)
        {
            Directory.Delete(disabledDir);
        }
    }
}
