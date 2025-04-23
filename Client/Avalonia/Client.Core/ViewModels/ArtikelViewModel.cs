using System.Collections.ObjectModel;
using Artikelsystem.Shared.DTOs.Artikel.Enums;
using Artikelsystem.Shared.DTOs.Artikel.Request;
using Artikelsystem.Shared.DTOs.Artikel.Response;
using Client.Core.Services.ApiClient;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Client.Core.ViewModels;

public partial class ArtikelViewModel : ViewModelBase
{
    private readonly ArtikelApiService _artikelService;

    // Haupt-Collection für die Artikel
    [ObservableProperty]
    private ObservableCollection<ArtikelDto> _artikelDtos = new();

    // Pagination-Eigenschaften
    [ObservableProperty]
    private int _currentPage = 1;

    [ObservableProperty]
    private int _itemsPerPage = 10;

    [ObservableProperty]
    private int _totalPages;

    [ObservableProperty]
    private int _totalRecords;

    // Status-Eigenschaften
    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    // Filter-Eigenschaften
    [ObservableProperty]
    private string _nameFilter = string.Empty;

    [ObservableProperty]
    private decimal? _minPreis;

    [ObservableProperty]
    private decimal? _maxPreis;

    [ObservableProperty]
    private int? _minMenge;

    [ObservableProperty]
    private int? _maxMenge;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsArtikelSelected))]
    private ArtikelDto _selectedArtikel;
    
    // Hilfseigenschaft zur Anzeige/Ausblendung der Detailansicht
    public bool IsArtikelSelected => SelectedArtikel != null;
    
    partial void OnSelectedArtikelChanged(ArtikelDto value)
    {
        if (value != null)
        {
            // Artikel-Details laden
            Task.Run(() => LoadArtikelDetailsAsync(value.Id));
        }
    }
    
    [RelayCommand]
    private async Task ReloadArtikelDetailsAsync()
    {
        if (SelectedArtikel != null)
        {
            await LoadArtikelDetailsAsync(SelectedArtikel.Id);
        }
    }

    private async Task? LoadArtikelDetailsAsync(int valueId)
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            // Detaillierte Artikelinformationen laden
            var detailedArtikel = await _artikelService.GetArtikelByIdAsync(valueId);
            
            if (detailedArtikel != null)
            {
                // Den ausgewählten Artikel mit den detaillierten Daten aktualisieren
                SelectedArtikel = detailedArtikel;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Fehler beim Laden der Artikeldetails: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [ObservableProperty]
    private ArtikelStatus? _selectedStatus;

    [ObservableProperty]
    private bool? _unterMindestbestand;

    [ObservableProperty]
    private bool? _ueberMaximalbestand;

    [ObservableProperty]
    private decimal? _minDurchschnittlicherEinzelpreis;

    [ObservableProperty]
    private decimal? _maxDurchschnittlicherEinzelpreis;

    [ObservableProperty]
    private decimal? _minLagerwert;

    [ObservableProperty]
    private decimal? _maxLagerwert;

    [ObservableProperty]
    private string _sortBy = "Name";

    [ObservableProperty]
    private bool _sortDesc;

    // Verfügbare Status für das Dropdown
    public List<ArtikelStatus> VerfuegbareStatus => Enum.GetValues(typeof(ArtikelStatus))
        .Cast<ArtikelStatus>()
        .ToList();

    // Verfügbare Sortierfelder für Dropdown
    public List<string> SortierOptionen => new List<string>
    {
        "Name",
        "Preis",
        "Menge",
        "Mindestbestand",
        "Maximalbestand"
    };

    public ArtikelViewModel(ArtikelApiService artikelService)
    {
        _artikelService = artikelService ?? throw new ArgumentNullException(nameof(artikelService));

        // Commands initialisieren
        LoadArtikelCommand = new RelayCommand(async () => await LoadArtikelAsync());
        ApplyFiltersCommand = new RelayCommand(async () => await ApplyFiltersAsync());
        ClearFiltersCommand = new RelayCommand(ClearFilters);
        NextPageCommand = new RelayCommand(async () => await GoToNextPageAsync(), () => CurrentPage < TotalPages);
        PreviousPageCommand = new RelayCommand(async () => await GoToPreviousPageAsync(), () => CurrentPage > 1);
        FirstPageCommand = new RelayCommand(async () => await GoToFirstPageAsync(), () => CurrentPage > 1);
        LastPageCommand = new RelayCommand(async () => await GoToLastPageAsync(), () => CurrentPage < TotalPages);

        // Initial beim Start Artikel laden
        Task.Run(() => LoadArtikelAsync());
    }

    // Commands
    public IRelayCommand LoadArtikelCommand { get; }
    public IRelayCommand ApplyFiltersCommand { get; }
    public IRelayCommand ClearFiltersCommand { get; }
    public IRelayCommand NextPageCommand { get; }
    public IRelayCommand PreviousPageCommand { get; }
    public IRelayCommand FirstPageCommand { get; }
    public IRelayCommand LastPageCommand { get; }

    // Methoden
    private async Task LoadArtikelAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            var request = CreateFilterRequest();
            var result = await _artikelService.GetAllArtikelAsync(request);

            // Die ObservableCollection aktualisieren
            ArtikelDtos = new ObservableCollection<ArtikelDto>(result.Items);

            CurrentPage = result.Page;
            TotalPages = result.TotalPages;
            TotalRecords = result.TotalRecords;

            // Commands aktualisieren, damit CanExecute neu ausgewertet wird
            RefreshPaginationCommands();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Fehler beim Laden der Artikel: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task ApplyFiltersAsync()
    {
        CurrentPage = 1; // Zurück zur ersten Seite
        await LoadArtikelAsync();
    }

    private void ClearFilters()
    {
        NameFilter = string.Empty;
        MinPreis = null;
        MaxPreis = null;
        MinMenge = null;
        MaxMenge = null;
        SelectedStatus = null;
        UnterMindestbestand = null;
        UeberMaximalbestand = null;
        MinDurchschnittlicherEinzelpreis = null;
        MaxDurchschnittlicherEinzelpreis = null;
        MinLagerwert = null;
        MaxLagerwert = null;
        SortBy = "Name";
        SortDesc = false;

        // Da wir RelayCommand verwenden, können wir es direkt ausführen
        ApplyFiltersCommand.Execute(null);
    }

    private GetAllArtikelRequest CreateFilterRequest()
    {
        return new GetAllArtikelRequest
        {
            Page = CurrentPage,
            RecordsPerPage = ItemsPerPage,
            NameContains = string.IsNullOrWhiteSpace(NameFilter) ? null : NameFilter,
            MinPreis = MinPreis,
            MaxPreis = MaxPreis,
            MinMenge = MinMenge,
            MaxMenge = MaxMenge,
            StatusId = SelectedStatus.HasValue ? (int)SelectedStatus.Value : null,
            UnterMindestbestand = UnterMindestbestand,
            UeberMaximalbestand = UeberMaximalbestand,
            MinDurchschnittlicherEinzelpreis = MinDurchschnittlicherEinzelpreis,
            MaxDurchschnittlicherEinzelpreis = MaxDurchschnittlicherEinzelpreis,
            MinLagerwert = MinLagerwert,
            MaxLagerwert = MaxLagerwert,
            SortBy = SortBy,
            SortDesc = SortDesc
        };
    }

    private async Task GoToNextPageAsync()
    {
        if (CurrentPage < TotalPages)
        {
            CurrentPage++;
            await LoadArtikelAsync();
        }
    }

    private async Task GoToPreviousPageAsync()
    {
        if (CurrentPage > 1)
        {
            CurrentPage--;
            await LoadArtikelAsync();
        }
    }

    private async Task GoToFirstPageAsync()
    {
        CurrentPage = 1;
        await LoadArtikelAsync();
    }

    private async Task GoToLastPageAsync()
    {
        CurrentPage = TotalPages;
        await LoadArtikelAsync();
    }

    // Aktualisiert den CanExecute-Status aller Paginierungskommandos
    private void RefreshPaginationCommands()
    {
        // Da wir IRelayCommand verwenden, können wir direkt NotifyCanExecuteChanged aufrufen
        (NextPageCommand).NotifyCanExecuteChanged();
        (PreviousPageCommand).NotifyCanExecuteChanged();
        (FirstPageCommand).NotifyCanExecuteChanged();
        (LastPageCommand).NotifyCanExecuteChanged();
    }
    // Die INotifyPropertyChanged-Implementierung wird automatisch durch 
    // die [ObservableProperty]-Attribute und den partial-Modifier generiert
}