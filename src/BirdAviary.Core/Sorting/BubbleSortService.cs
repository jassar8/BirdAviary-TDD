using System.Diagnostics;
using BirdAviary.Core.Interfaces;
using BirdAviary.Core.Models;

namespace BirdAviary.Core.Sorting;

/// <summary>
/// Initial O(n²) implementation — replaced by MergeSortService for production use.
/// Retained for TDD benchmarking and educational comparison.
/// </summary>
public class BubbleSortService : ISortingService
{
    public string AlgorithmName => "Bubble Sort";

    public IList<Bird> SortByHatchYearDescending(IList<Bird> birds)
    {
        var list = birds.ToList();
        var n = list.Count;

        for (var i = 0; i < n - 1; i++)
        {
            for (var j = 0; j < n - i - 1; j++)
            {
                if (list[j].HatchYear < list[j + 1].HatchYear)
                    (list[j], list[j + 1]) = (list[j + 1], list[j]);
            }
        }

        return list;
    }

    public SortBenchmarkResult Benchmark(IList<Bird> birds)
    {
        var copy = birds.ToList();
        var sw = Stopwatch.StartNew();
        SortByHatchYearDescending(copy);
        sw.Stop();

        return new SortBenchmarkResult
        {
            AlgorithmName = AlgorithmName,
            ElapsedMilliseconds = sw.ElapsedMilliseconds,
            ItemCount = birds.Count
        };
    }
}
