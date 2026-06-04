using System.Collections.Generic;
using BirdAviaryManagement.Core.Models;

namespace BirdAviaryManagement.Core.Services
{
    public class ReportService
    {
        private readonly BirdSorter birdSorter = new BirdSorter();

        public int CountBirds(List<Bird> birds)
        {
            if (birds == null)
            {
                return 0;
            }

            return birds.Count;
        }

        public int CountAvailableForSale(List<Bird> birds)
        {
            if (birds == null)
            {
                return 0;
            }

            int count = 0;

            foreach (Bird bird in birds)
            {
                if (bird.IsAvailableForSale)
                {
                    count++;
                }
            }

            return count;
        }

        public double CalculateAverageAge(List<Bird> birds, int currentYear)
        {
            if (birds == null || birds.Count == 0)
            {
                return 0.0;
            }

            int totalAge = 0;

            foreach (Bird bird in birds)
            {
                int age = currentYear - bird.HatchYear;
                totalAge += age;
            }

            return (double)totalAge / birds.Count;
        }

        public InventoryReport CreateInventoryReport(List<Bird> birds, int currentYear)
        {
            List<Bird> safeBirds = birds ?? new List<Bird>();

            return new InventoryReport
            {
                TotalBirds = CountBirds(safeBirds),
                AverageAge = CalculateAverageAge(safeBirds, currentYear),
                AvailableForSaleCount = CountAvailableForSale(safeBirds),
                SortedBirds = birdSorter.SortByHatchYearDescending(safeBirds)
            };
        }
    }
}