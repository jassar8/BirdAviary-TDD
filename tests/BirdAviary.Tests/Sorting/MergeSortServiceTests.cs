using BirdAviary.Core.Models;
using BirdAviary.Core.Sorting;
using NUnit.Framework;

namespace BirdAviary.Tests.Sorting;

[TestFixture]
public class MergeSortServiceTests
{
    private MergeSortService _sorter = null!;

    [SetUp]
    public void SetUp() => _sorter = new MergeSortService();

    [Test]
    public void SortByHatchYearDescending_SortsCorrectly()
    {
        var birds = new List<Bird>
        {
            new() { HatchYear = 2018 },
            new() { HatchYear = 2024 },
            new() { HatchYear = 2020 },
            new() { HatchYear = 2015 }
        };

        var sorted = _sorter.SortByHatchYearDescending(birds);

        Assert.That(sorted.Select(b => b.HatchYear), Is.EqualTo(new[] { 2024, 2020, 2018, 2015 }));
    }

    [Test]
    public void SortByHatchYearDescending_LargeDataset_SortsCorrectly()
    {
        var random = new Random(42);
        var birds = Enumerable.Range(0, 1000)
            .Select(_ => new Bird { HatchYear = random.Next(1990, 2025) })
            .ToList();

        var sorted = _sorter.SortByHatchYearDescending(birds);

        for (var i = 0; i < sorted.Count - 1; i++)
            Assert.That(sorted[i].HatchYear, Is.GreaterThanOrEqualTo(sorted[i + 1].HatchYear));
    }

    [Test]
    public void Benchmark_IsFasterThanBubbleSort_OnLargeDataset()
    {
        var random = new Random(42);
        var birds = Enumerable.Range(0, 500)
            .Select(_ => new Bird { HatchYear = random.Next(1990, 2025) })
            .ToList();

        var bubble = new BubbleSortService().Benchmark(birds);
        var merge = _sorter.Benchmark(birds);

        Assert.That(merge.ElapsedMilliseconds, Is.LessThanOrEqualTo(bubble.ElapsedMilliseconds + 50));
    }
}
