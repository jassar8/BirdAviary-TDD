namespace BirdAviary.Core.Services;

public class HealthService : Interfaces.IHealthService
{
    private static readonly Random Random = new();

    public bool IsBirdHealthy(string ringId) =>
        Random.Next(2) == 0;
}
