using System.Collections.ObjectModel;
using BirdAviary.Core.Enums;
using BirdAviary.Core.Interfaces;
using BirdAviary.Core.Models;
using BirdAviary.Core.Services;
using BirdAviary.Core.Sorting;
using BirdAviary.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Moq;

namespace BirdAviary.ViewModels;

public partial class TddTestingViewModel : BaseViewModel
{
    [ObservableProperty] private int _passedCount;
    [ObservableProperty] private int _failedCount;
    [ObservableProperty] private string _mockStatus = "Moq ready — IHealthService can be mocked";
    [ObservableProperty] private bool _isRunning;

    public ObservableCollection<TestRunResult> TestResults { get; } = [];
    public ObservableCollection<SortBenchmarkResult> BenchmarkResults { get; } = [];

    [RelayCommand]
    private void RunTests()
    {
        IsRunning = true;
        TestResults.Clear();

        var results = AppServices.TestRunnerService.RunAllTests();
        foreach (var result in results)
            TestResults.Add(result);

        var (passed, failed) = AppServices.TestRunnerService.GetSummary(results);
        PassedCount = passed;
        FailedCount = failed;
        IsRunning = false;
    }

    [RelayCommand]
    private void RunBenchmark()
    {
        BenchmarkResults.Clear();
        var birds = AppServices.BirdService.GetAllBirds();
        if (birds.Count < 100)
        {
            MockStatus = "Run Bulk Load first for a meaningful benchmark";
            return;
        }

        var sample = new List<Bird>();
        var take = birds.Count < 5000 ? birds.Count : 5000;
        for (var i = 0; i < take; i++)
            sample.Add(birds[i]);

        BenchmarkResults.Add(AppServices.BubbleSortService.Benchmark(sample));
        BenchmarkResults.Add(AppServices.SortingService.Benchmark(sample));
        MockStatus = $"Benchmarked {sample.Count:N0} birds (Merge Sort target &lt; 9s @ 10k)";
    }

    [RelayCommand]
    private void SimulateMock()
    {
        var healthMock = new Mock<IHealthService>();
        healthMock.Setup(h => h.IsBirdHealthy("MOCK-APPROVED")).Returns(true);
        healthMock.Setup(h => h.IsBirdHealthy("MOCK-DENIED")).Returns(false);

        var repo = new BirdRepository();
        var service = new BirdService(repo, new ActivityService(), new MergeSortService(), healthMock.Object);

        var approved = new Bird
        {
            RingId = "MOCK-APPROVED",
            ColorMutation = "Lutino",
            HatchYear = 2022,
            Type = BirdType.Budgie,
            AvailableForSale = true,
            Status = BirdStatus.InAviary
        };
        service.TryAddBird(approved);

        var denied = new Bird
        {
            RingId = "MOCK-DENIED",
            ColorMutation = "Pied",
            HatchYear = 2021,
            Type = BirdType.Finch,
            AvailableForSale = true,
            Status = BirdStatus.InAviary
        };
        service.TryAddBird(denied);

        var stored = repo.GetAll();
        var approvedBird = stored.First(b => b.RingId == "MOCK-APPROVED");
        var deniedBird = stored.First(b => b.RingId == "MOCK-DENIED");

        MockStatus = approvedBird.AvailableForSale && !deniedBird.AvailableForSale
            ? "Moq IHealthService: approved ✓ sale ON | denied ✓ sale OFF"
            : "Moq simulation failed — check health mock setup";
    }
}
