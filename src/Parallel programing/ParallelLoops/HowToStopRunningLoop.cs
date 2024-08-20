using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.ParallelLoops.HowToStopRunningLoop
{
    public class Demo
    {
        public static void Demoo()
        {
            var cts = new CancellationTokenSource();
            ParallelOptions options = new ParallelOptions();
            options.CancellationToken = cts.Token;
            ParallelLoopResult result = Parallel.For(0, 100, options, (int x, ParallelLoopState state) =>
            {
                Console.WriteLine($"{x}[{Task.CurrentId}]\t");

                if (x == 2)
                {
                    cts.Cancel();
                     //state.Stop(); // 1st option: Stop the execution of the loop as soon as possible. Won't nessecarrly happen immediatly.
                    // state.Break(); // 2nd option: Won't do further iteration after this iteration(e.g if it was 7, iteration 8 won't occur) less immediatly.
                    //throw new Exception(); // 3rd option

                }
            });

            Console.WriteLine();
            Console.WriteLine($"was loop completed? {result.IsCompleted}");

            if (result.LowestBreakIteration.HasValue)
            {
                Console.WriteLine($"Lowest break iteration {result.LowestBreakIteration}");
            }
        }
        public static void Run()
        {
            try
            {
                Demoo();
            }
            catch (AggregateException ae)
            {
                ae.Handle(e =>
                {
                    Console.WriteLine(e.Message);
                    return true;
                });
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
