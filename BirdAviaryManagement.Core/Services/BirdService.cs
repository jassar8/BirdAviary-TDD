using System;
using System.Collections.Generic;
using BirdAviaryManagement.Core.Models;

namespace BirdAviaryManagement.Core.Services
{
    // Central business-logic service for managing the bird inventory.
    // Uses IHealthService so tests can inject a Moq mock instead of the real service.
    public class BirdService
    {
        private readonly List<Bird> birds = new List<Bird>();
        private readonly IHealthService healthService;

        // Production constructor: uses the real HealthService implementation.
        public BirdService()
        {
            healthService = new HealthService();
        }

        // Test/DI constructor: accepts any IHealthService (including a Moq mock).
        public BirdService(IHealthService healthService)
        {
            this.healthService = healthService;
        }

        // Validates the bird and adds it to inventory if all rules pass.
        public bool AddBird(Bird bird)
        {
            if (!IsValidBird(bird))
            {
                return false;
            }

            birds.Add(bird);
            return true;
        }

        // Adds multiple birds one by one (used by bulk load of 10,000 birds).
        public int AddBirds(List<Bird> birdsToAdd)
        {
            if (birdsToAdd == null)
            {
                return 0;
            }

            int addedCount = 0;

            foreach (Bird bird in birdsToAdd)
            {
                bool added = AddBird(bird);

                if (added)
                {
                    addedCount++;
                }
            }

            return addedCount;
        }

        // Asks the health service whether the bird may be marked available for sale.
        public bool UpdateSaleAvailability(string ringId)
        {
            Bird? bird = FindBirdByRingId(ringId);

            if (bird == null)
            {
                return false;
            }

            // Calls the external health service (mocked in unit tests).
            bool isHealthy = healthService.IsBirdHealthyForSale(ringId);

            if (isHealthy)
            {
                bird.IsAvailableForSale = true;
                return true;
            }

            bird.IsAvailableForSale = false;
            return false;
        }

        public List<Bird> GetAllBirds()
        {
            return new List<Bird>(birds);
        }

        // Removes only bulk-generated birds; keeps manually entered records.
        public int ClearBulkBirds()
        {
            int removedCount = birds.RemoveAll(bird => bird.IsBulkGenerated);
            return removedCount;
        }

        private Bird? FindBirdByRingId(string ringId)
        {
            foreach (Bird bird in birds)
            {
                if (bird.RingId == ringId)
                {
                    return bird;
                }
            }

            return null;
        }

        // Validates the user's input before creating/accepting a new bird.
        private bool IsValidBird(Bird bird)
        {
            if (bird == null)
            {
                return false;
            }

            if (!IsValidRingId(bird.RingId))
            {
                return false;
            }

            // Prevent duplicate Ring IDs in the aviary.
            if (IsDuplicateRingId(bird.RingId))
            {
                return false;
            }

            if (!IsValidColorMutation(bird.ColorMutation))
            {
                return false;
            }

            if (!IsValidHatchYear(bird.HatchYear))
            {
                return false;
            }

            return true;
        }

        private bool IsValidRingId(string ringId)
        {
            return RingIdValidator.IsValid(ringId);
        }

        // Prevent duplicate Ring IDs in the aviary.
        private bool IsDuplicateRingId(string ringId)
        {
            foreach (Bird bird in birds)
            {
                if (bird.RingId == ringId)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsValidColorMutation(string colorMutation)
        {
            return ColorMutationValidator.IsValid(colorMutation);
        }

        // Hatch year must be between MinimumHatchYear (2000) and the current year.
        private bool IsValidHatchYear(int hatchYear)
        {
            int currentYear = DateTime.Now.Year;

            if (hatchYear < Bird.MinimumHatchYear)
            {
                return false;
            }

            if (hatchYear > currentYear)
            {
                return false;
            }

            return true;
        }
    }
}
