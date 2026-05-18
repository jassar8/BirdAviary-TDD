using BirdAviary.Core.Interfaces;
using BirdAviary.Core.Models;

namespace BirdAviary.Core.Services;

public class ActivityService : IActivityService
{
    private readonly List<ActivityItem> _activities = [];

    public void Log(string description, string icon = "🐦")
    {
        _activities.Insert(0, new ActivityItem
        {
            Description = description,
            Timestamp = DateTime.Now,
            Icon = icon
        });

        if (_activities.Count > 100)
            _activities.RemoveAt(_activities.Count - 1);
    }

    public IReadOnlyList<ActivityItem> GetRecent(int count = 10) =>
        _activities.Take(count).ToList();
}
