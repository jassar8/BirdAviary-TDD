using BirdAviary.Core.Models;

namespace BirdAviary.Core.Interfaces;

public interface IBirdService
{
    ValidationResult ValidateBird(Bird bird);
    void AddBird(Bird bird);
    DashboardStats GetDashboardStats();
    IReadOnlyList<Bird> GetAllBirds();
    IReadOnlyList<Bird> SearchBirds(string query);
    IReadOnlyList<Bird> GetSortedByHatchYearDescending();
    int GenerateBulkBirds(int count, IProgress<int>? progress = null);
}
