using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.Concurrent_Collections.ConcurrentBagExample
{
    public class Demo
    {
        public static void Run()
        {
            // ConcurrentBag is Optimized for speed
            var bag = new ConcurrentBag<int>();
            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                var i1 = i;
                Task.Factory.StartNew(() =>
                {
                    bag.Add(i1);
                    Console.WriteLine($"{Task.CurrentId} has added {i1 + 1}");

                    int result;
                    if (bag.TryPeek(out result))
                    {
                        Console.WriteLine($"{Task.CurrentId} has peeked the value {result}");
                    }
                });
            }

            Task.WaitAll(tasks.ToArray());

            // ConcurrentBag actually keeps list of elements per thread. Each thread its own bag of elements..
            /* 
             * Output:
             *  1 has added 1
                2 has added 2
                5 has added 5
                3 has added 3
                1 has peeked the value 0
                6 has added 6
                6 has peeked the value 5
                8 has added 8
                8 has peeked the value 7
                10 has added 10
                10 has peeked the value 9
                2 has peeked the value 1
                4 has added 4
                4 has peeked the value 3
                7 has added 7
                7 has peeked the value 6
                3 has peeked the value 2
                5 has peeked the value 4
                9 has added 9
                9 has peeked the value 8
            */

            Console.ReadKey();
        }
    }
}
