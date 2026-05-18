using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BirdAviary.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    [ObservableProperty]
    private BaseViewModel _currentViewModel;

    [ObservableProperty]
    private string _currentPageTitle = "Dashboard";

    [ObservableProperty]
    private int _selectedNavIndex;

    public DashboardViewModel Dashboard { get; }
    public AddBirdViewModel AddBird { get; }
    public BulkLoadViewModel BulkLoad { get; }
    public InventoryViewModel Inventory { get; }
    public TddTestingViewModel TddTesting { get; }

    public MainViewModel()
    {
        Dashboard = new DashboardViewModel();
        AddBird = new AddBirdViewModel();
        BulkLoad = new BulkLoadViewModel();
        Inventory = new InventoryViewModel();
        TddTesting = new TddTestingViewModel();
        _currentViewModel = Dashboard;
    }

    [RelayCommand]
    private void Navigate(string page)
    {
        CurrentViewModel = page switch
        {
            "Dashboard" => Dashboard,
            "AddBird" => AddBird,
            "BulkLoad" => BulkLoad,
            "Inventory" => Inventory,
            "TddTesting" => TddTesting,
            _ => Dashboard
        };
        CurrentPageTitle = page switch
        {
            "Dashboard" => "Dashboard",
            "AddBird" => "Add Bird",
            "BulkLoad" => "Bulk Load",
            "Inventory" => "Inventory Report",
            "TddTesting" => "TDD Testing",
            _ => "Dashboard"
        };
        SelectedNavIndex = page switch
        {
            "Dashboard" => 0,
            "AddBird" => 1,
            "BulkLoad" => 2,
            "Inventory" => 3,
            "TddTesting" => 4,
            _ => 0
        };

        if (CurrentViewModel is DashboardViewModel dash) dash.Refresh();
        if (CurrentViewModel is InventoryViewModel inv) inv.Refresh();
    }
}
