using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client.Avalonia.Controls.Navigation;


public partial class UserProfileControl : UserControl
{
    public static readonly StyledProperty<string> DisplayNameProperty =
        AvaloniaProperty.Register<UserProfileControl, string>(nameof(DisplayName));
        
    public static readonly StyledProperty<string> EmailProperty =
        AvaloniaProperty.Register<UserProfileControl, string>(nameof(Email));
        
    public static readonly StyledProperty<string> InitialsProperty =
        AvaloniaProperty.Register<UserProfileControl, string>(nameof(Initials));
        
    public static readonly StyledProperty<ICommand> SettingsCommandProperty =
        AvaloniaProperty.Register<UserProfileControl, ICommand>(nameof(SettingsCommand));
        
    public string DisplayName
    {
        get => GetValue(DisplayNameProperty);
        set => SetValue(DisplayNameProperty, value);
    }
    
    public string Email
    {
        get => GetValue(EmailProperty);
        set => SetValue(EmailProperty, value);
    }
    
    public string Initials
    {
        get => GetValue(InitialsProperty);
        set => SetValue(InitialsProperty, value);
    }
    
    public ICommand SettingsCommand
    {
        get => GetValue(SettingsCommandProperty);
        set => SetValue(SettingsCommandProperty, value);
    }
    
    public UserProfileControl()
    {
        InitializeComponent();
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
