using System;
using System.Collections.Generic;
using BirdAviaryManagement.Core.Models;

namespace BirdAviaryManagement.Core.Services
{
    // Generates large sets of random birds for performance and bulk-load testing.
    public class BulkBirdGenerator
    {
        private readonly Random random = new Random();

        private readonly BirdType[] birdTypes =
        {
            BirdType.Budgie,
            BirdType.Finch,
            BirdType.Cockatiel,
            BirdType.Canary,
            BirdType.Lovebird
        };

        private readonly BirdStatus[] birdStatuses =
        {
            BirdStatus.InAviary,
            BirdStatus.Sold,
            BirdStatus.Quarantine
        };

        private readonly string[] colors =
        {
            "Blue",
            "White",
            "Yellow",
            "Green",
            "Gray",
            "Albino",
            "Lutino",
            "Pearl",
            "Pied",
            "כחול",
            "לבן",
            "צהוב",
            "ירוק",
            "אפור"
        };

        // Generates 10,000 random birds for performance testing (or any positive count).
        public List<Bird> GenerateBirds(int count)
        {
            List<Bird> birds = new List<Bird>();

            if (count <= 0)
            {
                return birds;
            }

            int currentYear = DateTime.Now.Year;

            for (int i = 1; i <= count; i++)
            {
                Bird bird = new Bird
                {
                    RingId = CreateUniqueRingId(i),
                    Type = GetRandomBirdType(),
                    ColorMutation = GetRandomColor(),
                    HatchYear = random.Next(Bird.MinimumHatchYear, currentYear + 1),
                    Status = GetRandomBirdStatus(),
                    IsAvailableForSale = GetRandomBoolean(),
                    IsBulkGenerated = true
                };

                birds.Add(bird);
            }

            return birds;
        }

        // Predictable unique Ring IDs so bulk load never creates duplicates.
        private string CreateUniqueRingId(int index)
        {
            return (900000000L + index).ToString();
        }

        private BirdType GetRandomBirdType()
        {
            int index = random.Next(0, birdTypes.Length);
            return birdTypes[index];
        }

        private BirdStatus GetRandomBirdStatus()
        {
            int index = random.Next(0, birdStatuses.Length);
            return birdStatuses[index];
        }

        private string GetRandomColor()
        {
            int index = random.Next(0, colors.Length);
            return colors[index];
        }

        private bool GetRandomBoolean()
        {
            return random.Next(0, 2) == 1;
        }
    }
}
