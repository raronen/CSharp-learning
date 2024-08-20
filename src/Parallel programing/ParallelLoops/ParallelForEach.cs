using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.ParallelLoops.ParallelForEach
{
    public class Demo
    {

        public static IEnumerable<int> Range(int start, int end, int step)
        {
            for (int i = start; i < end; i += step)
            {
                yield return i;
            }
        }

        public static void Run()
        {
            var a = new Action(() => Console.WriteLine($"First {Task.CurrentId}"));
            var b = new Action(() => Console.WriteLine($"Second {Task.CurrentId}"));
            var c = new Action(() => Console.WriteLine($"Third {Task.CurrentId}"));

            // 1st method
            Parallel.Invoke(a, b, c); // Running all in parallel. Block until all completes or exception occur.

            // 2nd method
            Parallel.For(1, 11, i => // Immediatly run 10 of the lambda in parallel.
            {
                Console.WriteLine($"{i * i}\t");
            });

            // 3rd method
            string[] words = { "oh", "what", "a", "night" };
            Parallel.ForEach(words, word => {
                Console.WriteLine($"{word} has length {words.Length} (task: {Task.CurrentId})");
            });

            // Parallel.For always jumps by 1.
            // So, we can use IEnumerbale to get around it.
            Parallel.ForEach(Range(1, 20, 3), Console.WriteLine);
        }
    }
}

