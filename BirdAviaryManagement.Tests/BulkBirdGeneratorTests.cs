using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using BirdAviaryManagement.Core.Models;
using BirdAviaryManagement.Core.Services;

namespace BirdAviaryManagement.Tests
{
    [TestClass]
    public class BulkBirdGeneratorTests
    {
        [TestMethod]
        public void GenerateBirds_WithCount10000_ShouldReturn10000Birds()
        {
            // Arrange
            BulkBirdGenerator generator = new BulkBirdGenerator();

            // Act
            List<Bird> birds = generator.GenerateBirds(10000);

            // Assert
            Assert.IsNotNull(birds);
            Assert.AreEqual(10000, birds.Count);
        }

        [TestMethod]
        public void GenerateBirds_ShouldCreateUniqueRingIds()
        {
            // Arrange
            BulkBirdGenerator generator = new BulkBirdGenerator();

            // Act
            List<Bird> birds = generator.GenerateBirds(10000);

            // Assert
            HashSet<string> ringIds = new HashSet<string>();

            foreach (Bird bird in birds)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(bird.RingId));
                Assert.IsTrue(RingIdValidator.IsValid(bird.RingId));
                Assert.IsTrue(ringIds.Add(bird.RingId));
            }
        }

        [TestMethod]
        public void GenerateBirds_ShouldCreateValidHatchYears()
        {
            // Arrange
            BulkBirdGenerator generator = new BulkBirdGenerator();
            int currentYear = System.DateTime.Now.Year;

            // Act
            List<Bird> birds = generator.GenerateBirds(10000);

            // Assert
            foreach (Bird bird in birds)
            {
                Assert.IsTrue(bird.HatchYear >= Bird.MinimumHatchYear);
                Assert.IsTrue(bird.HatchYear <= currentYear);
            }
        }

        [TestMethod]
        public void GenerateBirds_ShouldCreateValidColorMutations()
        {
            // Arrange
            BulkBirdGenerator generator = new BulkBirdGenerator();

            // Act
            List<Bird> birds = generator.GenerateBirds(10000);

            // Assert
            foreach (Bird bird in birds)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(bird.ColorMutation));

                Assert.IsTrue(ColorMutationValidator.IsValid(bird.ColorMutation));
            }
        }

        [TestMethod]
        public void GenerateBirds_ShouldCreateValidBirdTypes()
        {
            // Arrange
            BulkBirdGenerator generator = new BulkBirdGenerator();

            // Act
            List<Bird> birds = generator.GenerateBirds(10000);

            // Assert
            foreach (Bird bird in birds)
            {
                Assert.IsTrue(System.Enum.IsDefined(typeof(BirdType), bird.Type));
            }
        }

        [TestMethod]
        public void GenerateBirds_ShouldCreateValidStatuses()
        {
            // Arrange
            BulkBirdGenerator generator = new BulkBirdGenerator();

            // Act
            List<Bird> birds = generator.GenerateBirds(10000);

            // Assert
            foreach (Bird bird in birds)
            {
                Assert.IsTrue(System.Enum.IsDefined(typeof(BirdStatus), bird.Status));
            }
        }

        [TestMethod]
        public void GenerateBirds_WithZeroCount_ShouldReturnEmptyList()
        {
            // Arrange
            BulkBirdGenerator generator = new BulkBirdGenerator();

            // Act
            List<Bird> birds = generator.GenerateBirds(0);

            // Assert
            Assert.IsNotNull(birds);
            Assert.AreEqual(0, birds.Count);
        }

        [TestMethod]
        public void GenerateBirds_WithNegativeCount_ShouldReturnEmptyList()
        {
            // Arrange
            BulkBirdGenerator generator = new BulkBirdGenerator();

            // Act
            List<Bird> birds = generator.GenerateBirds(-5);

            // Assert
            Assert.IsNotNull(birds);
            Assert.AreEqual(0, birds.Count);
        }
    }
}