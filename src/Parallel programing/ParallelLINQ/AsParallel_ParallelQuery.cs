using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.ParallelLINQ.AsParallel_ParallelQuery
{
    public class Demo {
        public static void Run()
        {
            const int count = 50;

            var items = Enumerable.Range(1, count).ToArray();
            var results = new int[count];

            // Ordinary LINQ
            // items.Select(...)

            // Parallel LINQ
            // Processed in Parallel, but no control in order. Down below is example in order
            items.AsParallel().ForAll(x => {
                int newValue = x * x * x;

                Console.Write($"{newValue} ({ Task.CurrentId})\t");
                results[x - 1] = newValue;
            });

            Console.WriteLine();

            foreach (var item in results) Console.Write($"{item}\t");

            Console.WriteLine();
            Console.WriteLine();

            // Processed in Parallel and in order
            var cubes = items.AsParallel().AsOrdered().Select(x => x * x * x);

            // Important point! - the parallel execution only happens in the foreach!
            foreach (var item in cubes) Console.Write($"{item}\t"); 
        }    
    }
}
