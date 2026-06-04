using System;
using System.Collections.Generic;
using System.Diagnostics;
using BirdAviaryManagement.Core.Models;

namespace BirdAviaryManagement.Core.Services
{
    public class BirdSorter
    {
        /*
         * First version - Bubble Sort - O(n^2)
         * This version was implemented first according to the TDD assignment requirements.
         * It is kept here as a comment and not deleted, as required.
         *
         * public List<Bird> SortByHatchYearDescending(List<Bird>? birds)
         * {
         *     Stopwatch stopwatch = Stopwatch.StartNew();
         *
         *     if (birds == null)
         *     {
         *         stopwatch.Stop();
         *         Console.WriteLine($"Sorting time: {stopwatch.ElapsedMilliseconds} ms");
         *         return new List<Bird>();
         *     }
         *
         *     List<Bird> sortedBirds = new List<Bird>(birds);
         *
         *     for (int i = 0; i < sortedBirds.Count - 1; i++)
         *     {
         *         for (int j = 0; j < sortedBirds.Count - i - 1; j++)
         *         {
         *             if (sortedBirds[j].HatchYear < sortedBirds[j + 1].HatchYear)
         *             {
         *                 Bird temp = sortedBirds[j];
         *                 sortedBirds[j] = sortedBirds[j + 1];
         *                 sortedBirds[j + 1] = temp;
         *             }
         *         }
         *     }
         *
         *     stopwatch.Stop();
         *     Console.WriteLine($"Sorting time: {stopwatch.ElapsedMilliseconds} ms");
         *
         *     return sortedBirds;
         * }
         */

        public List<Bird> SortByHatchYearDescending(List<Bird>? birds)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            if (birds == null)
            {
                stopwatch.Stop();
                Console.WriteLine($"Sorting time: {stopwatch.ElapsedMilliseconds} ms");
                return new List<Bird>();
            }

            List<Bird> sortedBirds = new List<Bird>(birds);

            List<Bird> result = MergeSortDescending(sortedBirds);

            stopwatch.Stop();
            Console.WriteLine($"Sorting time: {stopwatch.ElapsedMilliseconds} ms");

            return result;
        }

        private List<Bird> MergeSortDescending(List<Bird> birds)
        {
            if (birds.Count <= 1)
            {
                return birds;
            }

            int middle = birds.Count / 2;

            List<Bird> left = new List<Bird>();
            List<Bird> right = new List<Bird>();

            for (int i = 0; i < middle; i++)
            {
                left.Add(birds[i]);
            }

            for (int i = middle; i < birds.Count; i++)
            {
                right.Add(birds[i]);
            }

            left = MergeSortDescending(left);
            right = MergeSortDescending(right);

            return MergeDescending(left, right);
        }

        private List<Bird> MergeDescending(List<Bird> left, List<Bird> right)
        {
            List<Bird> result = new List<Bird>();

            int leftIndex = 0;
            int rightIndex = 0;

            while (leftIndex < left.Count && rightIndex < right.Count)
            {
                if (left[leftIndex].HatchYear >= right[rightIndex].HatchYear)
                {
                    result.Add(left[leftIndex]);
                    leftIndex++;
                }
                else
                {
                    result.Add(right[rightIndex]);
                    rightIndex++;
                }
            }

            while (leftIndex < left.Count)
            {
                result.Add(left[leftIndex]);
                leftIndex++;
            }

            while (rightIndex < right.Count)
            {
                result.Add(right[rightIndex]);
                rightIndex++;
            }

            return result;
        }
    }
}