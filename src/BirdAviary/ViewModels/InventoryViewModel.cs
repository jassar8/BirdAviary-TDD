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
        var birds = string.IsNullOrWhiteSpace(SearchQuery)
            ? AppServices.BirdService.GetSortedByHatchYearDescending()
            : AppServices.BirdService.SearchBirds(SearchQuery)
                .OrderByDescending(b => b.HatchYear)
                .ToList();

        Birds.Clear();
        foreach (var bird in birds)
            Birds.Add(bird);

        TotalCount = birds.Count;
        ForSaleCount = birds.Count(b => b.AvailableForSale);
        AvgAge = birds.Count > 0 ? Math.Round(birds.Average(b => b.Age), 1) : 0;
    }

    [RelayCommand]
    private void Export()
    {
        ExportStatus = $"Export ready — {Birds.Count:N0} records (UI placeholder)";
    }

    public InventoryViewModel() => Refresh();
}
