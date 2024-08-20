using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.ParallelLoops.PartitioningExample
{
    public class Demo
    {
        [Benchmark]
        public void SquarEachValue()
        {
            const int count = 1000000;
            var values = Enumerable.Range(0, count);
            var results = new int[count];

            Parallel.ForEach(values,
                // Anytime you call something like this, a delegate is beeing created.
                // For very short operation this is inefficient.
                // So we implement SquareEachValueChuncked to lower the count of delegates, and measure effiecency of those 2 methods.
                // We see SquareEachValueChuncked is much better.
                x => results[x] = (int) Math.Pow(x, 2)); 
        }

        [Benchmark]
        public void SquareEachValueChuncked()
        {
            const int count = 1000000;
            var values = Enumerable.Range(0, count);
            var results = new int[count];

            var part = Partitioner.Create(0, count, 10000);
            Parallel.ForEach(part, range =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    results[i] = (int) Math.Pow(i, 2);
                }
            });
        }

        public static void Run()
        {
            var summary = BenchmarkRunner.Run<Demo>();
            Console.WriteLine(summary);
        }
    }
}
