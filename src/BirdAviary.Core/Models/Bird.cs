using BirdAviary.Core.Enums;

namespace BirdAviary.Core.Models;

public class Bird
{
    public string RingId { get; set; } = string.Empty;
    public BirdType Type { get; set; }
    public string ColorMutation { get; set; } = string.Empty;
    public int HatchYear { get; set; }
    public BirdStatus Status { get; set; } = BirdStatus.InAviary;
    public bool AvailableForSale { get; set; }

    public int Age => DateTime.Now.Year - HatchYear;
}
