using BirdAviary.Core.Models;
using BirdAviary.Core.Sorting;
using NUnit.Framework;

namespace BirdAviary.Tests.Sorting;

[TestFixture]
public class BubbleSortServiceTests
{
    private BubbleSortService _sorter = null!;

    [SetUp]
    public void SetUp() => _sorter = new BubbleSortService();

    [Test]
    public void SortByHatchYearDescending_SortsCorrectly()
    {
        var birds = new List<Bird>
        {
            new() { HatchYear = 2018, RingId = "A" },
            new() { HatchYear = 2024, RingId = "B" },
            new() { HatchYear = 2020, RingId = "C" },
            new() { HatchYear = 2015, RingId = "D" }
        };

        var sorted = _sorter.SortByHatchYearDescending(birds);

        Assert.That(sorted[0].HatchYear, Is.EqualTo(2024));
        Assert.That(sorted[1].HatchYear, Is.EqualTo(2020));
        Assert.That(sorted[2].HatchYear, Is.EqualTo(2018));
        Assert.That(sorted[3].HatchYear, Is.EqualTo(2015));
    }

    [Test]
    public void SortByHatchYearDescending_EmptyList_ReturnsEmpty()
    {
        var sorted = _sorter.SortByHatchYearDescending([]);
        Assert.That(sorted, Is.Empty);
    }

    [Test]
    public void SortByHatchYearDescending_SingleItem_ReturnsSame()
    {
        var birds = new List<Bird> { new() { HatchYear = 2020 } };
        var sorted = _sorter.SortByHatchYearDescending(birds);
        Assert.That(sorted, Has.Count.EqualTo(1));
    }

    [Test]
    public void Benchmark_ReturnsElapsedTime()
    {
        var birds = Enumerable.Range(0, 100)
            .Select(i => new Bird { HatchYear = 2000 + (i % 25) })
            .ToList();

        var result = _sorter.Benchmark(birds);

        Assert.That(result.AlgorithmName, Is.EqualTo("Bubble Sort"));
        Assert.That(result.ItemCount, Is.EqualTo(100));
        Assert.That(result.ElapsedMilliseconds, Is.GreaterThanOrEqualTo(0));
    }
}
