using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using System;
using System.Threading.Tasks;

namespace UI.Views;

public partial class ReceiveWorldDialog : Window
{
    private bool _isReceiving = false;
    
    public ReceiveWorldDialog()
    {
        InitializeComponent();
    }

    private void OnCancelClicked(object? sender, RoutedEventArgs e)
    {
        if (!_isReceiving)
        {
            Close();
        }
    }

    private async void OnConnectClicked(object? sender, RoutedEventArgs e)
    {
        if (_isReceiving) return;
        
        var hostIp = IpInput.Text?.Trim();
        
        if (string.IsNullOrEmpty(hostIp))
        {
            StatusText.Text = "❌ Please enter a host IP address"; //TODO maybe make it cleaner, color instead of emojis and whatnot
            return;
        }
        
        _isReceiving = true;
        ConnectButton.IsEnabled = false;
        IpInput.IsEnabled = false;
        
        StatusText.Text = $"Connecting to {hostIp}...";
        ProgressPanel.IsVisible = true;
        
        try
        {
            await Task.Run(() =>
            {
                //getting the world
                var receivedZipPath = WorldReceiver.ReceiveWorld(hostIp, (received, total) =>
                {
                    var percent = (int)((received * 100) / total);
                    
                    Dispatcher.UIThread.Post(() =>
                    {
                        ReceiveProgress.Value = percent;
                        ProgressText.Text = $"{percent}% ({received / (1024 * 1024)} MB / {total / (1024 * 1024)} MB)";
                    });
                });
                
                //extracting in worlds folder and as usual for ui change gotta post it to uithread
                Dispatcher.UIThread.Post(() =>
                {
                    StatusText.Text = "Extracting world...";
                });
                WorldRestorer.RestoreFromArchive(receivedZipPath);
                
                //cleanup of the zip perhaps since doesnt matter for versioning I think (in future TODO  will add history.txt handling)
                System.IO.File.Delete(receivedZipPath);
                
                //success
                Dispatcher.UIThread.Post(() =>
                {
                    StatusText.Text = "✅ World received successfully!";
                    ProgressText.Text = "You are now the host. The world is ready to play!";
                    ConnectButton.Content = "✅ Done";
                    ConnectButton.IsEnabled = true;
                    ConnectButton.Click -= OnConnectClicked;
                    ConnectButton.Click += (s, ev) => Close();
                });
            });
        }
        catch (Exception ex)
        {
            StatusText.Text = $"❌ Error: {ex.Message}";
            ConnectButton.IsEnabled = true;
            IpInput.IsEnabled = true;
            _isReceiving = false;
        }
    }
}