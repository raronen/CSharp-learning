using DesignPatterns.Stepwise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.ParallelLINQ.Cancellation_Exceptions
{
    public class Demo
    {
        public static void Run()
        {
            var cts = new CancellationTokenSource();
            var items = ParallelEnumerable.Range(0, 20);

            // Entire sequence choose Parallel operators. E.g this Select is Parallel.
            var results = items.WithCancellation(cts.Token).Select(i =>
            {
                double result = Math.Log10(i);

                //if (result > 1) throw new InvalidOperationException();

                Console.WriteLine($"i = {i}, tid = {Task.CurrentId}");
                return result;
            });

            try
            {
                foreach (var c in results)
                {
                    if (c > 1)
                    {
                        cts.Cancel();
                    }
                    Console.WriteLine($"result = {c}");
                }
            } catch (AggregateException ae)
            {
                ae.Handle(e =>
                {
                    Console.WriteLine($"{e.GetType().Name}: {e.Message}");
                    return true;
                });
            } catch (OperationCanceledException e)
            {
                Console.WriteLine("Cancelled");
            }
        }
    }
}
