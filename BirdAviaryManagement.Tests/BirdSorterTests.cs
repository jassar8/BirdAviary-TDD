using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using BirdAviaryManagement.Core.Models;
using BirdAviaryManagement.Core.Services;

namespace BirdAviaryManagement.Tests
{
    [TestClass]
    public class BirdSorterTests
    {
        [TestMethod]
        public void SortByHatchYearDescending_WithValidBirds_ShouldReturnNotNull()
        {
            // Arrange
            BirdSorter sorter = new BirdSorter();

            List<Bird> birds = new List<Bird>
            {
                CreateBird("1001", 2020),
                CreateBird("1002", 2024),
                CreateBird("1003", 2022)
            };

            // Act
            List<Bird> result = sorter.SortByHatchYearDescending(birds);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void SortByHatchYearDescending_ShouldNotLoseRecords()
        {
            // Arrange
            BirdSorter sorter = new BirdSorter();

            List<Bird> birds = new List<Bird>
            {
                CreateBird("1001", 2020),
                CreateBird("1002", 2024),
                CreateBird("1003", 2022),
                CreateBird("1004", 2021)
            };

            int originalCount = birds.Count;

            // Act
            List<Bird> result = sorter.SortByHatchYearDescending(birds);

            // Assert
            Assert.AreEqual(originalCount, result.Count);
        }

        [TestMethod]
        public void SortByHatchYearDescending_ShouldSortBirdsDescending()
        {
            // Arrange
            BirdSorter sorter = new BirdSorter();

            List<Bird> birds = new List<Bird>
            {
                CreateBird("1001", 2020),
                CreateBird("1002", 2025),
                CreateBird("1003", 2022),
                CreateBird("1004", 2024)
            };

            // Act
            List<Bird> result = sorter.SortByHatchYearDescending(birds);

            // Assert
            for (int i = 0; i < result.Count - 1; i++)
            {
                Assert.IsTrue(result[i].HatchYear >= result[i + 1].HatchYear);
            }
        }

        [TestMethod]
        public void SortByHatchYearDescending_WithEmptyList_ShouldReturnEmptyList()
        {
            // Arrange
            BirdSorter sorter = new BirdSorter();
            List<Bird> birds = new List<Bird>();

            // Act
            List<Bird> result = sorter.SortByHatchYearDescending(birds);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void SortByHatchYearDescending_WithOneBird_ShouldReturnOneBird()
        {
            // Arrange
            BirdSorter sorter = new BirdSorter();

            List<Bird> birds = new List<Bird>
            {
                CreateBird("1001", 2020)
            };

            // Act
            List<Bird> result = sorter.SortByHatchYearDescending(birds);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("1001", result[0].RingId);
        }

        [TestMethod]
        public void SortByHatchYearDescending_WithSameYears_ShouldKeepAllRecords()
        {
            // Arrange
            BirdSorter sorter = new BirdSorter();

            List<Bird> birds = new List<Bird>
            {
                CreateBird("1001", 2022),
                CreateBird("1002", 2022),
                CreateBird("1003", 2022)
            };

            // Act
            List<Bird> result = sorter.SortByHatchYearDescending(birds);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(2022, result[i].HatchYear);
            }
        }

        [TestMethod]
        public void SortByHatchYearDescending_WithNullList_ShouldReturnEmptyList()
        {
            // Arrange
            BirdSorter sorter = new BirdSorter();

            // Act
            List<Bird> result = sorter.SortByHatchYearDescending(null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        private Bird CreateBird(string ringId, int hatchYear)
        {
            return new Bird
            {
                RingId = ringId,
                Type = BirdType.Budgie,
                ColorMutation = "Blue",
                HatchYear = hatchYear,
                Status = BirdStatus.InAviary,
                IsAvailableForSale = false
            };
        }
    }
}