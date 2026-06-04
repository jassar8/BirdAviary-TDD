using System;

namespace BirdAviaryManagement.Core.Services
{
    public class HealthService : IHealthService
    {
        private readonly Random random = new Random();

        public bool IsBirdHealthyForSale(string ringId)
        {
            return random.Next(0, 2) == 1;
        }
    }
}