using System.Collections.ObjectModel;
using BirdAviary.Core.Models;
using BirdAviary.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BirdAviary.ViewModels;

public partial class InventoryViewModel : BaseViewModel
{
    [ObservableProperty] private string _searchQuery = string.Empty;
    [ObservableProperty] private int _totalCount;
    [ObservableProperty] private int _forSaleCount;
    [ObservableProperty] private double _avgAge;
    [ObservableProperty] private string _exportStatus = string.Empty;

    public ObservableCollection<Bird> Birds { get; } = [];

    public void Refresh() => ApplyFilter();

    partial void OnSearchQueryChanged(string value) => ApplyFilter();

    private void ApplyFilter()
    {
        var birds = AppServices.BirdService.GetInventoryBirds(
            string.IsNullOrWhiteSpace(SearchQuery) ? null : SearchQuery);

        Birds.Clear();
        foreach (var bird in birds)
            Birds.Add(bird);

        TotalCount = birds.Count;
        var forSale = 0;
        var ageSum = 0;
        foreach (var bird in birds)
        {
            if (bird.AvailableForSale) forSale++;
            ageSum += bird.Age;
        }

        ForSaleCount = forSale;
        AvgAge = birds.Count > 0 ? Math.Round((double)ageSum / birds.Count, 1) : 0;
    }

    [RelayCommand]
    private void Export()
    {
        ExportStatus = $"Export ready — {Birds.Count:N0} records (UI placeholder)";
    }

    public InventoryViewModel() => Refresh();
}
