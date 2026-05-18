using BirdAviary.Core.Models;

namespace BirdAviary.Core.Interfaces;

public interface IBirdRepository
{
    IReadOnlyList<Bird> GetAll();
    void Add(Bird bird);
    void AddRange(IEnumerable<Bird> birds);
    void Clear();
    int Count { get; }
    bool Exists(string ringId);
}
