using Avalonia.Controls;
using Avalonia.Interactivity;

namespace UI.Views;

public partial class WorldHistoryView : UserControl
{
    private readonly MainWindow _mainWindow;
    private readonly WorldInfo _world;
    
    public WorldHistoryView(MainWindow mainWindow, WorldInfo world)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        _world = world;
    }

    private void OnBackClicked(object? sender, RoutedEventArgs e)
    {
        _mainWindow.NavigateToWorldDetails(_world);
    }
}