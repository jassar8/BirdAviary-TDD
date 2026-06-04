namespace BirdAviaryManagement.Core.Services
{
    public interface IHealthService
    {
        bool IsBirdHealthyForSale(string ringId);
    }
}