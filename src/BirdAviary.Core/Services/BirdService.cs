using BirdAviary.Core.Enums;
using BirdAviary.Core.Interfaces;
using BirdAviary.Core.Models;

namespace BirdAviary.Core.Services;

public class BirdService : IBirdService
{
    private readonly IBirdRepository _repository;
    private readonly IActivityService _activityService;
    private readonly ISortingService _sortingService;
    private static readonly Random Random = new();

    private static readonly BirdType[] Types =
        [BirdType.Cockatiel, BirdType.Finch, BirdType.Budgie, BirdType.Canary, BirdType.Lovebird];

    private static readonly string[] Colors =
        ["Lutino", "Pied", "Albino", "Normal", "Pearl", "Cinnamon", "Opaline", "Spangle"];

    private static readonly BirdStatus[] Statuses =
        [BirdStatus.Healthy, BirdStatus.Sick, BirdStatus.Isolation, BirdStatus.Breeding];

    public BirdService(
        IBirdRepository repository,
        IActivityService activityService,
        ISortingService sortingService)
    {
        _repository = repository;
        _activityService = activityService;
        _sortingService = sortingService;
    }

    public ValidationResult ValidateBird(Bird bird)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(bird.RingId))
            errors.Add("Ring ID is required.");
        else if (_repository.Exists(bird.RingId))
            errors.Add("Ring ID already exists.");

        if (string.IsNullOrWhiteSpace(bird.ColorMutation))
            errors.Add("Color/Mutation is required.");

        var currentYear = DateTime.Now.Year;
        if (bird.HatchYear < 1990 || bird.HatchYear > currentYear)
            errors.Add($"Hatch year must be between 1990 and {currentYear}.");

        return errors.Count == 0
            ? ValidationResult.Success()
            : ValidationResult.Failure(errors.ToArray());
    }

    public void AddBird(Bird bird)
    {
        var validation = ValidateBird(bird);
        if (!validation.IsValid)
            throw new InvalidOperationException(string.Join(" ", validation.Errors));

        _repository.Add(bird);
        _activityService.Log($"Added bird {bird.RingId} ({bird.Type})", "➕");
    }

    public DashboardStats GetDashboardStats()
    {
        var birds = _repository.GetAll();
        return new DashboardStats
        {
            TotalBirds = birds.Count,
            AvailableForSale = birds.Count(b => b.AvailableForSale),
            AverageAge = birds.Count > 0 ? Math.Round(birds.Average(b => b.Age), 1) : 0,
            BirdsInIsolation = birds.Count(b => b.Status == BirdStatus.Isolation)
        };
    }

    public IReadOnlyList<Bird> GetAllBirds() => _repository.GetAll();

    public IReadOnlyList<Bird> SearchBirds(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return _repository.GetAll();

        return _repository.GetAll()
            .Where(b =>
                b.RingId.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                b.ColorMutation.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                b.Type.ToString().Contains(query, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public IReadOnlyList<Bird> GetSortedByHatchYearDescending()
    {
        var birds = _repository.GetAll().ToList();
        return _sortingService.SortByHatchYearDescending(birds).ToList();
    }

    public int GenerateBulkBirds(int count, IProgress<int>? progress = null)
    {
        var birds = new List<Bird>(count);
        var currentYear = DateTime.Now.Year;

        for (var i = 0; i < count; i++)
        {
            birds.Add(new Bird
            {
                RingId = $"BULK-{Guid.NewGuid():N}"[..12].ToUpperInvariant(),
                Type = Types[Random.Next(Types.Length)],
                ColorMutation = Colors[Random.Next(Colors.Length)],
                HatchYear = Random.Next(2015, currentYear + 1),
                Status = Statuses[Random.Next(Statuses.Length)],
                AvailableForSale = Random.Next(2) == 0
            });

            if (i % 500 == 0)
                progress?.Report((int)((double)i / count * 100));
        }

        _repository.AddRange(birds);
        progress?.Report(100);
        _activityService.Log($"Bulk loaded {count:N0} birds", "📦");
        return count;
    }
}
