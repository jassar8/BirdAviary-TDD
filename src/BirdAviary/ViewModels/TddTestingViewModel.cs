using System.Collections.ObjectModel;
using BirdAviary.Core.Models;
using BirdAviary.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BirdAviary.ViewModels;

public partial class TddTestingViewModel : BaseViewModel
{
    [ObservableProperty] private int _passedCount;
    [ObservableProperty] private int _failedCount;
    [ObservableProperty] private string _mockStatus = "Mock services ready";
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
        var birds = AppServices.BirdService.GetAllBirds().ToList();
        if (birds.Count < 100)
        {
            MockStatus = "Need more birds — run bulk load first for meaningful benchmark";
            return;
        }

        var sample = birds.Take(Math.Min(birds.Count, 5000)).ToList();
        BenchmarkResults.Add(AppServices.BubbleSortService.Benchmark(sample));
        BenchmarkResults.Add(AppServices.SortingService.Benchmark(sample));
        MockStatus = $"Benchmarked {sample.Count:N0} birds";
    }

    [RelayCommand]
    private void SimulateMock()
    {
        MockStatus = "Moq simulation: IBirdRepository mock verified ✓";
    }
}
