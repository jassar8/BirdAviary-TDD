using BirdAviary.Core.Models;

namespace BirdAviary.Core.Interfaces;

public interface IBirdService
{
    ValidationResult ValidateBird(Bird bird);
    ValidationResult TryAddBird(Bird bird);
    DashboardStats GetDashboardStats();
    IReadOnlyList<Bird> GetAllBirds();
    IReadOnlyList<Bird> SearchBirds(string query);
    IReadOnlyList<Bird> GetSortedByHatchYearDescending();
    IReadOnlyList<Bird> GetInventoryBirds(string? searchQuery = null);
    int GenerateBulkBirds(int count, IProgress<int>? progress = null);
}
