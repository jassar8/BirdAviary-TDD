namespace BirdAviary.Core.Models;

public class ActivityItem
{
    public string Description { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public string Icon { get; set; } = "🐦";
}
