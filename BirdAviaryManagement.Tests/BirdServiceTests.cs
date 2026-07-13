using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using BirdAviaryManagement.Core.Models;
using BirdAviaryManagement.Core.Services;

namespace BirdAviaryManagement.Tests
{
    [TestClass]
    public class BirdServiceTests
    {
        [TestMethod]
        public void AddBird_WithValidBird_ShouldAddBird()
        {
            // Arrange
            BirdService service = new BirdService();

            Bird bird = new Bird
            {
                RingId = "1001",
                Type = BirdType.Budgie,
                ColorMutation = "Blue",
                HatchYear = 2022,
                Status = BirdStatus.InAviary,
                IsAvailableForSale = false
            };

            // Act
            bool result = service.AddBird(bird);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, service.GetAllBirds().Count);
        }

        // Unit test verifying duplicate Ring IDs cannot be added.
        [TestMethod]
        public void AddBird_WithDuplicateRingId_ShouldNotAddBird()
        {
            // Arrange
            BirdService service = new BirdService();

            Bird firstBird = new Bird
            {
                RingId = "1001",
                Type = BirdType.Budgie,
                ColorMutation = "Blue",
                HatchYear = 2022,
                Status = BirdStatus.InAviary,
                IsAvailableForSale = false
            };

            Bird secondBird = new Bird
            {
                RingId = "1001",
                Type = BirdType.Finch,
                ColorMutation = "White",
                HatchYear = 2021,
                Status = BirdStatus.InAviary,
                IsAvailableForSale = false
            };

            // Act
            bool firstResult = service.AddBird(firstBird);
            bool secondResult = service.AddBird(secondBird);

            // Assert
            Assert.IsTrue(firstResult);
            Assert.IsFalse(secondResult);
            Assert.AreEqual(1, service.GetAllBirds().Count);
        }

        [TestMethod]
        public void AddBird_WithRingIdContainingLetters_ShouldNotAddBird()
        {
            // Arrange
            BirdService service = new BirdService();

            Bird bird = new Bird
            {
                RingId = "B001",
                Type = BirdType.Budgie,
                ColorMutation = "Blue",
                HatchYear = 2022,
                Status = BirdStatus.InAviary,
                IsAvailableForSale = false
            };

            // Act
            bool result = service.AddBird(bird);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(0, service.GetAllBirds().Count);
        }

        [TestMethod]
        public void AddBird_WithEmptyRingId_ShouldNotAddBird()
        {
            // Arrange
            BirdService service = new BirdService();

            Bird bird = new Bird
            {
                RingId = "",
                Type = BirdType.Cockatiel,
                ColorMutation = "Yellow",
                HatchYear = 2020,
                Status = BirdStatus.InAviary,
                IsAvailableForSale = false
            };

            // Act
            bool result = service.AddBird(bird);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(0, service.GetAllBirds().Count);
        }

        [TestMethod]
        public void AddBird_WithInvalidColorContainingNumbers_ShouldNotAddBird()
        {
            // Arrange
            BirdService service = new BirdService();

            Bird bird = new Bird
            {
                RingId = "1002",
                Type = BirdType.Canary,
                ColorMutation = "Blue123",
                HatchYear = 2023,
                Status = BirdStatus.InAviary,
                IsAvailableForSale = false
            };

            // Act
            bool result = service.AddBird(bird);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(0, service.GetAllBirds().Count);
        }

        [TestMethod]
        public void AddBird_WithFutureHatchYear_ShouldNotAddBird()
        {
            // Arrange
            BirdService service = new BirdService();

            Bird bird = new Bird
            {
                RingId = "1003",
                Type = BirdType.Lovebird,
                ColorMutation = "Green",
                HatchYear = 3000,
                Status = BirdStatus.InAviary,
                IsAvailableForSale = false
            };

            // Act
            bool result = service.AddBird(bird);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(0, service.GetAllBirds().Count);
        }

        [TestMethod]
        public void AddBird_WithTooOldHatchYear_ShouldNotAddBird()
        {
            // Arrange
            BirdService service = new BirdService();

            Bird bird = new Bird
            {
                RingId = "1004",
                Type = BirdType.Finch,
                ColorMutation = "Gray",
                HatchYear = 1999,
                Status = BirdStatus.InAviary,
                IsAvailableForSale = false
            };

            // Act
            bool result = service.AddBird(bird);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(0, service.GetAllBirds().Count);
        }

        [TestMethod]
        public void AddBird_WithMinimumHatchYear2000_ShouldAddBird()
        {
            // Arrange
            BirdService service = new BirdService();

            Bird bird = new Bird
            {
                RingId = "2000",
                Type = BirdType.Budgie,
                ColorMutation = "Blue",
                HatchYear = Bird.MinimumHatchYear,
                Status = BirdStatus.InAviary,
                IsAvailableForSale = false
            };

            // Act
            bool result = service.AddBird(bird);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, service.GetAllBirds().Count);
            Assert.AreEqual(Bird.MinimumHatchYear, service.GetAllBirds()[0].HatchYear);
        }

        [TestMethod]
        public void AddBird_WithHebrewColor_ShouldAddBird()
        {
            // Arrange
            BirdService service = new BirdService();

            Bird bird = new Bird
            {
                RingId = "1005",
                Type = BirdType.Budgie,
                ColorMutation = "כחול",
                HatchYear = 2022,
                Status = BirdStatus.InAviary,
                IsAvailableForSale = false
            };

            // Act
            bool result = service.AddBird(bird);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, service.GetAllBirds().Count);
        }

        [TestMethod]
        public void AddBird_WithNonEnglishHebrewColor_ShouldNotAddBird()
        {
            // Arrange
            BirdService service = new BirdService();

            Bird bird = new Bird
            {
                RingId = "1006",
                Type = BirdType.Budgie,
                ColorMutation = "أزرق",
                HatchYear = 2022,
                Status = BirdStatus.InAviary,
                IsAvailableForSale = false
            };

            // Act
            bool result = service.AddBird(bird);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(0, service.GetAllBirds().Count);
        }

    [TestMethod]
public void AddBirds_WithValidBirdList_ShouldAddAllBirds()
{
    // Arrange
    BirdService service = new BirdService();
    BulkBirdGenerator generator = new BulkBirdGenerator();

    List<Bird> birds = generator.GenerateBirds(10000);

    // Act
    int addedCount = service.AddBirds(birds);

    // Assert
    Assert.AreEqual(10000, addedCount);
    Assert.AreEqual(10000, service.GetAllBirds().Count);
    }
}
}