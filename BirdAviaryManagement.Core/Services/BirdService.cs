using System;
using System.Collections.Generic;
using BirdAviaryManagement.Core.Models;

namespace BirdAviaryManagement.Core.Services
{
    public class BirdService
    {
        private readonly List<Bird> birds = new List<Bird>();
        private readonly IHealthService healthService;

        public BirdService()
        {
            healthService = new HealthService();
        }

        public BirdService(IHealthService healthService)
        {
            this.healthService = healthService;
        }

        public bool AddBird(Bird bird)
        {
            if (!IsValidBird(bird))
            {
                return false;
            }

            birds.Add(bird);
            return true;
        }

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

        public bool UpdateSaleAvailability(string ringId)
        {
            Bird? bird = FindBirdByRingId(ringId);

            if (bird == null)
            {
                return false;
            }

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