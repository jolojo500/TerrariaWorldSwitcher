using System;
using Avalonia.Controls;
using Avalonia.Input;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UI.Models;

namespace UI.Views;

public partial class WorldListView : UserControl
{
    private readonly MainWindow _mainWindow;
    
    public WorldListView(MainWindow mainWindow)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        LoadWorlds();
    }

    private void LoadWorlds()
    {
        //already existing method
        var worlds = WorldScanner.GetWorlds();
        
        //adding info for cards
        var enrichedWorlds = worlds.Select(w => new WorldDisplayInfo
        {
            Name = w.Name,
            Files = w.Files,
            SizeFormatted = CalculateSize(w),
            LastModified = GetLastModified(w)
        }).ToList();

        WorldsItemsControl.ItemsSource = enrichedWorlds;
        
        //adding click handler
        WorldsItemsControl.PointerPressed += OnWorldClicked;
    }

    private void OnWorldClicked(object? sender, PointerPressedEventArgs e)
    {
        //finding out which world was clicked
        if (e.Source is Control control)
        {
            var world = control.DataContext as WorldDisplayInfo;
            if (world != null)
            {
                //go to detail page
                _mainWindow.NavigateToWorldDetails(new WorldInfo 
                { 
                    Name = world.Name, 
                    Files = world.Files 
                });
            }
        }
    }

    private string CalculateSize(WorldInfo world)
    {
        long totalBytes = 0;
        foreach (var file in world.Files)
        {
            var filePath = Path.Combine(Paths.Worlds, file);
            if (File.Exists(filePath))
            {
                totalBytes += new FileInfo(filePath).Length;
            }
        }
        
        double mb = totalBytes / (1024.0 * 1024.0);
        return $"{mb:F2} MB ({world.Files.Count} files)";
    }

    private string GetLastModified(WorldInfo world)
    {
        var dates = new List<DateTime>();
        foreach (var file in world.Files)
        {
            var filePath = Path.Combine(Paths.Worlds, file);
            if (File.Exists(filePath))
            {
                dates.Add(File.GetLastWriteTime(filePath));
            }
        }
        
        if (dates.Count == 0) return "Unknown";
        
        var latest = dates.Max();
        var timeAgo = DateTime.Now - latest;
        
        if (timeAgo.TotalHours < 1)
            return $"Modified {(int)timeAgo.TotalMinutes} minutes ago";
        if (timeAgo.TotalDays < 1)
            return $"Modified {(int)timeAgo.TotalHours} hours ago";
        
        return $"Modified {latest:yyyy-MM-dd HH:mm}";
    }
}

