using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.ParallelLINQ.MergeOptionsExample
{
    public class Demo
    {
        public static void Run()
        {
            var numbers = Enumerable.Range(1, 100).ToArray();

            var results = numbers
                .AsParallel()
                .WithMergeOptions(ParallelMergeOptions.NotBuffered) // <-- Options for merge. E.g this will give us items as soon as possible
                .Select(x =>
                {
                    var result = Math.Log10(x);
                    Console.WriteLine($"P {result}\t");
                    return result;
                });

            foreach (var item in results)
            {
                // Some items get here before all results went through the Select in the LINQ.
                // Great example to show the parallel of iterations in the foreach.
                // See output, there are C's before P's
                Console.WriteLine($"C {item}\t");
            }
        }
    }
}
