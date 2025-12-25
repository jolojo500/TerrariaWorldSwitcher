public static class WorldDisabler
{
    public static string Disable(WorldInfo world)
    {
        string disabledDir = Path.Combine(Paths.DisabledWorlds, world.Name);

        Directory.CreateDirectory(disabledDir);

        File.SetAttributes(disabledDir, FileAttributes.Normal);
        foreach (var file in world.Files)
        {
            string source = Path.Combine(Paths.Worlds, file);
            string destination = Path.Combine(disabledDir, file);

            File.Move(source, destination, overwrite: true);
        }
        //Console.WriteLine($"World '{world.Name}' moved to disabled_worlds.");

        return disabledDir;
    }
}
