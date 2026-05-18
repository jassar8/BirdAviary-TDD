using BirdAviary.Core.Interfaces;
using BirdAviary.Core.Models;

namespace BirdAviary.Core.Services;

public class BirdRepository : IBirdRepository
{
    private readonly List<Bird> _birds = [];

    public IReadOnlyList<Bird> GetAll() => _birds.AsReadOnly();

    public void Add(Bird bird) => _birds.Add(bird);

    public void AddRange(IEnumerable<Bird> birds) => _birds.AddRange(birds);

    public void Clear() => _birds.Clear();

    public int Count => _birds.Count;

    public bool Exists(string ringId) =>
        _birds.Any(b => b.RingId.Equals(ringId, StringComparison.OrdinalIgnoreCase));
}
