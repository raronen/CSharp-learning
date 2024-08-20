using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.ParallelLoops.ThreadLocalStorageExample
{
    public class Demo
    {
        public static void Run()
        {
            // Example - it'll be more effcient to calculate the sum in each thread and only than interlocked the sum
            // Shared value
            int sum_bad_example = 0;
            Parallel.For(1, 1001, x =>
            {
                Interlocked.Add(ref sum_bad_example, x);
            });


            int sum = 0;
            Parallel.For(1, 1001, /*local storage just for this thread*/
                () => 0 // 0 is the value each thread is going to increase
                , (x /*counter*/, state, tls /*current value of thread local value*/ ) =>
                    {
                        tls += x;
                        Console.WriteLine($"Task {Task.CurrentId} has sum {tls}");
                        return tls;
                    }, partialSum =>
                    {
                        Console.WriteLine($"Partial value of task {Task.CurrentId} is {partialSum}");
                        Interlocked.Add(ref sum, partialSum);
                    });

            Console.WriteLine($"Sum of 1..100 = {sum}");
        }
    }
}
