namespace BirdAviaryManagement.Core.Services
{
    // Abstraction for an external health-check dependency.
    // Enables dependency injection and Moq mocking in unit tests.
    public interface IHealthService
    {
        // Returns true when the bird is healthy enough to be marked available for sale.
        bool IsBirdHealthyForSale(string ringId);
    }
}
