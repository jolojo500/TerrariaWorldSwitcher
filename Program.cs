AppContext.SetState(AppState.Idle);

Console.WriteLine("=== Terraria World Manager ===\n");

Console.WriteLine("1. Archive & disable a world");
Console.WriteLine("2. Restore a world");
Console.WriteLine("3. Network transfer (TEST)");
Console.WriteLine("4. Exit");

Console.Write("\nSelect option: ");
int mainChoice = int.Parse(Console.ReadLine()!);

Console.WriteLine();



switch (mainChoice)
{
    case 1:
        RunArchiveFlow();
        break;

    case 2:
        RunRestoreFlow();
        break;

    case 3:
        RunNetworkTest();
        break;
    
    case 4:
        return;

    default:
        Console.WriteLine("Invalid choice.");
        break;
}



static void RunArchiveFlow()
{
    var worlds = WorldScanner.GetWorlds();
    if (worlds.Count == 0)
    {
        Console.WriteLine("No worlds found.");
        return;
    }

    
    AppContext.SetState(AppState.SelectingWorld);


    Console.WriteLine("Detected worlds:\n");
    for (int i = 0; i < worlds.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {worlds[i].Name}");
    }


    Console.Write("\nSelect world number: ");
    int choice = int.Parse(Console.ReadLine()!) - 1;
    var selectedWorld = worlds[choice];

    Console.WriteLine($"\nProcessing world: {selectedWorld.Name}");

    string stagingDir = WorldStager.StageWorld(selectedWorld);
    Console.WriteLine($"World staged at: {stagingDir}");

    string zipPath = WorldZipper.ZipWorld(stagingDir, selectedWorld.Name);
    Console.WriteLine($"World zipped at: {zipPath}");

    AppContext.SetState(AppState.Archiving);
    string archivedZipPath = WorldArchiver.Archive(zipPath, selectedWorld.Name);
    Console.WriteLine($"World archived at: {archivedZipPath}");

    AppContext.SetState(AppState.Disabling);
    string disabledDir = WorldDisabler.Disable(selectedWorld);
    Console.WriteLine($"World disabled at: {disabledDir}");

    WorldStager.CleanStaging(stagingDir);
    Console.WriteLine("Staging folder cleaned.");


    Console.WriteLine("\nProcess completed successfully.");
    Console.WriteLine($"Archived: {archivedZipPath}");
    Console.WriteLine($"Disabled: {disabledDir}");
    AppContext.SetState(AppState.Done);
}

static void RunRestoreFlow()
{
    Console.WriteLine("Restore options:\n");
    Console.WriteLine("1. Restore from disabled worlds");
    Console.WriteLine("2. Restore from archive");

    Console.Write("\nSelect option: ");
    int restoreChoice = int.Parse(Console.ReadLine()!);

    Console.WriteLine();

    switch (restoreChoice)
    {
        case 1:
            RestoreDisabled();
            break;

        case 2:
            RestoreFromArchive();
            break;

        default:
            Console.WriteLine("Invalid choice.");
            break;
    }
}


static void RestoreDisabled()
{
    if (!Directory.Exists(Paths.DisabledWorlds))
    {
        Console.WriteLine("No disabled worlds found.");
        return;
    }

    var worlds = Directory.GetDirectories(Paths.DisabledWorlds)
        .Select(Path.GetFileName)
        .ToList();

    if (worlds.Count == 0)
    {
        Console.WriteLine("No disabled worlds found.");
        return;
    }

    Console.WriteLine("Disabled worlds:\n");
    for (int i = 0; i < worlds.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {worlds[i]}");
    }

    Console.Write("\nSelect world number: ");
    int choice = int.Parse(Console.ReadLine()!) - 1;

    string worldName = worlds[choice];

    AppContext.SetState(AppState.Restoring);
    WorldRestorer.RestoreDisabledWorld(worldName);
    AppContext.SetState(AppState.Done);

    Console.WriteLine($"\nWorld '{worldName}' restored successfully.");
}


static void RestoreFromArchive()
{
    if (!Directory.Exists(Paths.OldWorlds))
    {
        Console.WriteLine("No archived worlds found.");
        return;
    }

    var worldDirs = Directory.GetDirectories(Paths.OldWorlds);

    if (worldDirs.Length == 0)
    {
        Console.WriteLine("No archived worlds found.");
        return;
    }

    Console.WriteLine("Archived worlds:\n");
    for (int i = 0; i < worldDirs.Length; i++)
    {
        Console.WriteLine($"{i + 1}. {Path.GetFileName(worldDirs[i])}");
    }

    Console.Write("\nSelect world: ");
    int worldChoice = int.Parse(Console.ReadLine()!) - 1;

    string selectedWorldDir = worldDirs[worldChoice];
    var zips = Directory.GetFiles(selectedWorldDir, "*.zip");

    if (zips.Length == 0)
    {
        Console.WriteLine("No archives found.");
        return;
    }

    Console.WriteLine("\nAvailable archives:\n");
    for (int i = 0; i < zips.Length; i++)
    {
        Console.WriteLine($"{i + 1}. {Path.GetFileName(zips[i])}");
    }

    Console.Write("\nSelect archive: ");
    int zipChoice = int.Parse(Console.ReadLine()!) - 1;

    AppContext.SetState(AppState.Restoring);
    WorldRestorer.RestoreFromArchive(zips[zipChoice]);
    AppContext.SetState(AppState.Done);

    Console.WriteLine("\nWorld restored from archive successfully.");
}

static void RunNetworkTest()
{
    Console.WriteLine("Network test:\n");
    Console.WriteLine("1. Host (send world)");
    Console.WriteLine("2. Connect (receive world)");

    Console.Write("\nSelect option: ");
    int choice = int.Parse(Console.ReadLine()!);
    Console.WriteLine();

    if (choice == 1)
    {
        Console.Write("Path to world zip: ");
        string zipPath = Console.ReadLine()!; //Aka hard coded for now and why bother menu

        WorldSender.SendWorld(zipPath);
    }
    else if (choice == 2)
    {
        Console.Write("Host IP: ");
        string hostIp = Console.ReadLine()!;

        string receivedZip = WorldReceiver.ReceiveWorld(hostIp);
        Console.WriteLine($"Received at: {receivedZip}");
    }
}
