using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.WaitingForTasks
{
    public class Demo
    {
        public static void Run()
        {
            var crs = new CancellationTokenSource();
            var token = crs.Token;
            var t = new Task(() =>
            {
                Console.WriteLine("I take 5 seconds");

                for (int i = 0; i < 5; i++)
                {
                    token.ThrowIfCancellationRequested();
                    Thread.Sleep(1000);
                }

                Console.WriteLine("I'm done");
            }, token);

            Task t2 = Task.Factory.StartNew(() => Thread.Sleep(3000), token);

            //Task.WaitAll(t); <-- 1st option
            t.Wait(token); // <--- 2nd option

            Task.WaitAll(t, t2); // wait for both - "await Task.WhenAny(..)" Doesn't block the current thread
            Task.WaitAll(new[] { t, t2 }, 4000); // can also add timeout
            Task.WaitAll(new[] { t, t2 }, 4000, token); // throws Aggregate exception! Handled in ExceptionHandling.cs

            Task.WaitAny(t);

            Console.WriteLine("Main program done");
            Console.ReadKey();
        }
    }
}
