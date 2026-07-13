using System.Collections.Generic;

namespace BirdAviaryManagement.Core.Models
{
    // Result object returned by ReportService after generating an inventory summary.
    public class InventoryReport
    {
        public int TotalBirds { get; set; }

        // Average age of all birds relative to the report year.
        public double AverageAge { get; set; }

        public int AvailableForSaleCount { get; set; }

        // Birds sorted by hatch year descending (newest first).
        public List<Bird> SortedBirds { get; set; } = new List<Bird>();
    }
}
