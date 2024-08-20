using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.WaitTask
{
    public class Demo
    {
        public static void Run()
        {
            var t = new Task(() =>
            {
                //Thread.Sleep(1000); // don't waste resources
                //SpinWait.SpinUntil() // Also pause the thread - but don't give up your space - you do wasting resource. But you are more ready to run.
            });

            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var t1 = Task.Run(() =>
            {
                Console.WriteLine("Press any key to disarm; you have 5 seconds");
                bool cancelled = token.WaitHandle.WaitOne(5000); // returns if was cancelled or timeout passed

                Console.WriteLine($"Cancel {cancelled}");
            }, token);

            Console.ReadKey();
            cts.Cancel();

            Console.WriteLine("Main program done");
            Console.ReadKey();
        }
    }
}
