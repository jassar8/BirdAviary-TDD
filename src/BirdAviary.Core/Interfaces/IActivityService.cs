using BirdAviary.Core.Models;

namespace BirdAviary.Core.Interfaces;

public interface IActivityService
{
    void Log(string description, string icon = "🐦");
    IReadOnlyList<ActivityItem> GetRecent(int count = 10);
}
