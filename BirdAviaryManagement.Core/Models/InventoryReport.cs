using System.Collections.Generic;

namespace BirdAviaryManagement.Core.Models
{
    public class InventoryReport
    {
        public int TotalBirds { get; set; }

        public double AverageAge { get; set; }

        public int AvailableForSaleCount { get; set; }

        public List<Bird> SortedBirds { get; set; } = new List<Bird>();
    }
}