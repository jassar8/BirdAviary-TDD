using System.Collections.ObjectModel;
using BirdAviary.Core.Models;
using BirdAviary.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BirdAviary.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
    [ObservableProperty] private int _totalBirds;
    [ObservableProperty] private int _availableForSale;
    [ObservableProperty] private double _averageAge;
    [ObservableProperty] private int _birdsInIsolation;

    public ObservableCollection<ActivityItem> RecentActivities { get; } = [];
    public ObservableCollection<ChartBar> TypeDistribution { get; } = [];

    public void Refresh()
    {
        var stats = AppServices.BirdService.GetDashboardStats();
        TotalBirds = stats.TotalBirds;
        AvailableForSale = stats.AvailableForSale;
        AverageAge = stats.AverageAge;
        BirdsInIsolation = stats.BirdsInIsolation;

        RecentActivities.Clear();
        foreach (var item in AppServices.ActivityService.GetRecent(8))
            RecentActivities.Add(item);

        TypeDistribution.Clear();
        var birds = AppServices.BirdService.GetAllBirds();
        var max = birds.GroupBy(b => b.Type).Max(g => g.Count());
        if (max == 0) max = 1;

        foreach (var group in birds.GroupBy(b => b.Type).OrderByDescending(g => g.Count()))
        {
            TypeDistribution.Add(new ChartBar
            {
                Label = group.Key.ToString(),
                Value = group.Count(),
                MaxValue = max
            });
        }
    }

    public DashboardViewModel() => Refresh();
}

public class ChartBar
{
    public string Label { get; set; } = string.Empty;
    public int Value { get; set; }
    public int MaxValue { get; set; }
    public double BarWidth => MaxValue > 0 ? (double)Value / MaxValue * 200 : 0;
}
