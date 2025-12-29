using Avalonia.Controls;
using UI.Views;

namespace UI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        NavigateToWorldList(); //default page at start
    }

    // Méthodes de navigation
    public void NavigateToWorldList()
    {
        PageContent.Content = new WorldListView(this);
    }

    public void NavigateToWorldDetails(WorldInfo world)
    {
        PageContent.Content = new WorldDetailsView(this, world);
    }

    public void NavigateToWorldHistory(WorldInfo world)
    {
        PageContent.Content = new WorldHistoryView(this, world);
    }

    // Action du bouton "Receive World"
    private async void OnReceiveWorldClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // TODO: implement this later
        var dialog = new Window
        {
            Title = "Receive World",
            Width = 400,
            Height = 200,
            Content = new TextBlock { Text = "Feature coming soon!" }
        };
        await dialog.ShowDialog(this);
    }
}