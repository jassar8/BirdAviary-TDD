using BirdAviary.Core.Enums;
using BirdAviary.Core.Interfaces;
using BirdAviary.Core.Models;
using BirdAviary.Core.Services;
using BirdAviary.Core.Sorting;
using Moq;
using NUnit.Framework;

namespace BirdAviary.Tests.Services;

[TestFixture]
public class BirdServiceTests
{
    private Mock<IBirdRepository> _repositoryMock = null!;
    private Mock<IActivityService> _activityMock = null!;
    private BirdService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IBirdRepository>();
        _activityMock = new Mock<IActivityService>();
        _repositoryMock.Setup(r => r.Exists(It.IsAny<string>())).Returns(false);
        _repositoryMock.Setup(r => r.GetAll()).Returns([]);
        _service = new BirdService(_repositoryMock.Object, _activityMock.Object, new MergeSortService());
    }

    [Test]
    public void ValidateBird_EmptyRingId_ReturnsInvalid()
    {
        var bird = new Bird { RingId = "", ColorMutation = "Lutino", HatchYear = 2020 };
        var result = _service.ValidateBird(bird);
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Does.Contain("Ring ID is required."));
    }

    [Test]
    public void ValidateBird_DuplicateRingId_ReturnsInvalid()
    {
        _repositoryMock.Setup(r => r.Exists("RING-001")).Returns(true);
        var bird = new Bird { RingId = "RING-001", ColorMutation = "Lutino", HatchYear = 2020 };
        var result = _service.ValidateBird(bird);
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Does.Contain("Ring ID already exists."));
    }

    [Test]
    public void ValidateBird_EmptyColor_ReturnsInvalid()
    {
        var bird = new Bird { RingId = "RING-002", ColorMutation = "", HatchYear = 2020 };
        var result = _service.ValidateBird(bird);
        Assert.That(result.IsValid, Is.False);
    }

    [Test]
    public void ValidateBird_InvalidHatchYear_ReturnsInvalid()
    {
        var bird = new Bird { RingId = "RING-003", ColorMutation = "Normal", HatchYear = 1800 };
        var result = _service.ValidateBird(bird);
        Assert.That(result.IsValid, Is.False);
    }

    [Test]
    public void ValidateBird_ValidBird_ReturnsValid()
    {
        var bird = new Bird
        {
            RingId = "RING-004",
            ColorMutation = "Pearl",
            HatchYear = 2022,
            Type = BirdType.Cockatiel
        };
        var result = _service.ValidateBird(bird);
        Assert.That(result.IsValid, Is.True);
    }

    [Test]
    public void AddBird_ValidBird_CallsRepositoryAndLogsActivity()
    {
        var bird = new Bird
        {
            RingId = "RING-005",
            ColorMutation = "Pied",
            HatchYear = 2021,
            Type = BirdType.Budgie
        };
        _service.AddBird(bird);
        _repositoryMock.Verify(r => r.Add(bird), Times.Once);
        _activityMock.Verify(a => a.Log(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GetDashboardStats_ReturnsCorrectCounts()
    {
        var birds = new List<Bird>
        {
            new() { RingId = "A", ColorMutation = "X", HatchYear = 2020, AvailableForSale = true, Status = BirdStatus.Healthy },
            new() { RingId = "B", ColorMutation = "Y", HatchYear = 2018, AvailableForSale = false, Status = BirdStatus.Isolation },
            new() { RingId = "C", ColorMutation = "Z", HatchYear = 2022, AvailableForSale = true, Status = BirdStatus.Isolation }
        };
        _repositoryMock.Setup(r => r.GetAll()).Returns(birds);
        var stats = _service.GetDashboardStats();
        Assert.That(stats.TotalBirds, Is.EqualTo(3));
        Assert.That(stats.AvailableForSale, Is.EqualTo(2));
        Assert.That(stats.BirdsInIsolation, Is.EqualTo(2));
    }

    [Test]
    public void SearchBirds_WithQuery_FiltersResults()
    {
        var birds = new List<Bird>
        {
            new() { RingId = "ABC-001", ColorMutation = "Lutino", HatchYear = 2020, Type = BirdType.Finch },
            new() { RingId = "XYZ-002", ColorMutation = "Pied", HatchYear = 2021, Type = BirdType.Budgie }
        };
        _repositoryMock.Setup(r => r.GetAll()).Returns(birds);
        var results = _service.SearchBirds("ABC");
        Assert.That(results, Has.Count.EqualTo(1));
        Assert.That(results[0].RingId, Is.EqualTo("ABC-001"));
    }
}
