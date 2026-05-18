using System.Diagnostics;
using BirdAviary.Core.Interfaces;
using BirdAviary.Core.Models;

namespace BirdAviary.Core.Sorting;

/// <summary>
/// Refactored O(n log n) merge sort — production sorting implementation.
/// </summary>
public class MergeSortService : ISortingService
{
    public string AlgorithmName => "Merge Sort";

    public IList<Bird> SortByHatchYearDescending(IList<Bird> birds)
    {
        var list = birds.ToList();
        if (list.Count <= 1)
            return list;

        return MergeSort(list);
    }

    private static List<Bird> MergeSort(List<Bird> birds)
    {
        if (birds.Count <= 1)
            return birds;

        var mid = birds.Count / 2;
        var left = MergeSort(birds.Take(mid).ToList());
        var right = MergeSort(birds.Skip(mid).ToList());
        return Merge(left, right);
    }

    private static List<Bird> Merge(List<Bird> left, List<Bird> right)
    {
        var result = new List<Bird>(left.Count + right.Count);
        var i = 0;
        var j = 0;

        while (i < left.Count && j < right.Count)
        {
            if (left[i].HatchYear >= right[j].HatchYear)
                result.Add(left[i++]);
            else
                result.Add(right[j++]);
        }

        while (i < left.Count) result.Add(left[i++]);
        while (j < right.Count) result.Add(right[j++]);

        return result;
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
