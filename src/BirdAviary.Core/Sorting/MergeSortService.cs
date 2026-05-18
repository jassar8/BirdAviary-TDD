using System.Diagnostics;
using BirdAviary.Core.Interfaces;
using BirdAviary.Core.Models;

namespace BirdAviary.Core.Sorting;

/// <summary>
/// Production O(n log n) merge sort. Previous bubble-sort implementation is preserved below as comments.
/// </summary>
public class MergeSortService : ISortingService
{
    public string AlgorithmName => "Merge Sort";

    /*
     * --- Previous Bubble Sort (O(n²)) — replaced by merge sort ---
     *
     * public IList<Bird> SortByHatchYearDescending(IList<Bird> birds)
     * {
     *     var list = birds.ToList();
     *     var n = list.Count;
     *     for (var i = 0; i < n - 1; i++)
     *     {
     *         for (var j = 0; j < n - i - 1; j++)
     *         {
     *             if (list[j].HatchYear < list[j + 1].HatchYear)
     *                 (list[j], list[j + 1]) = (list[j + 1], list[j]);
     *         }
     *     }
     *     return list;
     * }
     */

    public IList<Bird> SortByHatchYearDescending(IList<Bird> birds)
    {
        if (birds.Count <= 1)
            return birds.Count == 0 ? [] : [birds[0]];

        var array = new Bird[birds.Count];
        for (var i = 0; i < birds.Count; i++)
            array[i] = birds[i];

        var buffer = new Bird[array.Length];
        MergeSortInPlace(array, buffer, 0, array.Length);
        return array;
    }

    private static void MergeSortInPlace(Bird[] array, Bird[] buffer, int start, int end)
    {
        if (end - start <= 1)
            return;

        var mid = start + (end - start) / 2;
        MergeSortInPlace(array, buffer, start, mid);
        MergeSortInPlace(array, buffer, mid, end);
        Merge(array, buffer, start, mid, end);
    }

    private static void Merge(Bird[] array, Bird[] buffer, int start, int mid, int end)
    {
        var left = start;
        var right = mid;
        var index = start;

        while (left < mid && right < end)
        {
            if (array[left].HatchYear >= array[right].HatchYear)
                buffer[index++] = array[left++];
            else
                buffer[index++] = array[right++];
        }

        while (left < mid)
            buffer[index++] = array[left++];

        while (right < end)
            buffer[index++] = array[right++];

        for (var i = start; i < end; i++)
            array[i] = buffer[i];
    }

    public SortBenchmarkResult Benchmark(IList<Bird> birds)
    {
        var copy = new Bird[birds.Count];
        for (var i = 0; i < birds.Count; i++)
            copy[i] = birds[i];

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
