using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace ParallelClassExamples
{
    class FindPrimeUsingParallelForLoop
    {
        // to run this class, enable the main method, disable main methods of other classes 
        // of the same project
        static void Main(string[] args)
           {
                GeneratePrimeNumbers();
           } 

        private static void GeneratePrimeNumbers()
        {
            Stopwatch elapsedTimeTracker = new Stopwatch();
            elapsedTimeTracker.Start();

            const int minimum = 2, maximum = 100000;

            //Represents a thread-safe unordered collection of objects.
            var resultCollection = new ConcurrentBag<string>();
            // prime number checking starts from 2 
            ParallelLoopResult result = Parallel.For(minimum, maximum, i =>
            {

                if (IsPrime(i))
                    resultCollection.Add(i.ToString());   // prime numbers of will be added in random order. we cannot control
            });

            //now process the results
            string[] unsortedPrimes = resultCollection.ToArray(); // contains prime numbers in string format in random order

            int[] sortedPrimes = new int[unsortedPrimes.Length];

            for (int i = 0; i < sortedPrimes.Length; i++)
                sortedPrimes[i] = int.Parse(unsortedPrimes[i]);

            Array.Sort(sortedPrimes); // sort the prime numbers

            Console.WriteLine("All Prime Numbers from 2 to:" + maximum);
            Console.WriteLine("2"); // 2 is by default prime
            for (int i = 0; i < sortedPrimes.Length; i++)
                Console.WriteLine(sortedPrimes[i]);


            Console.WriteLine("Execution time (in seconds):" + elapsedTimeTracker.ElapsedMilliseconds / 1000);

            Console.WriteLine("Press any key to quit...");
            Console.ReadKey();

        }

        static bool IsPrime(int num)
        {
            if (num % 2 == 0)
                return false;

            int divisorLimit = (int)Math.Sqrt(num); // we have at least one divisior below square root of num 

            for (int i = 3; i <= divisorLimit; i += 2)
            {
                if (num % i == 0)
                    return false;
            }

            return true;
        }
    }
}
