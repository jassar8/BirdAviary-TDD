namespace BirdAviaryManagement.Core.Models
{
    // Core data model representing a single bird in the aviary inventory.
    public class Bird
    {
        // Hatch year must be at least 2000 according to assignment validation rules.
        public const int MinimumHatchYear = 2000;

        // Unique numeric identifier for the bird (digits only).
        public string RingId { get; set; } = string.Empty;

        public BirdType Type { get; set; }

        // Color/mutation text limited to English or Hebrew letters.
        public string ColorMutation { get; set; } = string.Empty;

        public int HatchYear { get; set; }

        public BirdStatus Status { get; set; }

        public bool IsAvailableForSale { get; set; }

        // Display helper used by the DataGrid binding.
        public string AvailableForSaleText => IsAvailableForSale ? "True" : "False";

        // Distinguishes bulk-generated birds from manually entered ones (Clear Bulk).
        public bool IsBulkGenerated { get; set; }
    }
}
