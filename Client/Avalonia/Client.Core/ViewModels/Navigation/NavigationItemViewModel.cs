using CommunityToolkit.Mvvm.Input;

namespace Client.Core.ViewModels.Navigation;

public partial class NavigationItemViewModel : ViewModelBase
{
    private string _title;
    private string _iconData;
    private bool _isActive;
        
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }
        
    public string IconData
    {
        get => _iconData;
        set => SetProperty(ref _iconData, value);
    }
        
    public bool IsActive
    {
        get => _isActive;
        set => SetProperty(ref _isActive, value);
    }
        
    public IRelayCommand NavigateCommand { get; }
        
    public NavigationItemViewModel(string title, string iconData, IRelayCommand navigateCommand, bool isActive = false)
    {
        _title = title;
        _iconData = iconData;
        _isActive = isActive;
        NavigateCommand = navigateCommand;
    }
}