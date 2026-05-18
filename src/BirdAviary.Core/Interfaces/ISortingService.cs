using BirdAviary.Core.Models;

namespace BirdAviary.Core.Interfaces;

public interface ISortingService
{
    string AlgorithmName { get; }
    IList<Bird> SortByHatchYearDescending(IList<Bird> birds);
    SortBenchmarkResult Benchmark(IList<Bird> birds);
}
