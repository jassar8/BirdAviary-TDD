using BirdAviary.Core.Enums;
using BirdAviary.Core.Helpers;
using BirdAviary.Core.Interfaces;
using BirdAviary.Core.Models;

namespace BirdAviary.Core.Services;

public class BirdService : IBirdService
{
    private readonly IBirdRepository _repository;
    private readonly IActivityService _activityService;
    private readonly ISortingService _sortingService;
    private readonly IHealthService _healthService;
    private static readonly Random Random = new();

    private static readonly BirdType[] Types =
        [BirdType.Cockatiel, BirdType.Finch, BirdType.Budgie, BirdType.Canary, BirdType.Lovebird];

    private static readonly string[] Colors =
        ["Lutino", "Pied", "Albino", "Normal", "Pearl", "Cinnamon", "צהוב", "כחול"];

    private static readonly BirdStatus[] Statuses =
        [BirdStatus.InAviary, BirdStatus.InAviary, BirdStatus.Isolation, BirdStatus.Sold];

    public BirdService(
        IBirdRepository repository,
        IActivityService activityService,
        ISortingService sortingService,
        IHealthService healthService)
    {
        _repository = repository;
        _activityService = activityService;
        _sortingService = sortingService;
        _healthService = healthService;
    }

    public ValidationResult ValidateBird(Bird bird)
    {
        var errors = new List<string>();

        if (!ValidationHelper.IsValidRingId(bird.RingId))
            errors.Add("Ring ID is required (at least 3 characters).");
        else if (_repository.Exists(bird.RingId.Trim()))
            errors.Add("Ring ID already exists.");

        if (!ValidationHelper.IsValidColorMutation(bird.ColorMutation))
            errors.Add("Color/Mutation must contain only English or Hebrew letters.");

        if (!ValidationHelper.IsValidHatchYear(bird.HatchYear))
            errors.Add($"Hatch year must be between 1990 and {DateTime.Now.Year}.");

        return errors.Count == 0
            ? ValidationResult.Success()
            : ValidationResult.Failure(errors.ToArray());
    }

    public ValidationResult TryAddBird(Bird bird)
    {
        bird.RingId = bird.RingId.Trim();
        bird.ColorMutation = bird.ColorMutation.Trim();

        var validation = ValidateBird(bird);
        if (!validation.IsValid)
            return validation;

        ApplySaleEligibility(bird);
        _repository.Add(bird);
        _activityService.Log($"Added bird {bird.RingId} ({bird.Type})", "➕");
        return ValidationResult.Success();
    }

    public void AddBird(Bird bird)
    {
        var result = TryAddBird(bird);
        if (!result.IsValid)
            throw new InvalidOperationException(string.Join(" ", result.Errors));
    }

    internal void ApplySaleEligibility(Bird bird)
    {
        if (!bird.AvailableForSale)
            return;

        bird.AvailableForSale = _healthService.IsBirdHealthy(bird.RingId);
    }

    public DashboardStats GetDashboardStats()
    {
        var birds = _repository.GetAll();
        var total = birds.Count;
        var forSale = 0;
        var isolation = 0;
        var ageSum = 0;

        foreach (var bird in birds)
        {
            if (bird.AvailableForSale) forSale++;
            if (bird.Status == BirdStatus.Isolation) isolation++;
            ageSum += bird.Age;
        }

        return new DashboardStats
        {
            TotalBirds = total,
            AvailableForSale = forSale,
            AverageAge = total > 0 ? Math.Round((double)ageSum / total, 1) : 0,
            BirdsInIsolation = isolation
        };
    }

    public IReadOnlyList<Bird> GetAllBirds() => _repository.GetAll();

    public IReadOnlyList<Bird> SearchBirds(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return _repository.GetAll();

        var results = new List<Bird>();
        foreach (var bird in _repository.GetAll())
        {
            if (MatchesSearch(bird, query))
                results.Add(bird);
        }

        return results;
    }

    public IReadOnlyList<Bird> GetSortedByHatchYearDescending()
    {
        var birds = CopyBirdList(_repository.GetAll());
        return ToReadOnlyList(_sortingService.SortByHatchYearDescending(birds));
    }

    public IReadOnlyList<Bird> GetInventoryBirds(string? searchQuery = null)
    {
        var birds = string.IsNullOrWhiteSpace(searchQuery)
            ? CopyBirdList(_repository.GetAll())
            : CopyBirdList(SearchBirds(searchQuery));

        return ToReadOnlyList(_sortingService.SortByHatchYearDescending(birds));
    }

    public int GenerateBulkBirds(int count, IProgress<int>? progress = null)
    {
        var birds = new List<Bird>(count);
        var currentYear = DateTime.Now.Year;

        for (var i = 0; i < count; i++)
        {
            var ringId = $"BULK-{Guid.NewGuid():N}"[..12].ToUpperInvariant();
            var bird = new Bird
            {
                RingId = ringId,
                Type = Types[Random.Next(Types.Length)],
                ColorMutation = Colors[Random.Next(Colors.Length)],
                HatchYear = Random.Next(2015, currentYear + 1),
                Status = Statuses[Random.Next(Statuses.Length)],
                AvailableForSale = false
            };

            bird.AvailableForSale = Random.Next(2) == 0 && _healthService.IsBirdHealthy(ringId);
            birds.Add(bird);

            if (i % 500 == 0)
                progress?.Report((int)((double)i / count * 100));
        }

        _repository.AddRange(birds);
        progress?.Report(100);
        _activityService.Log($"Bulk loaded {count:N0} birds", "📦");
        return count;
    }

    private static bool MatchesSearch(Bird bird, string query)
    {
        return bird.RingId.Contains(query, StringComparison.OrdinalIgnoreCase)
               || bird.ColorMutation.Contains(query, StringComparison.OrdinalIgnoreCase)
               || bird.Type.ToString().Contains(query, StringComparison.OrdinalIgnoreCase);
    }

    private static List<Bird> CopyBirdList(IReadOnlyList<Bird> birds)
    {
        var copy = new List<Bird>(birds.Count);
        foreach (var bird in birds)
            copy.Add(bird);
        return copy;
    }

    private static IReadOnlyList<Bird> ToReadOnlyList(IList<Bird> birds)
    {
        var list = new List<Bird>(birds.Count);
        foreach (var bird in birds)
            list.Add(bird);
        return list;
    }
}
