using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

//The following example counts the approximate number of words in text files
//Each task is responsible for opening a file, reading its entire contents asynchronously,and calculating the word count by using a regular expression.

//The regular expression \p{P}*\s+ matches zero, one, or more punctuation characters followed by one or more whitespace characters.
// It assumes that the total number of matches equals the approximate word count.

namespace ParallelClassExamples
{
    class IOBoundParallelTaskExecution
    {
        // to run this class, enable the main method, disable main methods of other classes 
        // of the same project
        /* public static void Main()
         {
             ShowWordCount();
         } */

        public static void ShowWordCount()
        {
            string dirNames = "C://Users//MuhammadRezaulkarim//testDir";

            DirectoryInfo temp = new DirectoryInfo(dirNames);
            Console.WriteLine(temp.Name);
            Console.WriteLine(temp.Exists);
            Console.WriteLine(temp.FullName);

            string pattern = @"\p{P}*\s+";

            var tasks = new List<Task<int>>(); // list of task where each task will return int (number of words)

            foreach (string fileName in Directory.GetFiles(dirNames))
            {
                // Task.Run Queues the specified work to run on the thread pool and returns a Task<TResult> object 
                //that represents that work.
                Console.WriteLine(fileName);

                tasks.Add(Task.Run(() => {
                    // Number of words.
                    int nWords = 0;

                    if (File.Exists(fileName))
                    {
                        StreamReader sr = new StreamReader(fileName);
                        string fileContent = sr.ReadToEndAsync().Result;
                        nWords = Regex.Matches(fileContent, pattern).Count;
                    }

                    return nWords; // this will be stored in the Result property of the associated task
                }));

            }

            //The WaitAll(Task[]) method  is called to ensure that all tasks have completed before displaying 
            //word counts of each book to the console
            Task.WaitAll(tasks.ToArray());

            Console.WriteLine("Count:" + tasks.Count);

            foreach (var t in tasks)
                Console.WriteLine("Word count:" + t.Result + " Task Id:" + t.Id + " Task Status:" + t.Status);

            Console.WriteLine("Press any key to quit...");
            Console.ReadKey();

        }
    

    }
}
