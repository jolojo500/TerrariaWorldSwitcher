public static class WorldScanner
{
    public static List<WorldInfo> GetWorlds()
    {
        if (!Directory.Exists(Paths.Worlds))
            return new List<WorldInfo>();

        var files = Directory.GetFiles(Paths.Worlds);

        var grouped = files
            .Select(f => Path.GetFileName(f))
            .GroupBy(name =>
            {
                var parts = name!.Split('.');
                return parts[0];
            });

        var worlds = new List<WorldInfo>();

        foreach (var group in grouped)
        {
            worlds.Add(new WorldInfo
            {
                Name = group.Key,
                Files = group.ToList()
            });
        }

        return worlds;
    }
}
