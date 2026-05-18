using BirdAviary.Core.Models;
using BirdAviary.Core.Services;
using NUnit.Framework;

namespace BirdAviary.Tests.Repositories;

[TestFixture]
public class BirdRepositoryTests
{
    private BirdRepository _repository = null!;

    [SetUp]
    public void SetUp() => _repository = new BirdRepository();

    [Test]
    public void Add_IncreasesCount()
    {
        _repository.Add(new Bird { RingId = "R001", ColorMutation = "Normal", HatchYear = 2020 });
        Assert.That(_repository.Count, Is.EqualTo(1));
    }

    [Test]
    public void Exists_ReturnsTrueForExistingRingId()
    {
        _repository.Add(new Bird { RingId = "R002", ColorMutation = "Lutino", HatchYear = 2021 });
        Assert.That(_repository.Exists("R002"), Is.True);
        Assert.That(_repository.Exists("r002"), Is.True);
    }

    [Test]
    public void Exists_ReturnsFalseForMissingRingId()
    {
        Assert.That(_repository.Exists("MISSING"), Is.False);
    }

    [Test]
    public void Clear_RemovesAllBirds()
    {
        _repository.Add(new Bird { RingId = "R003", ColorMutation = "Pied", HatchYear = 2019 });
        _repository.Clear();
        Assert.That(_repository.Count, Is.EqualTo(0));
    }
}
