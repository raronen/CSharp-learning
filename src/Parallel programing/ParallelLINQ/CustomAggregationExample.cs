using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.ParallelLINQ.CustomAggregationExample
{
    public class Demo
    {
        public static void Run()
        {
            // Regular - Sequental:
            // var sum = Enumerable.Range(1, 1000).Sum();

            // Regular - Sequental:
            // var sum = Enumerable.Range(1, 1000)
            //     .Aggregate(0, (i, acc) =>  i + acc);

            var sum = ParallelEnumerable.Range(1, 1000)
                .Aggregate(
                    // Starting with 0. (if we do "partialSum * i" than we should start with 1)
                    0,
                    // Lots of tasks (threads) will process this
                    (partialSum, i) => partialSum + i,
                    // Combine the results of all tasks (threads) to single value.
                    (total, subtotal) => total + subtotal,
                    // Post-processing the final result before returning it.
                    i => i);

            Console.WriteLine($"Sum: {sum}");
        }
    }
}
