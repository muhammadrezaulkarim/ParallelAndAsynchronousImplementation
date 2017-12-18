using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ParallelClassExamples
{
    class FindPrimeUsingCPUBoundRunTaskThread
    {
        // to run this class, enable the main method, disable main methods of other classes 
        // of the same project
        /* public static void Main(string[] args)
         {
             GeneratePrimeNumbers();
         } */

        private static void GeneratePrimeNumbers()
        {
            Stopwatch elapsedTimeTracker = new Stopwatch();
            elapsedTimeTracker.Start();
            const int minimum = 2, maximum = 100000;

            List <Task<int>> tasks = ValidatePrimes(minimum, maximum);

            List<int> resultCollection = new List<int>();

            foreach (var t in tasks)
            {
               // Console.WriteLine("ReturnVal:" + t.Result + " Task Id:" + t.Id + " Task Status:" + t.Status);
                if (t.Result !=-1)
                {
                   resultCollection.Add(t.Result);
                }
            }

            int[] sortedPrimes = resultCollection.ToArray();

            Console.WriteLine("All Prime Numbers from 2 to:" + maximum);
            Console.WriteLine("2"); // 2 is by default prime

            for (int i = 0; i < sortedPrimes.Length; i++)
               Console.WriteLine(sortedPrimes[i]);


            Console.WriteLine("Execution time (in seconds):" + elapsedTimeTracker.ElapsedMilliseconds / 1000);

            Console.WriteLine("Press any key to quit...");
            Console.ReadKey();

        }

        private static List<Task<int>> ValidatePrimes(int minimum, int maximum)
        {
            var count = maximum - minimum + 1;
    
            var tasks = new List<Task<int>>(); 
            // list of task where each task will return an int (the prime number or -1 (if not prime))

            for (int i = minimum; i <= count;i++)
            {
                int index = i; 
                //Tricky part: if not copied in a seperate varaible
                //we get unsual results. same number processed and displayed multiple times
                // probability, when the last thread starts running,
                // the loop has already incremented i to 4, and 
                //that's the value that gets passed to IsPrime. 
                //Capturing the value of i into a separate variable 'index' and
                // using that instead should solve that issue

                tasks.Add(Task.Run(() =>  {
                    bool val = IsPrime(index);

                    if (val)
                        return index; // for prime, returns the number itself
                    else
                        return -1;  // for non-prime, returns zero
                }));
            }

            Task.WaitAll(tasks.ToArray()); // wait until all tasks finishes and then return

            return tasks;
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
            return true;
        }
    }
}
