using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client.Avalonia.Controls.Navigation;

public partial class NavigationButton : UserControl
{
    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<NavigationButton, string>(nameof(Title));
            
    public static readonly StyledProperty<string> IconDataProperty =
        AvaloniaProperty.Register<NavigationButton, string>(nameof(IconData));
            
    public static readonly StyledProperty<bool> IsActiveProperty =
        AvaloniaProperty.Register<NavigationButton, bool>(nameof(IsActive));
            
    public static readonly StyledProperty<ICommand> NavigateCommandProperty =
        AvaloniaProperty.Register<NavigationButton, ICommand>(nameof(NavigateCommand));
            
    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
        
    public string IconData
    {
        get => GetValue(IconDataProperty);
        set => SetValue(IconDataProperty, value);
    }
        
    public bool IsActive
    {
        get => GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }
        
    public ICommand NavigateCommand
    {
        get => GetValue(NavigateCommandProperty);
        set => SetValue(NavigateCommandProperty, value);
    }
        
    public NavigationButton()
    {
        InitializeComponent();
    }
        
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}