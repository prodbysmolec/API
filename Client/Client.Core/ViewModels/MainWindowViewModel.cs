using System.Collections.ObjectModel;
using Client.Core.ViewModels.Navigation;
using CommunityToolkit.Mvvm.Input;

namespace Client.Core.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase _currentView;
    private string _currentPageTitle = "Dashboard";
    private string _currentPagePath = "Übersicht";
    
    public ViewModelBase CurrentView
    {
        get => _currentView;
        set => SetProperty(ref _currentView, value);
    }
    
    public string CurrentPageTitle
    {
        get => _currentPageTitle;
        set => SetProperty(ref _currentPageTitle, value);
    }
    
    public string CurrentPagePath
    {
        get => _currentPagePath;
        set => SetProperty(ref _currentPagePath, value);
    }
    
    public ObservableCollection<NavigationItemViewModel> NavigationItems { get; } = new ObservableCollection<NavigationItemViewModel>();
    
    public UserProfileViewModel UserProfile { get; }
    
    public IRelayCommand SearchCommand { get; }
    public IRelayCommand NotificationsCommand { get; }
    public IRelayCommand HelpCommand { get; }
    
    public MainWindowViewModel()
    {
        // Initialize commands
        SearchCommand = new RelayCommand(ExecuteSearch);
        NotificationsCommand = new RelayCommand(OpenNotifications);
        HelpCommand = new RelayCommand(OpenHelp);
        
        // Initialize user profile
        UserProfile = new UserProfileViewModel("Demo User", "user@example.com");
        
        // Initialize navigation items with commands
        NavigationItems.Add(new NavigationItemViewModel("Dashboard", 
            "M2 10.5C2 9.4 2.4 8.5 3.3 7.7C4.1 6.9 5.2 6.5 6.3 6.5H17.6C18.7 6.5 19.7 6.9 20.6 7.7C21.4 8.5 21.9 9.4 21.9 10.5V17.9C21.9 19.1 21.4 20 20.6 20.8C19.8 21.6 18.8 22 17.7 22H6.3C5.1 22 4.2 21.6 3.3 20.8C2.5 20 2 19 2 17.9V10.5ZM17.6 8.8H6.3C5.7 8.8 5.2 9.1 4.8 9.4C4.3 9.8 4.1 10.2 4.1 10.8V17.9C4.1 18.5 4.4 19 4.7 19.4C5.1 19.9 5.5 20.1 6.1 20.1H17.6C18.2 20.1 18.7 19.8 19.1 19.5C19.6 19.1 19.8 18.6 19.8 18.1V10.8C19.8 10.2 19.5 9.7 19.2 9.3C18.8 9 18.3 8.8 17.7 8.8H17.6Z M2 3.9C2 3.4 2.2 2.9 2.5 2.5C2.9 2.2 3.4 2 3.9 2H20C20.5 2 21 2.2 21.4 2.5C21.8 2.9 22 3.4 22 3.9C22 4.4 21.8 4.9 21.4 5.2C21 5.6 20.5 5.8 20 5.8H3.9C3.4 5.8 2.9 5.6 2.5 5.2C2.2 4.9 2 4.4 2 3.9Z", 
            new RelayCommand(() => NavigateTo("Dashboard", "Übersicht")), 
            true));
            
        NavigationItems.Add(new NavigationItemViewModel("Produkte", 
            "M20 2H4C3 2 2 2.9 2 4V7.01C2 7.73 2.43 8.35 3 8.7V20C3 21.1 4.1 22 5 22H19C19.9 22 21 21.1 21 20V8.7C21.57 8.35 22 7.73 22 7.01V4C22 2.9 21 2 20 2ZM19 20H5V9H19V20ZM20 7H4V4H20V7Z M13 12H16V14H13V12Z M9 12H12V14H9V12Z M13 15H16V17H13V15Z M9 15H12V17H9V15Z", 
            new RelayCommand(() => NavigateTo("Produkte", "Liste"))));
            
        NavigationItems.Add(new NavigationItemViewModel("Kunden", 
            "M12 12C14.21 12 16 10.21 16 8C16 5.79 14.21 4 12 4C9.79 4 8 5.79 8 8C8 10.21 9.79 12 12 12ZM12 6C13.1 6 14 6.9 14 8C14 9.1 13.1 10 12 10C10.9 10 10 9.1 10 8C10 6.9 10.9 6 12 6ZM12 13C9.33 13 4 14.34 4 17V20H20V17C20 14.34 14.67 13 12 13ZM12 15C14.67 15 18 16.25 18 17V18H6V17C6 16.25 9.33 15 12 15Z", 
            new RelayCommand(() => NavigateTo("Kunden", "Übersicht"))));
            
        NavigationItems.Add(new NavigationItemViewModel("Berichte", 
            "M19 3H5C3.9 3 3 3.9 3 5V19C3 20.1 3.9 21 5 21H19C20.1 21 21 20.1 21 19V5C21 3.9 20.1 3 19 3ZM19 19H5V5H19V19ZM7 10H9V17H7V10ZM11 7H13V17H11V7ZM15 13H17V17H15V13Z", 
            new RelayCommand(() => NavigateTo("Berichte", "Zusammenfassung"))));
            
        NavigationItems.Add(new NavigationItemViewModel("Einstellungen", 
            "M12 1L3 5V11C3 16.55 6.84 21.74 12 23C17.16 21.74 21 16.55 21 11V5L12 1ZM19 11C19 15.52 16.02 19.69 12 20.93C7.98 19.69 5 15.52 5 11V6.3L12 3.19L19 6.3V11ZM7.41 11.59L6 13L10 17L18 9L16.59 7.58L10 14.17L7.41 11.59Z", 
            new RelayCommand(() => NavigateTo("Einstellungen", "Allgemein"))));
        
        // Set initial view (you'd need to create these view models)
        // CurrentView = new DashboardViewModel();
    }
    
    private void NavigateTo(string pageTitle, string pagePath)
    {
        // Update active navigation item
        foreach (var item in NavigationItems)
        {
            item.IsActive = item.Title == pageTitle;
        }
        
        // Update page title
        CurrentPageTitle = pageTitle;
        CurrentPagePath = pagePath;
        
        // Update current view based on navigation
        // TODO: You would implement this logic based on your application needs
        // Example: CurrentView = _viewModelFactory.CreateViewModel(pageTitle);
    }
    
    private void ExecuteSearch()
    {
        // Implement search functionality
    }
    
    private void OpenNotifications()
    {
        // Open notifications panel
    }
    
    private void OpenHelp()
    {
        // Open help documentation
    }
}