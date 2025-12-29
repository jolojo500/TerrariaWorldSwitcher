using Avalonia.Controls;
using Avalonia.Interactivity;

namespace UI.Views;

public partial class WorldDetailsView : UserControl
{
    private readonly MainWindow _mainWindow;
    private readonly WorldInfo _world;
    
    public WorldDetailsView(MainWindow mainWindow, WorldInfo world)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        _world = world;
        
        WorldNameText.Text = $"🗺️ {world.Name}";
    }

    private void OnBackClicked(object? sender, RoutedEventArgs e)
    {
        _mainWindow.NavigateToWorldList();
    }

    private async void OnTransferHostClicked(object? sender, RoutedEventArgs e)
    {
        // TODO: Implémenter plus tard
        WorldNameText.Text = "Transfer Host clicked! (TODO)";
    }
}