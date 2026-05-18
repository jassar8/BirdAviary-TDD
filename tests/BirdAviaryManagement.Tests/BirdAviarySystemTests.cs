using BirdAviary.Core.Enums;
using BirdAviary.Core.Interfaces;
using BirdAviary.Core.Models;
using BirdAviary.Core.Services;
using BirdAviary.Core.Sorting;
using Moq;
using NUnit.Framework;

namespace BirdAviaryManagement.Tests;

/// <summary>
/// Main system tests for Bird Aviary Management (TDD).
/// All tests use Arrange → Act → Assert with clear names for teacher review.
/// </summary>
[TestFixture]
public class BirdAviarySystemTests
{
    private Mock<IBirdRepository> _repositoryMock = null!;
    private Mock<IActivityService> _activityMock = null!;
    private Mock<IHealthService> _healthMock = null!;
    private BirdService _birdService = null!;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IBirdRepository>();
        _activityMock = new Mock<IActivityService>();
        _healthMock = new Mock<IHealthService>();

        _repositoryMock.Setup(r => r.Exists(It.IsAny<string>())).Returns(false);
        _repositoryMock.Setup(r => r.GetAll()).Returns([]);
        _healthMock.Setup(h => h.IsBirdHealthy(It.IsAny<string>())).Returns(true);

        _birdService = new BirdService(
            _repositoryMock.Object,
            _activityMock.Object,
            new MergeSortService(),
            _healthMock.Object);
    }

    // -------------------------------------------------------------------------
    // Bird validation tests
    // -------------------------------------------------------------------------

    [Test]
    public void Reject_InvalidHatchYear()
    {
        // Arrange
        var bird = CreateValidBird("RING-YEAR-01");
        bird.HatchYear = 1800;

        // Act
        var result = _birdService.ValidateBird(bird);

        // Assert
        Assert.That(result.IsValid, Is.False);
    }

    [Test]
    public void Reject_InvalidColor()
    {
        // Arrange
        var bird = CreateValidBird("RING-COLOR-01");
        bird.ColorMutation = "Lutino123";

        // Act
        var result = _birdService.ValidateBird(bird);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Does.Contain("Color/Mutation must contain only English or Hebrew letters."));
    }

    // -------------------------------------------------------------------------
    // Duplicate ring ID tests
    // -------------------------------------------------------------------------

    [Test]
    public void Reject_DuplicateRingId()
    {
        // Arrange
        _repositoryMock.Setup(r => r.Exists("RING-DUP-01")).Returns(true);
        var bird = CreateValidBird("RING-DUP-01");

        // Act
        var result = _birdService.ValidateBird(bird);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Does.Contain("Ring ID already exists."));
    }

    // -------------------------------------------------------------------------
    // Add bird tests
    // -------------------------------------------------------------------------

    [Test]
    public void AddBird_Successfully()
    {
        // Arrange
        Bird? storedBird = null;
        _repositoryMock.Setup(r => r.Add(It.IsAny<Bird>())).Callback<Bird>(b => storedBird = b);
        var bird = CreateValidBird("RING-ADD-01");

        // Act
        var result = _birdService.TryAddBird(bird);

        // Assert
        Assert.That(result.IsValid, Is.True);
        Assert.That(storedBird, Is.Not.Null);
        Assert.That(storedBird!.RingId, Is.EqualTo("RING-ADD-01"));
        _repositoryMock.Verify(r => r.Add(It.IsAny<Bird>()), Times.Once);
    }

    // -------------------------------------------------------------------------
    // Average age tests
    // -------------------------------------------------------------------------

    [Test]
    public void Calculate_AverageAge_Correctly()
    {
        // Arrange — two birds aged 2 and 4 years
        var currentYear = DateTime.Now.Year;
        var birds = new List<Bird>
        {
            new() { RingId = "A", ColorMutation = "Lutino", HatchYear = currentYear - 2 },
            new() { RingId = "B", ColorMutation = "Pied", HatchYear = currentYear - 4 }
        };
        _repositoryMock.Setup(r => r.GetAll()).Returns(birds);

        // Act
        var stats = _birdService.GetDashboardStats();

        // Assert
        Assert.That(stats.AverageAge, Is.EqualTo(3.0));
    }

    // -------------------------------------------------------------------------
    // Sorting tests
    // -------------------------------------------------------------------------

    [Test]
    public void Sort_ReturnsAValue()
    {
        // Arrange
        var birds = CreateSampleBirdsForSorting();
        SetupRepositoryWithBirds(birds);

        // Act
        var sorted = _birdService.GetSortedByHatchYearDescending();

        // Assert
        Assert.That(sorted, Is.Not.Null);
        Assert.That(sorted.Count, Is.GreaterThan(0));
    }

    [Test]
    public void Sort_DoesNotLoseRecords()
    {
        // Arrange
        var birds = CreateSampleBirdsForSorting();
        SetupRepositoryWithBirds(birds);

        // Act
        var sorted = _birdService.GetSortedByHatchYearDescending();

        // Assert
        Assert.That(sorted.Count, Is.EqualTo(birds.Count));
    }

    [Test]
    public void Sort_IsDescendingByHatchYear()
    {
        // Arrange
        var birds = CreateSampleBirdsForSorting();
        SetupRepositoryWithBirds(birds);

        // Act
        var sorted = _birdService.GetSortedByHatchYearDescending();

        // Assert
        for (var i = 0; i < sorted.Count - 1; i++)
            Assert.That(sorted[i].HatchYear, Is.GreaterThanOrEqualTo(sorted[i + 1].HatchYear));

        Assert.That(sorted[0].HatchYear, Is.EqualTo(2024));
        Assert.That(sorted[^1].HatchYear, Is.EqualTo(2018));
    }

    // -------------------------------------------------------------------------
    // Bulk load tests
    // -------------------------------------------------------------------------

    [Test]
    public void BulkLoad_CreatesTenThousandBirds()
    {
        // Arrange
        var repository = new BirdRepository();
        var healthMock = new Mock<IHealthService>();
        healthMock.Setup(h => h.IsBirdHealthy(It.IsAny<string>())).Returns(true);

        var service = new BirdService(
            repository,
            new ActivityService(),
            new MergeSortService(),
            healthMock.Object);

        // Act
        var created = service.GenerateBulkBirds(10_000);

        // Assert
        Assert.That(created, Is.EqualTo(10_000));
        Assert.That(repository.Count, Is.EqualTo(10_000));
    }

    // -------------------------------------------------------------------------
    // HealthService mocking tests
    // -------------------------------------------------------------------------

    [Test]
    public void MockHealthService_ApprovesBirdForSale()
    {
        // Arrange
        _healthMock.Setup(h => h.IsBirdHealthy("RING-HEALTH-OK")).Returns(true);
        Bird? storedBird = null;
        _repositoryMock.Setup(r => r.Add(It.IsAny<Bird>())).Callback<Bird>(b => storedBird = b);

        var bird = CreateValidBird("RING-HEALTH-OK");
        bird.AvailableForSale = true;

        // Act
        _birdService.TryAddBird(bird);

        // Assert
        Assert.That(storedBird!.AvailableForSale, Is.True);
        _healthMock.Verify(h => h.IsBirdHealthy("RING-HEALTH-OK"), Times.Once);
    }

    [Test]
    public void MockHealthService_DeniesBirdForSale()
    {
        // Arrange
        _healthMock.Setup(h => h.IsBirdHealthy("RING-HEALTH-NO")).Returns(false);
        Bird? storedBird = null;
        _repositoryMock.Setup(r => r.Add(It.IsAny<Bird>())).Callback<Bird>(b => storedBird = b);

        var bird = CreateValidBird("RING-HEALTH-NO");
        bird.AvailableForSale = true;

        // Act
        _birdService.TryAddBird(bird);

        // Assert
        Assert.That(storedBird!.AvailableForSale, Is.False);
        _healthMock.Verify(h => h.IsBirdHealthy("RING-HEALTH-NO"), Times.Once);
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    private void SetupRepositoryWithBirds(List<Bird> birds) =>
        _repositoryMock.Setup(r => r.GetAll()).Returns(birds);

    private static List<Bird> CreateSampleBirdsForSorting() =>
    [
        new() { RingId = "A", ColorMutation = "Lutino", HatchYear = 2018, Type = BirdType.Finch },
        new() { RingId = "B", ColorMutation = "Pied", HatchYear = 2024, Type = BirdType.Budgie },
        new() { RingId = "C", ColorMutation = "Pearl", HatchYear = 2020, Type = BirdType.Canary }
    ];

    private static Bird CreateValidBird(string ringId) => new()
    {
        RingId = ringId,
        ColorMutation = "Lutino",
        HatchYear = 2022,
        Type = BirdType.Cockatiel,
        Status = BirdStatus.InAviary
    };
}
