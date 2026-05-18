using BirdAviary.Core.Interfaces;
using BirdAviary.Core.Services;
using BirdAviary.Core.Sorting;

namespace BirdAviary.Services;

public static class AppServices
{
    private static bool _initialized;

    public static IBirdRepository Repository { get; private set; } = null!;
    public static IActivityService ActivityService { get; private set; } = null!;
    public static IHealthService HealthService { get; private set; } = null!;
    public static ISortingService SortingService { get; private set; } = null!;
    public static ISortingService BubbleSortService { get; private set; } = null!;
    public static IBirdService BirdService { get; private set; } = null!;
    public static ITestRunnerService TestRunnerService { get; private set; } = null!;

    public static void Initialize()
    {
        if (_initialized) return;

        Repository = new BirdRepository();
        ActivityService = new ActivityService();
        HealthService = new HealthService();
        SortingService = new MergeSortService();
        BubbleSortService = new BubbleSortService();
        BirdService = new BirdService(Repository, ActivityService, SortingService, HealthService);
        TestRunnerService = new TestRunnerService(Repository, BirdService);

        SeedSampleData();
        _initialized = true;
    }

    private static void SeedSampleData()
    {
        var samples = new[]
        {
            ("RING-1001", Core.Enums.BirdType.Cockatiel, "Lutino", 2022, Core.Enums.BirdStatus.InAviary, true),
            ("RING-1002", Core.Enums.BirdType.Finch, "Normal", 2021, Core.Enums.BirdStatus.InAviary, true),
            ("RING-1003", Core.Enums.BirdType.Budgie, "Pied", 2020, Core.Enums.BirdStatus.Isolation, false),
            ("RING-1004", Core.Enums.BirdType.Canary, "Yellow", 2023, Core.Enums.BirdStatus.Sold, false),
            ("RING-1005", Core.Enums.BirdType.Lovebird, "Peach", 2019, Core.Enums.BirdStatus.InAviary, false),
        };

        foreach (var (ringId, type, color, year, status, sale) in samples)
        {
            var bird = new Core.Models.Bird
            {
                RingId = ringId,
                Type = type,
                ColorMutation = color,
                HatchYear = year,
                Status = status,
                AvailableForSale = sale
            };

            if (sale)
                bird.AvailableForSale = HealthService.IsBirdHealthy(ringId);

            Repository.Add(bird);
        }

        ActivityService.Log("System initialized with sample data", "🚀");
        ActivityService.Log("Dashboard ready", "📊");
    }
}
