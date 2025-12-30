using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace UI.Views;

public partial class WorldDetailsView : UserControl
{
    private readonly MainWindow _mainWindow;
    private readonly WorldInfo _world;
    private bool _isTransferring = false;
    
    public WorldDetailsView(MainWindow mainWindow, WorldInfo world)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        _world = world;
        
        WorldNameText.Text = $"🗺️ {world.Name}";
        WorldInfoText.Text = $"{world.Files.Count} files";
    }

    private void OnBackClicked(object? sender, RoutedEventArgs e)
    {
        _mainWindow.NavigateToWorldList();
    }

    private async void OnTransferHostClicked(object? sender, RoutedEventArgs e)
    {
        if (_isTransferring) return;
        
        _isTransferring = true; //TODO maybe for consistensy check with our already present app states
        TransferButton.IsEnabled = false;
        
        try
        {
            StatusText.Text = "Preparing world...";
            
            await Task.Run(() =>
            {
                //stage world
                var stagingDir = WorldStager.StageWorld(_world);
                
                //zip world
                var zipPath = WorldZipper.ZipWorld(stagingDir, _world.Name);
                
                //pretty self explanatory
                var localIp = GetLocalIPAddress();
                
                //Update ui thread (because we must be on the ui thread to mess with ui)
                Dispatcher.UIThread.Post(() =>
                {
                    StatusText.Text = "Waiting for client connection...";
                    IpText.Text = $"Your IP: {localIp}";
                    IpText.IsVisible = true;
                    TransferProgress.IsVisible = true;
                    ProgressText.IsVisible = true;
                });
                
                //Send world (no actions can be done until sent)
                WorldSender.SendWorld(zipPath, (sent, total) =>
                {
                    var percent = (int)((sent * 100) / total);
                    
                    Dispatcher.UIThread.Post(() =>
                    {
                        TransferProgress.Value = percent;
                        ProgressText.Text = $"{percent}% ({sent / (1024 * 1024)} MB / {total / (1024 * 1024)} MB)";
                    });
                });
                
                //archive and disable
                var archivedPath = WorldArchiver.Archive(zipPath, _world.Name);
                WorldArchiver.UpdateHistory(System.IO.Path.GetDirectoryName(archivedPath)!);
                
                WorldDisabler.Disable(_world);
                
                //cleanup
                WorldStager.CleanStaging(stagingDir);
                
                //success
                Dispatcher.UIThread.Post(() =>
                {
                    StatusText.Text = "✅ Transfer completed successfully!";
                    ProgressText.Text = "World has been transferred and disabled locally.";
                    TransferButton.Content = "✅ Transfer Complete";
                });
            });
        }
        catch (Exception ex)
        {
            StatusText.Text = $"❌ Error: {ex.Message}";
            TransferButton.IsEnabled = true;
            _isTransferring = false;
        }
    }
    
    private string GetLocalIPAddress()
    {
        try
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ip = host.AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            
            return ip?.ToString() ?? "Unknown";
        }
        catch
        {
            return "Unable to detect IP";
        }
    }
}