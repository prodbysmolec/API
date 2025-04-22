using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client.Avalonia.Views;

public partial class ArtikelView : UserControl
{
    public ArtikelView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}