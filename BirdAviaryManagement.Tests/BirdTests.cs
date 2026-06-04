using Microsoft.VisualStudio.TestTools.UnitTesting;
using BirdAviaryManagement.Core.Models;

namespace BirdAviaryManagement.Tests
{
    [TestClass]
    public class BirdTests
    {
        [TestMethod]
        public void CreateBird_WithValidData_ShouldCreateBirdSuccessfully()
        {
            // Arrange
            string ringId = "1001";
            BirdType type = BirdType.Budgie;
            string colorMutation = "Blue";
            int hatchYear = 2022;
            BirdStatus status = BirdStatus.InAviary;
            bool isAvailableForSale = false;

            // Act
            Bird bird = new Bird
            {
                RingId = ringId,
                Type = type,
                ColorMutation = colorMutation,
                HatchYear = hatchYear,
                Status = status,
                IsAvailableForSale = isAvailableForSale
            };

            // Assert
            Assert.IsNotNull(bird);
            Assert.AreEqual(ringId, bird.RingId);
            Assert.AreEqual(type, bird.Type);
            Assert.AreEqual(colorMutation, bird.ColorMutation);
            Assert.AreEqual(hatchYear, bird.HatchYear);
            Assert.AreEqual(status, bird.Status);
            Assert.AreEqual(isAvailableForSale, bird.IsAvailableForSale);
        }
    }
}