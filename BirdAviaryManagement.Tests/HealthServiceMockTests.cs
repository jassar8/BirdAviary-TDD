using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BirdAviaryManagement.Core.Models;
using BirdAviaryManagement.Core.Services;

namespace BirdAviaryManagement.Tests
{
    // Mocking tests: isolate BirdService business logic from the real HealthService.
    [TestClass]
    public class HealthServiceMockTests
    {
        [TestMethod]
        public void UpdateSaleAvailability_WhenHealthServiceReturnsTrue_ShouldSetBirdAvailableForSale()
        {
            // Arrange
            // Mocking the health service to isolate the business logic.
            Mock<IHealthService> healthServiceMock = new Mock<IHealthService>();

            healthServiceMock
                .Setup(service => service.IsBirdHealthyForSale("1001"))
                .Returns(true);

            BirdService birdService = new BirdService(healthServiceMock.Object);

            Bird bird = new Bird
            {
                RingId = "1001",
                Type = BirdType.Budgie,
                ColorMutation = "Blue",
                HatchYear = 2022,
                Status = BirdStatus.InAviary,
                IsAvailableForSale = false
            };

            birdService.AddBird(bird);

            // Act
            bool result = birdService.UpdateSaleAvailability("1001");

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(bird.IsAvailableForSale);
        }

        [TestMethod]
        public void UpdateSaleAvailability_WhenHealthServiceReturnsFalse_ShouldNotSetBirdAvailableForSale()
        {
            // Arrange
            Mock<IHealthService> healthServiceMock = new Mock<IHealthService>();

            healthServiceMock
                .Setup(service => service.IsBirdHealthyForSale("1002"))
                .Returns(false);

            BirdService birdService = new BirdService(healthServiceMock.Object);

            Bird bird = new Bird
            {
                RingId = "1002",
                Type = BirdType.Finch,
                ColorMutation = "White",
                HatchYear = 2021,
                Status = BirdStatus.InAviary,
                IsAvailableForSale = false
            };

            birdService.AddBird(bird);

            // Act
            bool result = birdService.UpdateSaleAvailability("1002");

            // Assert
            Assert.IsFalse(result);
            Assert.IsFalse(bird.IsAvailableForSale);
        }

        [TestMethod]
        public void UpdateSaleAvailability_WithUnknownRingId_ShouldReturnFalse()
        {
            // Arrange
            Mock<IHealthService> healthServiceMock = new Mock<IHealthService>();
            BirdService birdService = new BirdService(healthServiceMock.Object);

            // Act
            bool result = birdService.UpdateSaleAvailability("UNKNOWN");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpdateSaleAvailability_ShouldCallHealthServiceOnce()
        {
            // Arrange
            Mock<IHealthService> healthServiceMock = new Mock<IHealthService>();

            healthServiceMock
                .Setup(service => service.IsBirdHealthyForSale("1003"))
                .Returns(true);

            BirdService birdService = new BirdService(healthServiceMock.Object);

            Bird bird = new Bird
            {
                RingId = "1003",
                Type = BirdType.Cockatiel,
                ColorMutation = "Yellow",
                HatchYear = 2023,
                Status = BirdStatus.InAviary,
                IsAvailableForSale = false
            };

            birdService.AddBird(bird);

            // Act
            birdService.UpdateSaleAvailability("1003");

            // Assert
            healthServiceMock.Verify(
                service => service.IsBirdHealthyForSale("1003"),
                Times.Once
            );
        }
    }
}