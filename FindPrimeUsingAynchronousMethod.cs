using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace ParallelClassExamples
{
    class FindPrimeUsingAynchronousMethod
    {
          public static void Main(string[] args)
          {
              GeneratePrimeNumbersAsync();
            } 
        private static async void GeneratePrimeNumbersAsync()
        {
            Stopwatch elapsedTimeTracker = new Stopwatch();
            elapsedTimeTracker.Start();
            const int minimum = 2, maximum = 100000;

            var resultCollection = await ValidatePrimesAsync(minimum, maximum);
  
            int[] sortedPrimes = resultCollection.ToArray();

            //Array.Sort(sortedPrimes); // sort the prime numbers

            Console.WriteLine("All Prime Numbers from 2 to:" + maximum);
            Console.WriteLine("2"); // 2 is by default prime

            for (int i = 0; i < sortedPrimes.Length; i++)
                Console.WriteLine(sortedPrimes[i]);


            Console.WriteLine("Execution time (in seconds):" + elapsedTimeTracker.ElapsedMilliseconds / 1000);

            Console.WriteLine("Press any key to quit...");
            Console.ReadKey();

        }

       /*  private static async Task<IEnumerable<int>> ValidatePrimesAsync(int minimum, int maximum)
         {

             var count = maximum - minimum + 1;
             return await Task.FromResult<IEnumerable<int>>(Enumerable.Range(minimum, count).Where(i=>IsPrime(i)==true));
         } */

        /*  private static async Task<List<int>> ValidatePrimesAsync(int minimum, int maximum)
          {
              var count = maximum - minimum + 1;
              return await Task.FromResult<List<int>>(Enumerable.Range(minimum, count).Where(IsPrime).ToList());
          } */

        private static async Task<List<int>> ValidatePrimesAsync(int minimum, int maximum)
        {
            var count = maximum - minimum + 1;
  
            bool val=false;
            List<int> resultCollection = new List<int>();
            for (int i=minimum; i<=count;i++)
            {
                val = IsPrime(i);

                if (val==true)
                    resultCollection.Add(i);
            }

            return resultCollection;
        }


        static bool IsPrime(int num)
        {
            if (num % 2 == 0)
                return false;

            int divisorLimit = (int)Math.Sqrt(num); // we have at least one divisor below square root of num 

            for (int i = 3; i <= divisorLimit; i += 2)
            {
                if (num % i == 0)
                    return false;
            }
           // Console.WriteLine( num + "is" + "prime");
            return true;
        }
    }
}
