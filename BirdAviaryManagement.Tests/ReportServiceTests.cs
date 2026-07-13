using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BirdAviaryManagement.Core.Models;
using BirdAviaryManagement.Core.Services;

namespace BirdAviaryManagement.Tests
{
    [TestClass]
    public class ReportServiceTests
    {
        [TestMethod]
        public void CountBirds_WithThreeBirds_ShouldReturnThree()
        {
            // Arrange
            ReportService reportService = new ReportService();

            List<Bird> birds = new List<Bird>
            {
                new Bird
                {
                    RingId = "1001",
                    Type = BirdType.Budgie,
                    ColorMutation = "Blue",
                    HatchYear = 2020,
                    Status = BirdStatus.InAviary,
                    IsAvailableForSale = true
                },
                new Bird
                {
                    RingId = "1002",
                    Type = BirdType.Finch,
                    ColorMutation = "White",
                    HatchYear = 2021,
                    Status = BirdStatus.InAviary,
                    IsAvailableForSale = false
                },
                new Bird
                {
                    RingId = "1003",
                    Type = BirdType.Cockatiel,
                    ColorMutation = "Yellow",
                    HatchYear = 2022,
                    Status = BirdStatus.Quarantine,
                    IsAvailableForSale = false
                }
            };

            // Act
            int result = reportService.CountBirds(birds);

            // Assert
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void CountBirds_WithEmptyList_ShouldReturnZero()
        {
            // Arrange
            ReportService reportService = new ReportService();
            List<Bird> birds = new List<Bird>();

            // Act
            int result = reportService.CountBirds(birds);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CountAvailableForSale_WithTwoAvailableBirds_ShouldReturnTwo()
        {
            // Arrange
            ReportService reportService = new ReportService();

            List<Bird> birds = new List<Bird>
            {
                new Bird
                {
                    RingId = "1001",
                    Type = BirdType.Budgie,
                    ColorMutation = "Blue",
                    HatchYear = 2020,
                    Status = BirdStatus.InAviary,
                    IsAvailableForSale = true
                },
                new Bird
                {
                    RingId = "1002",
                    Type = BirdType.Finch,
                    ColorMutation = "White",
                    HatchYear = 2021,
                    Status = BirdStatus.InAviary,
                    IsAvailableForSale = true
                },
                new Bird
                {
                    RingId = "1003",
                    Type = BirdType.Cockatiel,
                    ColorMutation = "Yellow",
                    HatchYear = 2022,
                    Status = BirdStatus.Quarantine,
                    IsAvailableForSale = false
                }
            };

            // Act
            int result = reportService.CountAvailableForSale(birds);

            // Assert
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void CountAvailableForSale_WithNoAvailableBirds_ShouldReturnZero()
        {
            // Arrange
            ReportService reportService = new ReportService();

            List<Bird> birds = new List<Bird>
            {
                new Bird
                {
                    RingId = "1001",
                    Type = BirdType.Budgie,
                    ColorMutation = "Blue",
                    HatchYear = 2020,
                    Status = BirdStatus.InAviary,
                    IsAvailableForSale = false
                },
                new Bird
                {
                    RingId = "1002",
                    Type = BirdType.Finch,
                    ColorMutation = "White",
                    HatchYear = 2021,
                    Status = BirdStatus.InAviary,
                    IsAvailableForSale = false
                }
            };

            // Act
            int result = reportService.CountAvailableForSale(birds);

            // Assert
            Assert.AreEqual(0, result);
        }

        // Unit test verifying average age calculation (currentYear - HatchYear).
        [TestMethod]
        public void CalculateAverageAge_WithValidBirds_ShouldReturnCorrectAverage()
        {
            // Arrange
            ReportService reportService = new ReportService();

            List<Bird> birds = new List<Bird>
            {
                new Bird
                {
                    RingId = "1001",
                    Type = BirdType.Budgie,
                    ColorMutation = "Blue",
                    HatchYear = 2020,
                    Status = BirdStatus.InAviary,
                    IsAvailableForSale = false
                },
                new Bird
                {
                    RingId = "1002",
                    Type = BirdType.Finch,
                    ColorMutation = "White",
                    HatchYear = 2022,
                    Status = BirdStatus.InAviary,
                    IsAvailableForSale = false
                },
                new Bird
                {
                    RingId = "1003",
                    Type = BirdType.Cockatiel,
                    ColorMutation = "Yellow",
                    HatchYear = 2024,
                    Status = BirdStatus.InAviary,
                    IsAvailableForSale = false
                }
            };

            int currentYear = 2026;

            // Ages: 6, 4, 2
            // Average: 4

            // Act
            double result = reportService.CalculateAverageAge(birds, currentYear);

            // Assert
            Assert.AreEqual(4.0, result);
        }

        [TestMethod]
        public void CalculateAverageAge_WithEmptyList_ShouldReturnZero()
        {
            // Arrange
            ReportService reportService = new ReportService();
            List<Bird> birds = new List<Bird>();
            int currentYear = 2026;

            // Act
            double result = reportService.CalculateAverageAge(birds, currentYear);

            // Assert
            Assert.AreEqual(0.0, result);
        }

        [TestMethod]
        public void CalculateAverageAge_WithOneBird_ShouldReturnCorrectAge()
        {
            // Arrange
            ReportService reportService = new ReportService();

            List<Bird> birds = new List<Bird>
            {
                new Bird
                {
                    RingId = "1001",
                    Type = BirdType.Canary,
                    ColorMutation = "Green",
                    HatchYear = 2021,
                    Status = BirdStatus.InAviary,
                    IsAvailableForSale = true
                }
            };

            int currentYear = 2026;

            // Act
            double result = reportService.CalculateAverageAge(birds, currentYear);

            // Assert
            Assert.AreEqual(5.0, result);
        }

        [TestMethod]
        public void CreateInventoryReport_WithValidBirds_ShouldReturnCorrectReport()
        {
            // Arrange
            ReportService reportService = new ReportService();

            List<Bird> birds = new List<Bird>
            {
                new Bird
                {
                    RingId = "1001",
                    Type = BirdType.Budgie,
                    ColorMutation = "Blue",
                    HatchYear = 2020,
                    Status = BirdStatus.InAviary,
                    IsAvailableForSale = true
                },
                new Bird
                {
                    RingId = "1002",
                    Type = BirdType.Finch,
                    ColorMutation = "White",
                    HatchYear = 2022,
                    Status = BirdStatus.InAviary,
                    IsAvailableForSale = false
                },
                new Bird
                {
                    RingId = "1003",
                    Type = BirdType.Cockatiel,
                    ColorMutation = "Yellow",
                    HatchYear = 2024,
                    Status = BirdStatus.InAviary,
                    IsAvailableForSale = true
                }
            };

            int currentYear = 2026;

            // Act
            InventoryReport report = reportService.CreateInventoryReport(birds, currentYear);

            // Assert
            Assert.IsNotNull(report);
            Assert.AreEqual(3, report.TotalBirds);
            Assert.AreEqual(4.0, report.AverageAge);
            Assert.AreEqual(2, report.AvailableForSaleCount);
        }

        [TestMethod]
        public void CreateInventoryReport_WithValidBirds_ShouldReturnSortedBirdsDescending()
        {
            // Arrange
            ReportService reportService = new ReportService();

            List<Bird> birds = new List<Bird>
            {
                new Bird
                {
                    RingId = "1001",
                    Type = BirdType.Budgie,
                    ColorMutation = "Blue",
                    HatchYear = 2020,
                    Status = BirdStatus.InAviary,
                    IsAvailableForSale = true
                },
                new Bird
                {
                    RingId = "1002",
                    Type = BirdType.Finch,
                    ColorMutation = "White",
                    HatchYear = 2024,
                    Status = BirdStatus.InAviary,
                    IsAvailableForSale = false
                },
                new Bird
                {
                    RingId = "1003",
                    Type = BirdType.Cockatiel,
                    ColorMutation = "Yellow",
                    HatchYear = 2022,
                    Status = BirdStatus.InAviary,
                    IsAvailableForSale = true
                }
            };

            int currentYear = 2026;

            // Act
            InventoryReport report = reportService.CreateInventoryReport(birds, currentYear);

            // Assert
            Assert.IsNotNull(report);
            Assert.IsNotNull(report.SortedBirds);
            Assert.AreEqual(3, report.SortedBirds.Count);

            Assert.AreEqual(2024, report.SortedBirds[0].HatchYear);
            Assert.AreEqual(2022, report.SortedBirds[1].HatchYear);
            Assert.AreEqual(2020, report.SortedBirds[2].HatchYear);
        }

        [TestMethod]
        public void CreateInventoryReport_With10000Birds_ShouldReturnCorrectReport()
        {
            // Arrange
            BulkBirdGenerator generator = new BulkBirdGenerator();
            ReportService reportService = new ReportService();

            List<Bird> birds = generator.GenerateBirds(10000);
            int currentYear = System.DateTime.Now.Year;

            // Act
            InventoryReport report = reportService.CreateInventoryReport(birds, currentYear);

            // Assert
            Assert.IsNotNull(report);
            Assert.AreEqual(10000, report.TotalBirds);
            Assert.AreEqual(10000, report.SortedBirds.Count);

            for (int i = 0; i < report.SortedBirds.Count - 1; i++)
            {
                Assert.IsTrue(report.SortedBirds[i].HatchYear >= report.SortedBirds[i + 1].HatchYear);
            }
        }

        [TestMethod]
        public void Sort10000Birds_ShouldFinishUnder9Seconds()
        {
            // Arrange
            BulkBirdGenerator generator = new BulkBirdGenerator();
            ReportService reportService = new ReportService();

            List<Bird> birds = generator.GenerateBirds(10000);
            int currentYear = System.DateTime.Now.Year;

            Stopwatch stopwatch = Stopwatch.StartNew();

            // Act
            InventoryReport report = reportService.CreateInventoryReport(birds, currentYear);

            stopwatch.Stop();

            // Assert
            Assert.IsNotNull(report);
            Assert.AreEqual(10000, report.SortedBirds.Count);

            System.Console.WriteLine($"Sorting 10,000 birds took: {stopwatch.ElapsedMilliseconds} ms");

            Assert.IsTrue(
                stopwatch.ElapsedMilliseconds < 9000,
                $"Sorting took too long: {stopwatch.ElapsedMilliseconds} ms"
            );
        }
    }
}