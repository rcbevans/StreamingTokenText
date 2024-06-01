using Microsoft.UI.Xaml.Controls;

namespace StreamingTokenTest;

public sealed partial class MainPage : Page
{
    public MainPageViewModel ViewModel { get; } = new();

    public MainPage()
    {
        this.InitializeComponent();
    }
}
