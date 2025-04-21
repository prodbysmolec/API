using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client.Core.ViewModels.Navigation;

namespace Client.Avalonia.Controls.Navigation;

public partial class NavigationBar : UserControl
{
    public static readonly StyledProperty<IEnumerable<NavigationItemViewModel>> NavigationItemsProperty =
        AvaloniaProperty.Register<NavigationBar, IEnumerable<NavigationItemViewModel>>(nameof(NavigationItems));
            
    public static readonly StyledProperty<UserProfileViewModel> UserProfileProperty =
        AvaloniaProperty.Register<NavigationBar, UserProfileViewModel>(nameof(UserProfile));
            
    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<NavigationBar, string>(nameof(Title), "Merchandise System");
            
    public static readonly StyledProperty<string> LogoSourceProperty =
        AvaloniaProperty.Register<NavigationBar, string>(nameof(LogoSource), "/Assets/Bilder/Logo.png");
            
    public IEnumerable<NavigationItemViewModel> NavigationItems
    {
        get => GetValue(NavigationItemsProperty);
        set => SetValue(NavigationItemsProperty, value);
    }
        
    public UserProfileViewModel UserProfile
    {
        get => GetValue(UserProfileProperty);
        set => SetValue(UserProfileProperty, value);
    }
        
    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
        
    public string LogoSource
    {
        get => GetValue(LogoSourceProperty);
        set => SetValue(LogoSourceProperty, value);
    }
        
    public NavigationBar()
    {
        InitializeComponent();
    }
        
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}