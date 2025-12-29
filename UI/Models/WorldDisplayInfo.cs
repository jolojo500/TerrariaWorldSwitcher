using System.Collections.Generic;

namespace UI.Models;

public class WorldDisplayInfo
{
    public string Name { get; set; } = "";
    public List<string> Files { get; set; } = new();
    public string SizeFormatted { get; set; } = "";
    public string LastModified { get; set; } = "";
}