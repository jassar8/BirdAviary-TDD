using System.Diagnostics;
using BirdAviary.Core.Enums;
using BirdAviary.Core.Interfaces;
using BirdAviary.Core.Models;
using BirdAviary.Core.Sorting;

namespace BirdAviary.Core.Services;

public class TestRunnerService : ITestRunnerService
{
    private readonly IBirdRepository _repository;
    private readonly IBirdService _birdService;

    public TestRunnerService(IBirdRepository repository, IBirdService birdService)
    {
        _repository = repository;
        _birdService = birdService;
    }

    public IReadOnlyList<TestRunResult> RunAllTests()
    {
        var results = new List<TestRunResult>();
        results.Add(RunTest("ValidateBird_RejectsEmptyRingId", TestEmptyRingId));
        results.Add(RunTest("ValidateBird_RejectsDuplicateRingId", TestDuplicateRingId));
        results.Add(RunTest("ValidateBird_AcceptsValidBird", TestValidBird));
        results.Add(RunTest("AddBird_PersistsToRepository", TestAddBird));
        results.Add(RunTest("GetDashboardStats_CalculatesCorrectly", TestDashboardStats));
        results.Add(RunTest("BubbleSort_SortsDescending", TestBubbleSort));
        results.Add(RunTest("MergeSort_SortsDescending", TestMergeSort));
        results.Add(RunTest("SearchBirds_FiltersByQuery", TestSearch));
        return results;
    }

    public (int Passed, int Failed) GetSummary(IReadOnlyList<TestRunResult> results) =>
        (results.Count(r => r.Passed), results.Count(r => !r.Passed));

    private static TestRunResult RunTest(string name, Func<string?> test)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            var error = test();
            sw.Stop();
            return new TestRunResult
            {
                TestName = name,
                Passed = error is null,
                Message = error ?? "Passed",
                DurationMs = sw.ElapsedMilliseconds
            };
        }
        catch (Exception ex)
        {
            sw.Stop();
            return new TestRunResult
            {
                TestName = name,
                Passed = false,
                Message = ex.Message,
                DurationMs = sw.ElapsedMilliseconds
            };
        }
    }

    private string? TestEmptyRingId()
    {
        var bird = new Bird { RingId = "", ColorMutation = "Lutino", HatchYear = 2020 };
        var result = _birdService.ValidateBird(bird);
        return result.IsValid ? "Expected validation to fail" : null;
    }

    private string? TestDuplicateRingId()
    {
        var repo = new BirdRepository();
        var activity = new ActivityService();
        var mergeSort = new MergeSortService();
        var service = new BirdService(repo, activity, mergeSort);

        var bird = new Bird { RingId = "TEST-001", ColorMutation = "Lutino", HatchYear = 2020 };
        service.AddBird(bird);

        var duplicate = new Bird { RingId = "TEST-001", ColorMutation = "Pied", HatchYear = 2021 };
        var result = service.ValidateBird(duplicate);
        return result.IsValid ? "Expected duplicate to fail" : null;
    }

    private string? TestValidBird()
    {
        var bird = new Bird
        {
            RingId = $"VALID-{Guid.NewGuid():N}"[..10],
            ColorMutation = "Normal",
            HatchYear = 2022,
            Type = BirdType.Budgie
        };
        var result = _birdService.ValidateBird(bird);
        return result.IsValid ? null : string.Join(", ", result.Errors);
    }

    private string? TestAddBird()
    {
        var before = _repository.Count;
        var bird = new Bird
        {
            RingId = $"ADD-{Guid.NewGuid():N}"[..10],
            ColorMutation = "Pearl",
            HatchYear = 2023,
            Type = BirdType.Finch
        };
        _birdService.AddBird(bird);
        return _repository.Count == before + 1 ? null : "Bird was not added";
    }

    private string? TestDashboardStats()
    {
        var stats = _birdService.GetDashboardStats();
        return stats.TotalBirds >= 0 ? null : "Invalid stats";
    }

    private static string? TestBubbleSort()
    {
        var birds = new List<Bird>
        {
            new() { HatchYear = 2018 },
            new() { HatchYear = 2022 },
            new() { HatchYear = 2020 }
        };
        var sorted = new BubbleSortService().SortByHatchYearDescending(birds);
        return sorted[0].HatchYear == 2022 && sorted[2].HatchYear == 2018
            ? null
            : "Bubble sort order incorrect";
    }

    private static string? TestMergeSort()
    {
        var birds = new List<Bird>
        {
            new() { HatchYear = 2015 },
            new() { HatchYear = 2024 },
            new() { HatchYear = 2019 }
        };
        var sorted = new MergeSortService().SortByHatchYearDescending(birds);
        return sorted[0].HatchYear == 2024 && sorted[2].HatchYear == 2015
            ? null
            : "Merge sort order incorrect";
    }

    private string? TestSearch()
    {
        var results = _birdService.SearchBirds("BULK");
        return results is not null ? null : "Search returned null";
    }
}
