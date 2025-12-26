using Avalonia.Controls;

namespace UI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }


    private void OnNetworkTestClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        StatusText.Text = "Network test clicked!";
    }

}