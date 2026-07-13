using System;

namespace BirdAviaryManagement.Core.Services
{
    // Real health service implementation used by the application at runtime.
    // Simulates an external dependency with a random healthy/unhealthy result.
    public class HealthService : IHealthService
    {
        private readonly Random random = new Random();

        // Calls the simulated external health check for the given Ring ID.
        public bool IsBirdHealthyForSale(string ringId)
        {
            return random.Next(0, 2) == 1;
        }
    }
}
