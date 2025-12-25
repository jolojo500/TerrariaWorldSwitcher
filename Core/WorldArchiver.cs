public static class WorldArchiver
{
    public static string Archive(string zipPath, string worldName)
    {
        string worldArchiveDir = Path.Combine(
            Paths.OldWorlds,
            worldName
        );

        Directory.CreateDirectory(worldArchiveDir);
        File.SetAttributes(worldArchiveDir, FileAttributes.Normal);

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

        string archivedZipPath = Path.Combine(
            worldArchiveDir,
            $"{timestamp}.zip"
        );

        File.Move(zipPath, archivedZipPath);
        //Console.WriteLine($"Archived world to: {archivedZipPath}");

        //Dont think I wanna call it here for better flow visualing and error handling
        //UpdateHistory(worldArchiveDir); //Can always not call it here in the future

        return archivedZipPath;
    }

    public static void UpdateHistory(string worldArchiveDir)
    {
        //for versioning history (who was prev host etc)
        string historyFile = Path.Combine(worldArchiveDir, "history.txt");
        string hostName = Environment.UserName; //TODO change to user gui or steam  username or something
        string historyEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Host: {hostName}";
        File.AppendAllText(historyFile, historyEntry + Environment.NewLine);
        //Console.WriteLine("Host history updated.");
        
        return;
    }
}
