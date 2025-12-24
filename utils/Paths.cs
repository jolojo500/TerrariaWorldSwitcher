public static class Paths
{
    public static string Worlds =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "My Games",
            "Terraria",
            "tModLoader",
            "Worlds"
        );

    public static string Staging =>
        Path.Combine(Directory.GetCurrentDirectory(), "staging");

    public static string OldWorlds =>
        Path.Combine(Directory.GetCurrentDirectory(), "old_worlds");

    public static string DisabledWorlds =>
        Path.Combine(Directory.GetCurrentDirectory(), "disabled_worlds");
}
